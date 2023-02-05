namespace ACDCs.ApplicationLogic.Components.Window;

#pragma warning disable IDE0065

using Sharp.UI;

#pragma warning restore IDE0065

public class WindowContainer : AbsoluteLayout
{
    private readonly PanGestureRecognizer _windowPanRecognizer;
    private readonly List<Window> _windows;
    private Window? _pickWindow;
    private Rect? _lastPosition;

    public WindowContainer()
    {
        _windows = new List<Window>();
        _windowPanRecognizer = new PanGestureRecognizer();
        _windowPanRecognizer.PanUpdated += WindowPanRecognizer_PanUpdated;
        GestureRecognizers.Add(_windowPanRecognizer);
    }

    private void WindowPanRecognizer_PanUpdated(object? sender, PanUpdatedEventArgs e)
    {
        switch (sender)
        {
            case WindowTitle windowTitle:
                SetWindowState(windowTitle.ParentWindow, e.StatusType, e.TotalX, e.TotalY);
                break;

            case WindowContainer windowContainer:
                SetWindowPosition(e.StatusType, e.TotalX, e.TotalY);
                break;
        }
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

        if (_lastPosition != null && _pickWindow != null)
        {
            Rect newPosition = new(_lastPosition.Value.Location, _lastPosition.Value.Size);
            newPosition.X += totalX;
            newPosition.Y += totalY;
            _pickWindow.LastX = newPosition.X;
            _pickWindow.LastY = newPosition.Y;
            SetWindowPosition(_pickWindow, newPosition);
        }
    }

    public void SetWindowPosition(Window window, double x, double y)
    {
        SetWindowPosition(window, new Rect(x, y, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
    }

    private void SetWindowPosition(Window window, Rect newPosition)
    {
        AbsoluteLayout.SetLayoutBounds(window, newPosition);
    }

    private void SetWindowState(Window? window, GestureStatus statusType, double totalX, double totalY)
    {
        if (window?.WindowState == WindowState.Minimized)
            return;

        switch (statusType)
        {
            case GestureStatus.Started:
                _pickWindow = window;
                _lastPosition = AbsoluteLayout.GetLayoutBounds(_pickWindow);
                return;

            case GestureStatus.Completed:
                _pickWindow?.GetBackgroundImage();
                _pickWindow = null;
                _lastPosition = null;
                break;

            case GestureStatus.Running:
                break;

            case GestureStatus.Canceled:
                break;

            default:
                break;
        }
    }

    public void AddWindow(Window window)
    {
        _windows.Add(window);
        window.Title.GestureRecognizers.Add(_windowPanRecognizer);
        Add(window);
    }

    public void MinimizeWindow(Window window)
    {
        window.TranslateTo(-window.Width - 100, Height + 100);
    }

    public void MaximizeWindow(Window window)
    {
        AbsoluteLayout.SetLayoutBounds(window, new Rect(0, 0, Width, Height));
        window.GetBackgroundImage();
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
        window.GetBackgroundImage();
    }

    public void RestoreWindow(Window window)
    {
        AbsoluteLayout.SetLayoutBounds(window, new Rect(window.LastX, window.LastY, window.LastWidth, window.LastHeight));
        SetWindowSize(window, window.LastWidth, window.LastHeight, isRestore: true);
    }

    public void CloseWindow(Window window)
    {
        _windows.Remove(window);
        window.Title.GestureRecognizers.Remove(_windowPanRecognizer);
        Remove(window);
    }
}
