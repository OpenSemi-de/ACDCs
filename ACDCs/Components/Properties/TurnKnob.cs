using ACDCs.Services;
using Sharp.UI;
using GraphicsView = Sharp.UI.GraphicsView;
using PanGestureRecognizer = Sharp.UI.PanGestureRecognizer;
using TapGestureRecognizer = Sharp.UI.TapGestureRecognizer;

namespace ACDCs.Components.Properties;

using Debug = System.Diagnostics.Debug;

public class TurnKnob : GraphicsView, IDrawable
{
    private int _knobMaxValue;
    private Action<object>? _knobOnValueChanged;
    private int _knobValue;
    private int _knobValueMultiplier;
    private float _rotation = 0;
    private float _rotationDelta;
    private object? _value;

    public TurnKnob()
    {
        this.HorizontalOptions(LayoutOptions.Fill)
            .VerticalOptions(LayoutOptions.Fill);

        PanGestureRecognizer panGestureRecognizer = new();
        panGestureRecognizer.OnPanUpdated(PanUpdated);
        //  GestureRecognizers.Add(panGestureRecognizer);

        TapGestureRecognizer tapGestureRecognizer = new();
        tapGestureRecognizer.OnTapped(OnTapped);
        GestureRecognizers.Add(tapGestureRecognizer.MauiObject);

        Drawable = this;
    }

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        canvas.StrokeColor = ColorService.BackgroundHigh;
        canvas.FillColor = ColorService.Foreground;
        canvas.FontColor = ColorService.Text;
        canvas.StrokeSize = 1;

        canvas.FillEllipse(2, 2, Convert.ToSingle(Width) - 2, Convert.ToSingle(Height) - 2);
        canvas.DrawEllipse(0, 0, Convert.ToSingle(Width) - 2, Convert.ToSingle(Height) - 2);
        canvas.SaveState();
        canvas.StrokeColor = ColorService.Border;
        canvas.StrokeSize = 0.5f;
        canvas.DrawEllipse(3, 3, Convert.ToSingle(Width) - 3, Convert.ToSingle(Height) - 3);
        canvas.RestoreState();
        canvas.DrawString(S(_knobValue) + "*" + _knobValueMultiplier, new RectF(0, 0, F(Width), F(Height)), HorizontalAlignment.Center,
            VerticalAlignment.Center);
        canvas.SaveState();
        canvas.Rotate(_rotation, Convert.ToSingle(Width / 2), Convert.ToSingle(Height / 2));
        canvas.FillColor = ColorService.Border;
        canvas.FillRectangle(Convert.ToSingle(Width / 2) - 2, 0, 4, 10);
        canvas.DrawRectangle(Convert.ToSingle(Width / 2) - 2, 0, 4, 10);
        canvas.RestoreState();
    }

    public TurnKnob InputValue(object value)
    {
        _value = value;
        if (value is int intValue)
        {
            _knobValue = intValue;
            _knobMaxValue = 100;
            _rotation = 360 * (Convert.ToSingle(_knobValue) / _knobMaxValue);
            Invalidate();
        }
        return this;
    }

    public TurnKnob OnKnobValueChanged(Action<object> knobOnValueChanged)
    {
        _knobOnValueChanged = knobOnValueChanged;
        return this;
    }

    private void Drag_DragStart(object? sender, DragStartingEventArgs e)
    {
        API.PointerLayoutObjectToMeasure = this;
        API.PointerCallback = PointerCallback;
    }

    private float F(double d)
    {
        return Convert.ToSingle(d);
    }

    private void OnTapped(object? sender, TappedEventArgs e)
    {
        if (API.PointerLayoutObjectToMeasure != this)
        {
            API.PointerLayoutObjectToMeasure = this;
            API.PointerCallback = PointerCallback;
        }
        else
        {
            API.PointerCallback = null;
            API.PointerLayoutObjectToMeasure = null;
        }
    }

    private void PanUpdated(object? sender, PanUpdatedEventArgs e)
    {
        if (e.StatusType == GestureStatus.Started)
        {
            API.PointerLayoutObjectToMeasure = this;
            API.PointerCallback = PointerCallback;
        }

        if (e.StatusType == GestureStatus.Canceled || e.StatusType == GestureStatus.Completed)
        {
            API.PointerLayoutObjectToMeasure = null;
            API.PointerCallback = null;
        }
        //
        // if (e.StatusType == GestureStatus.Running)
        // {
        //     _rotation += Convert.ToSingle(e.TotalX + e.TotalY);
        // }

        Invalidate();
    }

    private void PointerCallback(Point obj)
    {
        float newRotation = 90 + Convert.ToSingle(Math.Atan2(obj.Y, obj.X) * (180.0 / Math.PI));
        Debug.WriteLine(newRotation);

        _rotationDelta = newRotation - _rotation;

        _rotation += _rotationDelta;

        if (_rotation < 0)
        {
            _rotation += 360;
            _knobValueMultiplier--;
        }
        else if (_rotation >= 360)
        {
            _rotation -= 360;
            _knobValueMultiplier++;
        }

        _knobValue = Convert.ToInt32(Math.Floor(Convert.ToSingle(_knobMaxValue) / 360 * _rotation));
        _knobOnValueChanged?.Invoke(_knobValue);
        Invalidate();
    }

    private string S(int value)
    {
        return Convert.ToString(value);
    }
}
