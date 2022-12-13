using System.Threading.Tasks;
using ACDCs.Views.Components.CircuitView;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Layouts;

namespace ACDCs.Views.Components.Edit;

public class EditContainer : StackLayout
{
    public EditContainer()
    {
        AbsoluteLayout.SetLayoutFlags(this, AbsoluteLayoutFlags.None);
        AbsoluteLayout.SetLayoutBounds(this, new Rect(0, 300, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
        _selectAreaButton = new(SelectArea, OnSelectButtonChange);
        _rotateButton = new(Rotate, OnSelectButtonChange);
        _mirrorButton = new(Mirror, OnSelectButtonChange);
        _deleteButton = new(Delete, OnSelectButtonChange);
        Add(_selectAreaButton);
        Add(_rotateButton);
        Add(_mirrorButton);
        Add(_deleteButton);

        
    }

    private readonly EditButton _selectAreaButton;

    private void OnSelectButtonChange(EditButton editButton)
    {
        _lastButton?.Deselect();
        _lastButton = editButton;
    }

    public void SelectArea()
    {
        CircuitView?.SelectArea();
        
    }

    private void OnSelectionChanged(object sender, SelectionChangeEventArgs args)
    {
        _lastButton?.Deselect();
        _selectAreaButton.Deselect();

    }

    public void Delete()
    {
        CircuitView?.DeleteSelected();
        _lastButton?.Deselect();
    }

    public void Mirror()
    {
        CircuitView?.MirrorSelected();
        _lastButton?.Deselect();
    }

    public void Rotate()
    {
        CircuitView?.RotateSelected();
        _lastButton?.Deselect();
    }

    private readonly EditButton _deleteButton;
    private readonly EditButton _mirrorButton;
    private readonly EditButton _rotateButton;
    private EditButton? _lastButton;

    public CircuitViewContainer? CircuitView
    {
        get => (CircuitViewContainer)GetValue(s_circuitViewProperty);
        set
        {
            SetValue(s_circuitViewProperty, value);

            if (CircuitView != null)
            {
                CircuitView.SelectionChanged += OnSelectionChanged;
            }
        }
    }


    private static readonly BindableProperty s_circuitViewProperty =
        BindableProperty.Create(nameof(CircuitView), typeof(CircuitViewContainer), typeof(CircuitSheetPage));

}
