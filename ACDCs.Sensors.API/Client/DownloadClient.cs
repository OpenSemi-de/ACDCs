namespace ACDCs.Sensors.API.Client;

using System.Collections;
using Interfaces;
using Newtonsoft.Json;

public class DownloadClient
{
    private readonly int _downloadDelay;
    private readonly string _outputPath;
    private readonly Type _resultType;
    private readonly Dictionary<string, List<ISample>> _sampleFiles;
    private readonly Uri _uri;
    private bool _isRunning;
    private List<ISample> _sampleCache;

    public DownloadClient(Uri uri, string outputPath, Type resultType, int downloadDelay = 1000)
    {
        _uri = uri;
        _outputPath = outputPath;
        _resultType = resultType;
        _downloadDelay = downloadDelay;
        _sampleCache = new List<ISample>();
        _sampleFiles = new Dictionary<string, List<ISample>>();
    }

    public List<ISample> SampleCache
    {
        get => _sampleCache;
        set => _sampleCache = value;
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

    public async Task Start()
    {
        _isRunning = true;
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

    public void Stop()
    {
        _isRunning = false;
    }

    private void AddToCache(List<ISample> samples)
    {
        _sampleCache = _sampleCache
            .Union(samples)
            .DistinctBy(s => s.Time)
            .OrderByDescending(s => s.Time)
            .Take(5000)
            .OrderBy(s => s.Time)
            .ToList();
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

        List<ISample> samples = new();
        try
        {
            HttpResponseMessage response = await httpClient.GetAsync(_uri);
            if (response.IsSuccessStatusCode)
            {
                string source = await response.Content.ReadAsStringAsync();
                var isamples = JsonConvert.DeserializeObject(source, _resultType);
                foreach (var sample in isamples as IList)
                {
                    samples.Add(sample as ISample);
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
