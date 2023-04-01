#pragma warning disable CS0649

namespace ACDCs.API.Core.Components.Simulation;

using ACDCs.API.Core.Components.Edit;

internal class SimulationControlView : Grid
{
    private EditButton? _addRemoveWatchButton;
    private EditButton? _rewindButton;
    private int _row = 0;
    private EditButton? _showGraphsButton;
    private EditButton? _startStopButton;

    public SimulationControlView()
    {
        this.Margin(2)
            .Padding(2)
            .RowSpacing(2)
            .ColumnSpacing(2);

        AddButton(_startStopButton, "Start", "Stop", OnStartStopClicked, OnStartStopEnabled);
        AddButton(_rewindButton, "Rewind", "", OnRewindClicked, OnRewindEnabled);
        AddButton(_addRemoveWatchButton, "Add watch", "Remove watch", OnAddWatchClicked, OnAddWatchEnabled);
        AddButton(_showGraphsButton, "Show graph", "Hide graph", OnShowGraphClicked, OnShowGraphEnabled);
    }

    // ReSharper disable once RedundantAssignment
    private void AddButton(EditButton? editButton, string text, string textEnabled, Action buttonAction,
        Action<EditButton> selectedAction)
    {
        editButton = new EditButton(text, buttonAction, selectedAction, 84, 60, textEnabled != "");
        this.RowDefinitions.Add(new RowDefinition());
        this.Add(editButton, 0, _row);
        _row++;
    }

    private void OnAddWatchClicked()
    {
    }

    private void OnAddWatchEnabled(EditButton obj)
    {
    }

    private void OnRewindClicked()
    {
    }

    private void OnRewindEnabled(EditButton obj)
    {
    }

    private void OnShowGraphClicked()
    {
    }

    private void OnShowGraphEnabled(EditButton obj)
    {
    }

    private void OnStartStopClicked()
    {
    }

    private void OnStartStopEnabled(EditButton obj)
    {
    }
}
