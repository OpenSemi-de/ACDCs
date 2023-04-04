namespace ACDCs.API.Core.Components.Simulation;

using System.Collections.ObjectModel;

public class SimulationLogView : Grid
{
    private readonly Button _closeButton;
    private readonly ListView _logList;
    private ObservableCollection<SimulationLogEntry> _logEntries = new();

    public SimulationLogView()
    {
        RowDefinition[] rows =
        {
            new (),
            new(30)
        };

        this.RowDefinitions(rows);

        _logList = new ListView()
            .ItemsSource(_logEntries)
            .ItemTemplate(new DataTemplate(() =>
            {
                ViewCell cell = new ViewCell();
                Grid grid = new Grid();
                Label label = new Label();
                label.Bind(Label.TextProperty, "Text");
                grid.Add(label);
                cell.Add(grid);
                return cell;
            }));

        this.Add(_logList);

        _closeButton = new Button("Close");
        this.Add(_closeButton, 0, 1);
    }

    public void AddLog(string text)
    {
        _logEntries.Add(new(DateTime.Now, text));
    }
}

public class SimulationLogEntry
{
    public DateTime Date { get; set; }

    public string Text { get; set; }

    public SimulationLogEntry(DateTime date, string text)
    {
        Date = date;
        Text = text;
    }
}
