namespace ACDCs.API.Windowing.Components.Window;

using Interfaces;
using Sharp.UI;

public class WindowContainer : AbsoluteLayout, IWindowContainer
{
    private readonly PanGestureRecognizer _windowPanRecognizer;
    private readonly List<Window> _windows;
    private double? _lastHeight;
    private double? _lastWidth;
    private double? _lastX;
    private double? _lastY;
    private Window? _pickWindow;
    private WindowOperation _windowOperation = WindowOperation.None;
    private PanGestureRecognizer _windowSizePanRecognizer;
    public WindowTabBar? TabBar { get; set; }

    public WindowContainer()
    {
        this.HorizontalOptions(LayoutOptions.Fill)
            .VerticalOptions(LayoutOptions.Fill);

        _windows = new List<Window>();
        _windowPanRecognizer = new PanGestureRecognizer();
        _windowPanRecognizer.PanUpdated += WindowPanRecognizer_PanUpdated;
        GestureRecognizers.Add(_windowPanRecognizer);

        _windowSizePanRecognizer = new PanGestureRecognizer();
        _windowSizePanRecognizer.PanUpdated += WindowSizePanRecognizer_PanUpdated;
        GestureRecognizers.Add(_windowSizePanRecognizer);
    }

    public void AddWindow(Window window)
    {
        _windows.Add(window);
        if (window.Title != null && !window.Title.GestureRecognizers.Contains(_windowPanRecognizer))
            window.Title.GestureRecognizers.Add(_windowPanRecognizer);
        if (window.Resizer != null && !window.Resizer.GestureRecognizers.Contains(_windowSizePanRecognizer))
            window.Resizer.GestureRecognizers.Add(_windowSizePanRecognizer);
        Add(window);
        TabBar?.AddWindow(window);
    }

    public void CloseWindow(Window window)
    {
        _windows.Remove(window);
        window.Title?.GestureRecognizers.Remove(_windowPanRecognizer);
        Remove(window);
        TabBar?.RemoveWindow(window);
    }

    public void MaximizeWindow(Window window)
    {
        window.LastWidth = Convert.ToInt32(window.Width);
        window.LastHeight = Convert.ToInt32(window.Height);
        AbsoluteLayout.SetLayoutBounds(window, new Rect(0, 0, Width, Height));
        window.WidthRequest = Width;
        window.HeightRequest = Height;
        window.GetBackgroundImage();
    }

    public void MinimizeWindow(Window window)
    {
        window.TranslateTo(-window.Width - 100, Height + 100);
    }

    public void RestoreWindow(Window window)
    {
        if (window.LastWindowState == WindowState.Maximized && window.WindowState != WindowState.Maximized)
        {
            SetWindowSize(window, window.LastWidth, window.LastHeight, true);
            window.TranslateTo(0, 0);
            window.WindowState = WindowState.Maximized;
            return;
        }

        window.TranslateTo(window.LastX, window.LastY);
        SetWindowSize(window, window.LastWidth, window.LastHeight, isRestore: true);
        window.WindowState = WindowState.Standard;
    }

    public void SetWindowPosition(Window window, double x, double y)
    {
        if (y < 0) y = 0;
        SetWindowPosition(window, new Rect(x, y, AutoSize, AutoSize));
    }

    public void SetWindowSize(Window window, int width, int height, bool isRestore = false)
    {
        if (!isRestore)
        {
            window.LastWidth = width;
            window.LastHeight = height;
        }

        var currentBounds = AbsoluteLayout.GetLayoutBounds(window);
        currentBounds.Width = width;
        currentBounds.Height = height;
        AbsoluteLayout.SetLayoutBounds(window, currentBounds);
        window.WidthRequest = width;
        window.HeightRequest = height;
        window.GetBackgroundImage();
    }

    private void GetPosition()
    {
        var lastPosition = AbsoluteLayout.GetLayoutBounds(_pickWindow);
        _lastX = lastPosition.X;
        _lastY = lastPosition.Y;
        _lastWidth = lastPosition.Width;
        _lastHeight = lastPosition.Height;
    }

    private void SetWindowPosition(GestureStatus statusType, double totalX, double totalY)
    {
        if (_pickWindow?.WindowState == WindowState.Maximized)
            return;

        if (statusType == GestureStatus.Completed)
        {
            SetWindowState(_pickWindow, statusType, totalX, totalY);
            _pickWindow?.GetBackgroundImage();
        }

        if (_pickWindow == null || _lastX == null || _lastY == null)
        {
            return;
        }

        Rect newPosition = new(_lastX.Value, _lastY.Value, AutoSize, AutoSize);
        newPosition.X += totalX;
        newPosition.Y += totalY;
        _pickWindow.LastX = newPosition.X;
        _pickWindow.LastY = newPosition.Y;
        SetWindowPosition(_pickWindow, newPosition);
    }

    private void SetWindowPosition(Window window, Rect newPosition)
    {
        AbsoluteLayout.SetLayoutBounds(window, newPosition);
    }

    private void SetWindowSize(GestureStatus statusType, double totalX, double totalY)
    {
        if (_pickWindow == null)
        {
            return;
        }

        if (_lastX == null)
        {
            GetPosition();
        }

        switch (statusType)
        {
            case GestureStatus.Completed:
            case GestureStatus.Canceled:
            case GestureStatus.Running:
                {
                    if (_lastWidth != null)
                    {
                        int widthRequest = Convert.ToInt32(_lastWidth + totalX);
                        int heightRequest = Convert.ToInt32(_lastHeight + totalY);
                        SetWindowSize(_pickWindow, widthRequest, heightRequest);
                    }

                    break;
                }
            case GestureStatus.Started:

                GetPosition();
                break;
        }
    }

    private void SetWindowState(Window? window, GestureStatus statusType, double totalX, double totalY)
    {
        if (window?.WindowState == WindowState.Minimized)
            return;

        switch (statusType)
        {
            case GestureStatus.Started:
                _pickWindow = window;
                GetPosition();
                return;

            case GestureStatus.Completed:
                _pickWindow?.GetBackgroundImage();
                _pickWindow = null;
                _windowOperation = WindowOperation.None;
                break;

            case GestureStatus.Running:
                break;

            case GestureStatus.Canceled:
                break;
        }
    }

    private void WindowPanRecognizer_PanUpdated(object? sender, PanUpdatedEventArgs e)
    {
        switch (sender)
        {
            case WindowTitle windowTitle:
                _windowOperation = WindowOperation.Move;
                SetWindowState(windowTitle.ParentWindow, e.StatusType, e.TotalX, e.TotalY);
                break;

            case WindowContainer windowContainer:
                if (_pickWindow == null)
                    return;

                switch (_windowOperation)
                {
                    case WindowOperation.Move:
                        SetWindowPosition(e.StatusType, e.TotalX, e.TotalY);
                        break;

                    case WindowOperation.Size:
                        SetWindowSize(e.StatusType, e.TotalX, e.TotalY);
                        break;
                }
                break;
        }
    }

    private void WindowSizePanRecognizer_PanUpdated(object? sender, PanUpdatedEventArgs e)
    {
        if (sender is WindowResizer { ParentWindow: { } } resizer)
        {
            _pickWindow = resizer.ParentWindow;
            if (e.StatusType == GestureStatus.Started)
            {
                GetPosition();
                _windowOperation = WindowOperation.Size;
            }
        }
    }
}
