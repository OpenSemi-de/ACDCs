using ACDCs.Services;
using ACDCs.Views.Components.CircuitView;
using Microsoft.Maui.Layouts;

namespace ACDCs.Views.Components.DragContainer;

using Sharp.UI;

[BindableProperties]
public interface DragContainerProperties
{
    CircuitViewContainer CircuitView { get; set; }
    IView Layout { get; set; }
    StackOrientation Orientation { get; set; }
    string Title { get; set; }
}

[SharpObject]
public partial class DragContainer : ContentView, DragContainerProperties
{
    private PanGestureRecognizer? _dragRecognizer;

    private Rect _lastBounds = Rect.Zero;

    public DragContainer()
    {
        InitializeComponent();
        Loaded += (sender, args) => StartDragging();

        LayoutChanged += DragContainer_LayoutChanged;
    }

    public void ButtonHide_OnClicked(object? sender, EventArgs e)
    {
        this.Orientation = StackOrientation.Horizontal;
        propertyChanged(this, Orientation, Orientation);
        Microsoft.Maui.Controls.AbsoluteLayout.SetLayoutFlags(this, AbsoluteLayoutFlags.XProportional);
        Microsoft.Maui.Controls.AbsoluteLayout.SetLayoutBounds(this, new(1, 200, 40, 300));
    }

    public void ShowButtonHide()
    {
        ButtonHide.IsVisible = true;
    }

    private static void propertyChanged(BindableObject bindable, object oldvalue, object newvalue)
    {
        if (bindable is DragContainer container)
        {
            if (newvalue is StackOrientation orientation)
            {
                if (container.Orientation == StackOrientation.Horizontal)
                {
                    container.TitleLabel.Rotation = 270;
                    container.TitleLabel.WidthRequest = 40;
                    container.TitleLabel.HeightRequest = 40;
                    container.TitleLabel.HorizontalOptions = LayoutOptions.Fill;
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

    private void DragContainer_LayoutChanged(object? sender, EventArgs e)
    {
        if (sender is DragContainer thisContainer)
        {
            float height = (float)thisContainer.Height;
            float width = (float)thisContainer.Width;
            BackgroundImage.Source = ImageService.BackgroundImageSource(width, height);
        }
    }

    private void PanGestureRecognizer_OnPanUpdated(object? sender, PanUpdatedEventArgs e)
    {
        App.Call(() =>
        {
            if (e.StatusType == GestureStatus.Running || e.StatusType == GestureStatus.Canceled)
            {
                if (ButtonHide.IsVisible)
                {
                    this.Orientation = StackOrientation.Vertical;
                    propertyChanged(this, Orientation, Orientation);
                }
                Rect newBounds = new(_lastBounds.Location, _lastBounds.Size);
                newBounds.Top += e.TotalY;
                newBounds.Left += e.TotalX;

                if (newBounds.Top > 5)
                {
                    newBounds.Width = Microsoft.Maui.Controls.AbsoluteLayout.AutoSize;
                }
                else
                {
                    newBounds.Width = 1;
                    newBounds.Top = 0;
                }

                if (newBounds.Left > 5)
                {
                    newBounds.Width = Microsoft.Maui.Controls.AbsoluteLayout.AutoSize;
                }
                else
                {
                    newBounds.Width = 1;
                    newBounds.Left = 0;
                }

                Microsoft.Maui.Controls.AbsoluteLayout.SetLayoutBounds(this, newBounds);

                if (newBounds.Width == Microsoft.Maui.Controls.AbsoluteLayout.AutoSize)
                {
                    Microsoft.Maui.Controls.AbsoluteLayout.SetLayoutFlags(this, AbsoluteLayoutFlags.None);
                }
                else
                {
                    Microsoft.Maui.Controls.AbsoluteLayout.SetLayoutFlags(this, AbsoluteLayoutFlags.WidthProportional);
                }
            }
            else
            {
                _lastBounds = Microsoft.Maui.Controls.AbsoluteLayout.GetLayoutBounds(this);
            }

            return Task.CompletedTask;
        }).Wait();
    }

    private void StartDragging()
    {
        _dragRecognizer = new();

        _dragRecognizer.PanUpdated += PanGestureRecognizer_OnPanUpdated;
        GestureRecognizers.Add(_dragRecognizer);
        TitleFrame.GestureRecognizers.Add(_dragRecognizer);
        if (ButtonHide.IsVisible)
            ButtonHide_OnClicked(this, EventArgs.Empty);
    }
}
