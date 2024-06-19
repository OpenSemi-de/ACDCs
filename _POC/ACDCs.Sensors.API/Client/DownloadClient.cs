namespace ACDCs.Sensors.API.Client;

using System.Collections;
using System.Collections.Concurrent;
using Interfaces;
using Newtonsoft.Json;
using Samples;

public class DownloadClient
{
    private readonly int _downloadDelay;
    private readonly string _outputPath;
    private readonly Type _resultType;
    private readonly Dictionary<string, List<ISample>> _sampleFiles;
    private readonly Uri _uri;
    private bool _isRunning = true;

    public ConcurrentQueue<ISample?> SampleCache { get; set; }

    public DownloadClient(Uri uri, string outputPath, Type resultType, int downloadDelay = 1000)
    {
        _uri = uri;
        _outputPath = outputPath;
        _resultType = resultType;
        _downloadDelay = downloadDelay;
        SampleCache = new ConcurrentQueue<ISample?>();
        _sampleFiles = new Dictionary<string, List<ISample>>();
    }

    public static async Task<List<SensorItem>?> GetSensorAvailability(Uri baseUrl)
    {
        HttpClient httpClient = new();
        List<SensorItem>? availability = new();
        Uri avUri = new($"{baseUrl.AbsoluteUri}Sensors/Availability");
        try
        {
            HttpResponseMessage response = await httpClient.GetAsync(avUri);
            if (response.IsSuccessStatusCode)
            {
                string source = await response.Content.ReadAsStringAsync();
                availability =
                    JsonConvert.DeserializeObject(source, typeof(List<SensorItem>)) as List<SensorItem>;
            }
        }
#pragma warning disable CS0168
        catch (Exception ex)
#pragma warning restore CS0168
        {
            // ignored
        }

        httpClient.Dispose();

        return availability;
    }

    public void Start()
    {
        Thread loop = new Thread(Loop);
        loop.Start();
    }

    public void Stop()
    {
        _isRunning = false;
    }

    private void AddToCache(List<ISample?> samples)
    {
        if (samples.Count == 0) return;
        ISample latestSample = SampleCache.OrderBy(s => s.Time).LastOrDefault() ?? new CompassSample() { Time = DateTime.MinValue };

        List<ISample?> updates = samples.Where(s => s != null && s.Time > latestSample.Time).ToList();
        foreach (ISample? update in updates)
        {
            if (update != null)
            {
                SampleCache.Enqueue(update);
            }
        }

        Task.Run(() =>
        {
            while (SampleCache.Count > 2000)
            {
                SampleCache.TryDequeue(out _);
            }
        });
    }

    private void CheckDirectories(string filePath, string dateTimePath)
    {
        if (!Directory.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
        }

        if (!Directory.Exists(Path.Combine(filePath, dateTimePath)))
        {
            Directory.CreateDirectory(Path.Combine(filePath, dateTimePath));
        }
    }

    private async Task<List<ISample>> GetSamplesFromServer()
    {
        HttpClient httpClient = new();
        ISample latestSample = SampleCache.ToList().OrderBy(s => s.Time).LastOrDefault() ?? new CompassSample() { Time = DateTime.MinValue };

        List<ISample> samples = new();
        try
        {
            HttpResponseMessage response = await httpClient.GetAsync($"{_uri}?date={latestSample.Time}");
            if (response.IsSuccessStatusCode)
            {
                string source = await response.Content.ReadAsStringAsync();
                object? isamples = JsonConvert.DeserializeObject(source, _resultType);
                if (isamples is IList list)
                {
                    samples.AddRange(list.Cast<object>().Select(sample => sample as ISample));
                }
            }
        }
#pragma warning disable CS0168
        catch (Exception ex)
#pragma warning restore CS0168
        {
            // ignored
        }

        httpClient.Dispose();

        return samples;
    }

    private async void Loop()
    {
        await Task.Delay(100);
        while (_isRunning)
        {
            List<ISample> samples = await GetSamplesFromServer();
            if (!string.IsNullOrEmpty(_outputPath))
            {
                PutToOutputPath(samples);
                SaveFiles();

                Thread.Sleep(_downloadDelay);
            }
            else
            {
                AddToCache(samples);
                await Task.Delay(_downloadDelay);
            }
        }
    }

    private void PutSampleToFile(string filePath, ISample sample)
    {
        // ReSharper disable once InvertIf
        if (sample.Time is var date)
        {
            string Z(int num) => num < 10 ? $"0{num}" : $"{num}";
            string dateTimePath = $"{Z(date.Year)}{Z(date.Month)}{Z(date.Day)}";
            string fileName = $"{Z(date.Hour)}{Z(date.Minute)}.json";
            CheckDirectories(filePath, dateTimePath);

            string outputFilePath = Path.Combine(filePath, dateTimePath, fileName);

            if (!_sampleFiles.ContainsKey(outputFilePath))
            {
                if (File.Exists(outputFilePath))
                {
                    SaveFiles();
                    string fileData = File.ReadAllText(outputFilePath);
                    _sampleFiles.Add(outputFilePath, (JsonConvert.DeserializeObject(fileData, _resultType) as ISample[] ?? Array.Empty<ISample>()).ToList());
                }
                else
                {
                    _sampleFiles.Add(outputFilePath, new List<ISample>());
                }
            }

            _sampleFiles[outputFilePath].Add(sample);
        }
    }

    private void PutToOutputPath(List<ISample> samples)
    {
        if (samples.Count <= 0)
        {
            return;
        }

        if (!Directory.Exists(_outputPath))
        {
            return;
        }

        string typeName = samples.First().GetType().Name;
        string filePath = Path.Combine(_outputPath, typeName);
        foreach (ISample sample in samples)
        {
            PutSampleToFile(filePath, sample);
        }
    }

    private void SaveFiles()
    {
        foreach (var sampleFile in _sampleFiles.ToList())
        {
            List<ISample> orderedSampleFile = sampleFile.Value.OrderBy(x => x.Time).ToList();
            orderedSampleFile = orderedSampleFile.DistinctBy(s => s.Time).ToList();

            File.WriteAllText(sampleFile.Key, JsonConvert.SerializeObject(orderedSampleFile));
            _sampleFiles.Remove(sampleFile.Key);
        }
    }
}
