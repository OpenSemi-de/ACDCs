#pragma warning disable CS0649

namespace ACDCs.API.Core.Components.Simulation;

using CircuitRenderer.Interfaces;
using Edit;
using UraniumUI.Icons.FontAwesome;

internal class SimulationControlView : Grid
{
    private readonly EditButton? _addRemoveWatchButton;
    private readonly EditButton? _rewindButton;
    private readonly EditButton? _showGraphsButton;
    private readonly EditButton? _showLogButton;
    private readonly EditButton? _startStopButton;
    private IWorksheetItem? _item;
    private int _row = 0;
    private SimulationController? _simulation;

    public Action<bool>? OnGraphVisibilityChanged { get; set; }
    public Action<bool>? OnLogVisibilityChanged { get; set; }

    public SimulationControlView()
    {
        this.Margin(2)
            .Padding(2)
            .RowSpacing(2)
            .ColumnSpacing(2);

        AddButton(ref _startStopButton, Solid.Play, Solid.Stop, OnStartStopClicked, OnStartStopEnabled, OnStartStopDisabled);
        AddButton(ref _rewindButton, Solid.Backward, "", OnRewindClicked, OnRewindEnabled, null);
        AddButton(ref _addRemoveWatchButton, Solid.MagnifyingGlassPlus, Solid.MagnifyingGlassMinus, OnAddWatchClicked, OnAddWatchEnabled, OnAddWatchDisabled);
        AddButton(ref _showGraphsButton, Solid.WaveSquare, Solid.CircleXmark, OnShowGraphClicked, OnShowGraphEnabled, OnShowGraphDisabled);
        AddButton(ref _showLogButton, Solid.ClipboardList, Solid.ClipboardCheck, OnShowLogClicked, OnShowLogEnabled, OnShowLogDisabled);
    }

    public void SelectItem(IWorksheetItem? item)
    {
        _item = item;
        if (_simulation == null)
        {
            return;
        }

        if (item != null && _simulation.HasGraph(item))
        {
            _addRemoveWatchButton?.Select();
        }
        else
        {
            _addRemoveWatchButton?.Deselect();
        }
    }

    public void SetSimulation(SimulationController simulation)
    {
        _simulation = simulation;
    }

    // ReSharper disable once RedundantAssignment
    private void AddButton(ref EditButton? editButton, string text, string textEnabled, Action buttonAction,
        Action<EditButton>? selectedAction, Action<EditButton>? deselectedAction)
    {
        editButton = new EditButton(text, buttonAction, selectedAction, deselectedAction, 74, 50, textEnabled != "", textEnabled, "FASolid", 26);
        this.RowDefinitions.Add(new RowDefinition());
        this.Add(editButton, 0, _row);
        _row++;
    }

    private void OnAddWatchClicked()
    {
    }

    private void OnAddWatchDisabled(EditButton obj)
    {
        if (_simulation == null)
        {
            return;
        }

        if (_item == null)
        {
            return;
        }

        if (_simulation.HasGraph(_item))
        {
            _simulation.RemoveGraph(_item);
        }
    }

    private void OnAddWatchEnabled(EditButton button)
    {
        if (_simulation == null)
        {
            return;
        }

        if (_item == null)
        {
            return;
        }

        if (!_simulation.HasGraph(_item))
        {
            _simulation.AddGraph(_item);
        }
    }

    private void OnRewindClicked()
    {
        _simulation?.Rewind();
        _startStopButton?.Deselect();
    }

    private void OnRewindEnabled(EditButton b)
    {
    }

    private void OnShowGraphClicked()
    {
    }

    private void OnShowGraphDisabled(EditButton obj)
    {
        OnGraphVisibilityChanged?.Invoke(false);
    }

    private void OnShowGraphEnabled(EditButton obj)
    {
        OnGraphVisibilityChanged?.Invoke(true);
    }

    private void OnShowLogClicked()
    {
    }

    private void OnShowLogDisabled(EditButton obj)
    {
        OnLogVisibilityChanged?.Invoke(false);
    }

    private void OnShowLogEnabled(EditButton obj)
    {
        OnLogVisibilityChanged?.Invoke(true);
    }

    private void OnStartStopClicked()
    {
    }

    private void OnStartStopDisabled(EditButton obj)
    {
        _simulation?.Stop();
    }

    private void OnStartStopEnabled(EditButton obj)
    {
        _simulation?.Start();
    }
}
