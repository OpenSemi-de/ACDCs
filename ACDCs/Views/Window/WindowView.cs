using System.ComponentModel;
using ACDCs.Components;
using ACDCs.Components.Menu;
using ACDCs.Components.Window;
using ACDCs.Interfaces;
using ACDCs.Services;
using Sharp.UI;
using AbsoluteLayout = Sharp.UI.AbsoluteLayout;
using ColumnDefinition = Microsoft.Maui.Controls.ColumnDefinition;
using ContentView = Sharp.UI.ContentView;
using Grid = Sharp.UI.Grid;
using Image = Sharp.UI.Image;
using IView = Sharp.UI.IView;
using Label = Sharp.UI.Label;
using PanGestureRecognizer = Sharp.UI.PanGestureRecognizer;
using RowDefinition = Microsoft.Maui.Controls.RowDefinition;

namespace ACDCs.Views.Window;

[SharpObject]
public partial class WindowView : ContentView, IWindowViewProperties
{
    private readonly WindowFrame _border;
    private readonly Grid _grid;
    private readonly Image _headerImage;
    private readonly MenuButton _menuButton;
    private readonly MenuFrame _menuFrame;
    private readonly Label _resizeField;
    private readonly TitleLabel _titleLabel;
    private readonly ContentView _windowContentView;
    private bool _isActive;
    private Rect _lastBounds = Rect.Zero;
    public SharpAbsoluteLayout MainContainer { get; set; }
    public Func<bool>? OnClose { get; set; }
    public WindowState State { get; set; } = WindowState.Standard;
    public WindowTabBar? TabBar { get; set; }
    public string WindowTitle { get; set; }

    public WindowView(SharpAbsoluteLayout sharpAbsoluteLayout, string title)
    {
        WindowTitle = title;
        AbsoluteLayout.SetLayoutBounds(this, new Rect(30, 30, 500, 400));
        MainContainer = sharpAbsoluteLayout;

        _grid = new Grid()
            .VerticalOptions(LayoutOptions.Fill)
            .HorizontalOptions(LayoutOptions.Fill)
            .Margin(0)
            .Padding(0)
            .ColumnSpacing(0)
            .RowSpacing(0)
            .BackgroundColor(ColorService.Background);

        _grid.ColumnDefinitions.Add(new ColumnDefinition(36));
        _grid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
        _grid.ColumnDefinitions.Add(new ColumnDefinition(36));
        _grid.RowDefinitions.Add(new RowDefinition(36));
        _grid.RowDefinitions.Add(new RowDefinition(GridLength.Star));
        _grid.RowDefinitions.Add(new RowDefinition(36));

        _headerImage = new Image()
            .Margin(0)
            .Aspect(Aspect.Fill)
            .HorizontalOptions(LayoutOptions.Fill)
            .VerticalOptions(LayoutOptions.Fill);

        SetRowAndColumn(_headerImage, 0, 0, 3, 3);
        _grid.Add(_headerImage);

        _menuButton = new MenuButton("*", "")
            .HeightRequest(32)
            .WidthRequest(32)
            .FontSize(30)
            .Margin(new Thickness(2, 4, 0, 0));

        SetRowAndColumn(_menuButton, 0, 0);

        _titleLabel = new TitleLabel(WindowTitle)
            .HorizontalOptions(LayoutOptions.Fill)
            .HeightRequest(34)
            .TextColor(ColorService.Text);

        SetRowAndColumn(_titleLabel, 0, 0, 3);

        var panGesture = new PanGestureRecognizer()
            .OnPanUpdated(PanGestureRecognizer_OnPanUpdated);

        _titleLabel.GestureRecognizers.Add(panGesture);
        _windowContentView = new ContentView()
            .Padding(4)
            .HorizontalOptions(LayoutOptions.Fill)
            .VerticalOptions(LayoutOptions.Fill);

        _grid.Add(_windowContentView);
        SetRowAndColumn(_windowContentView, 1, 0, 3, 2);

        _resizeField = new Label("//")
            .WidthRequest(32)
            .HeightRequest(32)
            .FontSize(30)
            .TextColor(ColorService.Text)
            .FontAttributes(FontAttributes.Bold | FontAttributes.Italic)
            .HorizontalTextAlignment(TextAlignment.End)
            .VerticalTextAlignment(TextAlignment.End);

        _grid.Add(_resizeField);
        SetRowAndColumn(_resizeField, 2, 2);

        var resizeRecognizer = new PanGestureRecognizer()
            .OnPanUpdated(resizeRecognizer_PanUpdated);

        _resizeField.GestureRecognizers.Add(resizeRecognizer);

        _menuFrame = new MenuFrame();
        List<MenuItemDefinition> menuItems = new()
        {
            new MenuItemDefinition
            {
                Text = "Maximize",
                MenuCommand = "dummy",
                ClickAction = Maximize
            },
            new MenuItemDefinition
            {
                Text = "Minimize",
                MenuCommand = "dummy2",
                ClickAction = Minimize
            },
            new MenuItemDefinition
            {
                Text = "Restore",
                MenuCommand = "dummy3",
                ClickAction = Restore
            },
            new MenuItemDefinition
            {
                Text = "Close",
                MenuCommand = "dummy3",
                ClickAction = Close
            }
        };

        _menuFrame.MainContainer = MainContainer;
        _menuFrame.WindowFrame = this;
        _menuFrame.LoadMenu(menuItems);
        _menuButton.MenuFrame = _menuFrame;

        _border = new WindowFrame()
            .Margin(0)
            .Padding(0)
            .BorderColor(ColorService.Border)
            .BackgroundColor(ColorService.Text)
            .HorizontalOptions(LayoutOptions.Fill)
            .VerticalOptions(LayoutOptions.Fill);

        _grid.Add(_titleLabel);
        _grid.Add(_menuButton);

        _border.Content = _grid;
        PropertyChanged += OnPropertyChanged;
        Content = _border;

        sharpAbsoluteLayout.Add(this);

        MainContainer.Add(_menuFrame);
        MenuFrame.HideAllMenus();
        _lastBounds = AbsoluteLayout.GetLayoutBounds(this);

        _headerImage.Source = ImageService.WindowImageSource(Convert.ToSingle(_lastBounds.Width), Convert.ToSingle(_lastBounds.Height));
    }

    public void Close()
    {
        if (OnClose != null && OnClose()) return;
        TabBar?.RemoveWindow(this);

        IsVisible = false;
        PropertyChanged -= OnPropertyChanged;
        MainContainer.Remove(this);
        MainContainer.Remove(_menuFrame);

        GC.Collect(GC.MaxGeneration, GCCollectionMode.Aggressive, true, true);
        GC.WaitForFullGCComplete(100000);
        GC.WaitForPendingFinalizers();
    }

    public void Maximize()
    {
        State = WindowState.Maximized;

        this.TranslateTo(0, 0);
        AbsoluteLayout.SetLayoutBounds(this, new Rect(0, 0, MainContainer.Width, MainContainer.Height));
        _lastBounds = AbsoluteLayout.GetLayoutBounds(this);
        _headerImage.Source = ImageService.WindowImageSource(Convert.ToSingle(_lastBounds.Width), Convert.ToSingle(_lastBounds.Height));
    }

    public async void Minimize()
    {
        if (State == WindowState.Minimized) return;
        State = WindowState.Minimized;

        if (TabBar != null)
        {
            await this.TranslateTo(-_lastBounds.Width - 100, MainContainer.Height + 100);
            //   await this.LayoutTo(new Rect(0, 0, 120, 30));

            //     AbsoluteLayout.SetLayoutBounds(this, bounds)t;
            return;
        }

        AbsoluteLayout.SetLayoutBounds(this, new Rect(MainContainer.Height + 100, 1, 120, 32));
    }

    public void Restore()
    {
        Maximize();
    }

    public void SetActive()
    {
        TabBar?.BringToFront(this);
        _isActive = true;
        _titleLabel.TextColor(ColorService.Text);
        _headerImage.BackgroundColor(ColorService.Foreground);
        _border.BorderColor(ColorService.Border);
        _grid.BackgroundColor(ColorService.Foreground);
    }

    public void SetInactive()
    {
        _isActive = false;
        _titleLabel.TextColor(ColorService.Foreground);
        _headerImage.BackgroundColor(ColorService.Background);
        _border.BorderColor(ColorService.Background);
        _grid.BackgroundColor(ColorService.BackgroundHigh);
    }

    protected void HideMenuButton()
    {
        _menuButton.IsVisible = false;
    }

    protected void HideResizer()
    {
        _resizeField.IsVisible = false;
    }

    private static void SetRowAndColumn(IView view, int row, int column, int columnSpan = 0, int rowSpan = 0)
    {
        Microsoft.Maui.Controls.Grid.SetRow((BindableObject)view, row);
        Microsoft.Maui.Controls.Grid.SetColumn((BindableObject)view, column);
        if (columnSpan > 0)
            Microsoft.Maui.Controls.Grid.SetColumnSpan((BindableObject)view, columnSpan);
        if (rowSpan > 0)
            Microsoft.Maui.Controls.Grid.SetRowSpan((BindableObject)view, rowSpan);
    }

    private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName != null && e.PropertyName.StartsWith("WindowContent"))
        {
            _windowContentView.Content = WindowContent;
        }
    }

    private void PanGestureRecognizer_OnPanUpdated(object? sender, PanUpdatedEventArgs e)
    {
        API.Call(() =>
        {
            if (!_isActive) SetActive();

            if (_lastBounds == Rect.Zero)
                _lastBounds = AbsoluteLayout.GetLayoutBounds(this);

            if (e.StatusType != GestureStatus.Started && e.StatusType != GestureStatus.Completed)
            {
                Rect newBounds = new(_lastBounds.Location, _lastBounds.Size);
                newBounds.Top += e.TotalY;
                newBounds.Left += e.TotalX;

                AbsoluteLayout.SetLayoutBounds(this, newBounds);
            }
            else
            {
                _lastBounds = AbsoluteLayout.GetLayoutBounds(this);
                _headerImage.Source = ImageService.WindowImageSource(Convert.ToSingle(_lastBounds.Width), Convert.ToSingle(_lastBounds.Height));
            }

            return Task.CompletedTask;
        }).Wait();
    }

    private void resizeRecognizer_PanUpdated(object? sender, PanUpdatedEventArgs e)
    {
        API.Call(() =>
        {
            if (!_isActive) SetActive();
            if (_lastBounds == Rect.Zero)
                _lastBounds = AbsoluteLayout.GetLayoutBounds(this);

            if (e.StatusType != GestureStatus.Started && e.StatusType != GestureStatus.Completed)
            {
                Rect newBounds = new(_lastBounds.Location, _lastBounds.Size);

                newBounds.Height += e.TotalY;
                newBounds.Width += e.TotalX;

                if (newBounds.Width < 200)
                    newBounds.Width = 200;

                if (newBounds.Height < 200)
                    newBounds.Height = 200;

                BatchBegin();
                AbsoluteLayout.SetLayoutBounds(this, newBounds);
                BatchCommit();
            }
            else
            {
                if (e.StatusType == GestureStatus.Started)
                {
                    _resizeField.WidthRequest(100)
                        .HeightRequest(100)
                        .Margin(new Thickness(-68, -68, 0, 0));
                }
                else
                {
                    _resizeField.WidthRequest(32)
                        .HeightRequest(32)
                        .Margin(new Thickness(0, 0, 0, 0));
                }

                _lastBounds = AbsoluteLayout.GetLayoutBounds(this);
                _headerImage.Source = ImageService.WindowImageSource(Convert.ToSingle(_lastBounds.Width), Convert.ToSingle(_lastBounds.Height));
            }

            return Task.CompletedTask;
        }).Wait();
    }
}

public class TitleLabel : ContentView
{
    private readonly Label _label;

    public TitleLabel(string windowTitle)
    {
        _label = new Label(windowTitle)
            .HorizontalOptions(LayoutOptions.Fill)
            .VerticalOptions(LayoutOptions.Fill)
            .VerticalTextAlignment(TextAlignment.Center)
            .HorizontalTextAlignment(TextAlignment.Center)
            .FontSize(14)
            .FontAttributes(FontAttributes.Bold)
            .Padding(0)
            .Margin(0);

        this.Padding(0).Margin(1)
            .HorizontalOptions(LayoutOptions.Fill)
            .VerticalOptions(LayoutOptions.Fill);

        Content = _label;
    }

    public TitleLabel TextColor(Color color)
    {
        _label.TextColor(color);
        return this;
    }
}
