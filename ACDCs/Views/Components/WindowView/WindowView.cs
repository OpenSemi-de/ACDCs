using System.ComponentModel;
using ACDCs.Views.Components.Menu;
using Microsoft.Maui.Layouts;

namespace ACDCs.Views.Components.WindowView;

using Sharp.UI;

/*
<ui:StackLayout Orientation = "Vertical" HorizontalOptions="Fill" VerticalOptions="Fill">
    <ui:StackLayout Orientation = "Horizontal" HeightRequest="32" HorizontalOptions="Fill">
    <ui:Button Text = "=" HorizontalOptions="Start" WidthRequest="32" HeightRequest="32" x:Name="MenuButton"></ui:Button>
    <ui:Label Text = "Title" HeightRequest="32" HorizontalOptions="Fill">
    <ui:Label.GestureRecognizers>
    <PanGestureRecognizer PanUpdated = "PanGestureRecognizer_OnPanUpdated" ></ PanGestureRecognizer >
    </ ui:Label.GestureRecognizers>
    </ui:Label>
    </ui:StackLayout>
    <ContentView x:Name="MainContent" HorizontalOptions="Fill" VerticalOptions="Fill"></ContentView>
    </ui:StackLayout>
    </ui:Border>
    </
    */

public class WindowFrame : Frame
{
}

[SharpObject]
public partial class WindowView : ContentView, IWindowViewProperties
{
    private readonly MenuFrame _menuFrame;
    private readonly ContentView _windowContentView;
    private Rect _lastBounds = Rect.Zero;
    public SharpAbsoluteLayout MainContainer { get; set; }

    public Action OnClose { get; set; }

    public WindowView(SharpAbsoluteLayout sharpAbsoluteLayout)
    {
        AbsoluteLayout.SetLayoutBounds(this, new Rect(30, 30, 500, 400));
        _lastBounds = AbsoluteLayout.GetLayoutBounds(this);
        MainContainer = sharpAbsoluteLayout;

        var grid = new Grid()
            .VerticalOptions(LayoutOptions.Fill)
            .HorizontalOptions(LayoutOptions.Fill);

        grid.ColumnDefinitions.Add(new Microsoft.Maui.Controls.ColumnDefinition(32));
        grid.ColumnDefinitions.Add(new Microsoft.Maui.Controls.ColumnDefinition(GridLength.Star));
        grid.ColumnDefinitions.Add(new Microsoft.Maui.Controls.ColumnDefinition(32));
        grid.RowDefinitions.Add(new Microsoft.Maui.Controls.RowDefinition(32));
        grid.RowDefinitions.Add(new Microsoft.Maui.Controls.RowDefinition(GridLength.Star));
        grid.RowDefinitions.Add(new Microsoft.Maui.Controls.RowDefinition(32));

        var menuButton = new MenuButton("*", "")
            .HeightRequest(32)
            .WidthRequest(32);

        grid.Add(menuButton);
        SetRowAndColumn(menuButton, 0, 0);

        var titleLabel = new Label("Title")
            .HorizontalOptions(LayoutOptions.Fill)
            .HeightRequest(32);

        grid.Add(titleLabel);
        SetRowAndColumn(titleLabel, 0, 1, 2);

        var panGesture = new PanGestureRecognizer()
            .OnPanUpdated(PanGestureRecognizer_OnPanUpdated);

        titleLabel.GestureRecognizers.Add(panGesture);
        _windowContentView = new ContentView()
            .HorizontalOptions(LayoutOptions.Fill)
            .VerticalOptions(LayoutOptions.Fill);

        grid.Add(_windowContentView);
        SetRowAndColumn(_windowContentView, 1, 0, 3, 2);

        var resizeField = new Label("//")
            .WidthRequest(32)
            .HeightRequest(32)
            .FontSize(30)
            .FontAttributes(FontAttributes.Bold | FontAttributes.Italic)
            .HorizontalTextAlignment(TextAlignment.End)
            .VerticalTextAlignment(TextAlignment.End);

        grid.Add(resizeField);
        SetRowAndColumn(resizeField, 2, 2);

        var resizeRecognizer = new PanGestureRecognizer()
            .OnPanUpdated(resizeRecognizer_PanUpdated);

        resizeField.GestureRecognizers.Add(resizeRecognizer);

        _menuFrame = new MenuFrame();
        List<MenuItemDefinition> menuItems = new()
        {
            new MenuItemDefinition()
            {
                Text = "Maximize",
                MenuCommand = "dummy",
                ClickAction = Maximize
            },
            new MenuItemDefinition()
            {
                Text = "Minimize",
                MenuCommand = "dummy",
                ClickAction = Minimize
            },
            new MenuItemDefinition()
            {
                Text = "Restore",
                MenuCommand = "dummy",
                ClickAction = RestoreWindow_Clicked
            },
            new MenuItemDefinition()
            {
                Text = "Close",
                MenuCommand = "dummy",
                ClickAction = CloseWindow_Clicked
            },
        };

        _menuFrame.MainContainer = MainContainer;
        _menuFrame.WindowFrame = this;
        _menuFrame.LoadMenu(menuItems);
        MainContainer.Add(_menuFrame);
        menuButton.MenuFrame = _menuFrame;

        var border = new WindowFrame()
            .Margin(0)
            .Padding(0)
            .HorizontalOptions(LayoutOptions.Fill)
            .VerticalOptions(LayoutOptions.Fill);

        border.Content = grid;
        PropertyChanged += OnPropertyChanged;
        Content = border;
    }

    public void Maximize()
    {
        AbsoluteLayout.SetLayoutFlags(this, AbsoluteLayoutFlags.SizeProportional);
        AbsoluteLayout.SetLayoutBounds(this, new Rect(0, 0, 1, 1));
    }

    private void CloseWindow_Clicked()
    {
        OnClose?.Invoke();
        IsVisible = false;
        PropertyChanged -= OnPropertyChanged;
        MainContainer.Remove(this);
        MainContainer.Remove(_menuFrame);

        GC.Collect(GC.MaxGeneration, GCCollectionMode.Aggressive, true, true);
        GC.WaitForFullGCComplete(100000);
        GC.WaitForPendingFinalizers();
    }

    private void Minimize()
    {
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
        App.Call(() =>
        {
            if (_lastBounds == Rect.Zero)
                _lastBounds = AbsoluteLayout.GetLayoutBounds(this);

            if (e.StatusType == GestureStatus.Running)
            {
                Rect newBounds = new(_lastBounds.Location, _lastBounds.Size);
                newBounds.Top += e.TotalY;
                newBounds.Left += e.TotalX;

                AbsoluteLayout.SetLayoutBounds(this, newBounds);
            }
            else
            {
                _lastBounds = AbsoluteLayout.GetLayoutBounds(this);
            }

            return Task.CompletedTask;
        }).Wait();
    }

    private void resizeRecognizer_PanUpdated(object? sender, PanUpdatedEventArgs e)
    {
        App.Call(() =>
        {
            if (_lastBounds == Rect.Zero)
                _lastBounds = AbsoluteLayout.GetLayoutBounds(this);

            if (e.StatusType == GestureStatus.Running)
            {
                Rect newBounds = new(_lastBounds.Location, _lastBounds.Size);

                newBounds.Height += e.TotalY;
                newBounds.Width += e.TotalX;

                if (newBounds.Width < 400)
                    newBounds.Width = 400;

                if (newBounds.Height < 400)
                    newBounds.Height = 400;

                AbsoluteLayout.SetLayoutFlags(this, AbsoluteLayoutFlags.None);
                AbsoluteLayout.SetLayoutBounds(this, newBounds);
            }
            else
            {
                _lastBounds = AbsoluteLayout.GetLayoutBounds(this);
            }

            return Task.CompletedTask;
        }).Wait();
    }

    private void RestoreWindow_Clicked()
    {
        AbsoluteLayout.SetLayoutFlags(this, AbsoluteLayoutFlags.None);
        AbsoluteLayout.SetLayoutBounds(this, _lastBounds);
    }

    private void SetRowAndColumn(IView view, int row, int column, int columnSpan = 0, int rowSpan = 0)
    {
        Microsoft.Maui.Controls.Grid.SetRow((BindableObject)view, row);
        Microsoft.Maui.Controls.Grid.SetColumn((BindableObject)view, column);
        if (columnSpan > 0)
            Microsoft.Maui.Controls.Grid.SetColumnSpan((BindableObject)view, columnSpan);
        if (rowSpan > 0)
            Microsoft.Maui.Controls.Grid.SetRowSpan((BindableObject)view, rowSpan);
    }
}

[BindableProperties]
public interface IWindowViewProperties
{
    public View WindowContent { get; set; }
}
