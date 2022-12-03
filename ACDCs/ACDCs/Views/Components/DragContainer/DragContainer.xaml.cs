using System.Threading.Tasks;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Layouts;

namespace ACDCs.Views.Components.DragContainer;

public partial class DragContainer : ContentView
{
    public static readonly BindableProperty TitleProperty =
        BindableProperty.Create(nameof(Title), typeof(string), typeof(DragContainer), "Title",
            propertyChanged: propertyChanged);

    public static readonly BindableProperty LayoutProperty =
        BindableProperty.Create(nameof(Layout), typeof(IView), typeof(DragContainer), null,
            propertyChanged: propertyChanged);

    private static void propertyChanged(BindableObject bindable, object oldvalue, object newvalue)
    {
        if (bindable is DragContainer container)
        {
            if (newvalue is StackOrientation orientation)
            {
                if (container.Orientation == StackOrientation.Horizontal)
                {
                    container.TitleLabel.Rotation = 270;
                    container.TitleLabel.HorizontalOptions = LayoutOptions.Start;
                    container.TitleLabel.VerticalOptions = LayoutOptions.Fill;
                }
                else
                {
                    container.TitleLabel.Rotation = 0;
                    container.TitleLabel.HorizontalOptions = LayoutOptions.Fill;
                    container.TitleLabel.VerticalOptions = LayoutOptions.Start;
                }
            }

            if (newvalue is IView view)
            {
                container.MainContentView.Content = (View)view;
            }
        }
    }

    public static readonly BindableProperty OrientationProperty =
        BindableProperty.Create(nameof(Orientation), typeof(StackOrientation), typeof(DragContainer),
            StackOrientation.Vertical, propertyChanged: propertyChanged);

    private Rect _lastBounds = Rect.Zero;
    private PanGestureRecognizer _dragRecognizer;


    public StackOrientation Orientation
    {
        get => (StackOrientation)GetValue(OrientationProperty);
        set => SetValue(OrientationProperty, value);
    }

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public new IView Layout
    {
        get => (IView)GetValue(LayoutProperty);
        set => SetValue(LayoutProperty, value);
    }

    public DragContainer()
    {
        InitializeComponent();
        this.Loaded += (sender, args) => StartDragging();
    }

    private void StartDragging()
    {
        _dragRecognizer = new();

        _dragRecognizer.PanUpdated += PanGestureRecognizer_OnPanUpdated;
        this.GestureRecognizers.Add(_dragRecognizer);
        TitleFrame.GestureRecognizers.Add(_dragRecognizer);
    }

    private void PanGestureRecognizer_OnPanUpdated(object? sender, PanUpdatedEventArgs e)
    {
        App.Call(() =>
        {
            if (e.StatusType == GestureStatus.Running || e.StatusType == GestureStatus.Canceled)
            {
                Rect newBounds = new(_lastBounds.Location, _lastBounds.Size);
                newBounds.Top += e.TotalY; // - TitleLabel.Height / 2;
                newBounds.Left += e.TotalX; // - TitleLabel.Width / 2;

                if (newBounds.Top > 5)
                {
                    newBounds.Width = AbsoluteLayout.AutoSize;
                    //  newBounds.Height = AbsoluteLayout.AutoSize;
                }
                else
                {
                    newBounds.Width = 1;
                    newBounds.Top = 0;
                }

                if (newBounds.Left > 5)
                {
                    newBounds.Width = AbsoluteLayout.AutoSize;
                    //newBounds.Height = AbsoluteLayout.AutoSize;
                }
                else
                {
                    newBounds.Width = 1;
                    newBounds.Left = 0;
                }

                AbsoluteLayout.SetLayoutBounds(this, newBounds);

                if (newBounds.Width == AbsoluteLayout.AutoSize)
                {
                    AbsoluteLayout.SetLayoutFlags(this, AbsoluteLayoutFlags.None);
                }
                else
                {
                    AbsoluteLayout.SetLayoutFlags(this, AbsoluteLayoutFlags.WidthProportional);
                }
            }
            else
            {
                _lastBounds = AbsoluteLayout.GetLayoutBounds(this);
            }

            return Task.CompletedTask;
        }).Wait();
    }
}