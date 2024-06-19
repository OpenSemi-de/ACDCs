using ACDCs.Interfaces;

namespace ACDCs.App.Desktop;

using ACDCs.App;
using MauiIcons.Material;
using Sharp.UI;
using System.Collections.Generic;
using Rect = Interfaces.Rect;

/// <summary>
/// The Module View (App Window with start menu entry) base class.
/// </summary>
/// <seealso cref="ACDCs.App.Desktop.AppBorderedGrid" />
/// <seealso cref="Sharp.UI.Grid" />
/// <seealso cref="ACDCs.Interfaces.IAppModule" />
public class ModuleView : AppBorderedGrid, IAppModule
{
    private readonly Grid? _bottomBar;
    private readonly AppImageButton? _closeButton;
    private readonly IDesktopView _desktopView;
    private readonly AppImageButton? _iconButton;
    private readonly AbsoluteLayout _mainView;
    private readonly AppImageButton? _maximizeOrRestoreButton;
    private readonly AppImageButton? _minimizeButton;
    private readonly PanGestureRecognizer? _movementPanGestureRecognizer;
    private readonly Label? _resizeField;
    private readonly PanGestureRecognizer? _resizePanGestureRecognizer;
    private readonly IThemeService _themeService;
    private readonly Border? _titleBorder;
    private readonly Grid? _titleContainer;
    private readonly Label? _titleLabel;
    private bool _isMaximized;
    private Rect _lastBounds;

    /// <summary>
    /// Initializes a new instance of the <see cref="ModuleView" /> class.
    /// </summary>
    /// <param name="themeService">The color service.</param>
    /// <param name="desktopView">The desktop view.</param>
    public ModuleView(IThemeService themeService, IDesktopView desktopView) : base(themeService)
    {
        Focused += ModuleView_Focused;

        TapGestureRecognizer item = new();
        item.Tapped += Item_Tapped;
        GestureRecognizers.Add(item);

        Rect position = this.GetAbsoluteLayoutBoundsValue().ToRect();
        _lastBounds = position;

        _themeService = themeService;
        _desktopView = desktopView;
        RowDefinitions([
                new(36),
                new(),
                new(32)
                ]);

        this.BackgroundColor(_themeService.GetColor(ColorDefinition.ModuleBackground));
        this.AbsoluteLayoutFlags(Microsoft.Maui.Layouts.AbsoluteLayoutFlags.None);
        this.AbsoluteLayoutBounds(new Rect(10, 10, 400, 300).FromRect());

        _mainView = new AbsoluteLayout()
            .Row(1)
            .HorizontalOptions(LayoutOptions.Fill)
            .VerticalOptions(LayoutOptions.Fill);

        Content = _mainView.Children;

        Children.Add(_mainView);

        if (HasTitle)
        {
            _titleContainer = new Grid()
                .ColumnDefinitions([
                    new(32),
                    new(),
                    new(32),
                    new(32),
                    new(32)])
                .HorizontalOptions(LayoutOptions.Fill)
                .VerticalOptions(LayoutOptions.Fill);

            _titleBorder = new Border()
                .StrokeThickness(1)
                .BackgroundColor(_themeService.GetColor(ColorDefinition.ModuleTitleBackground))
                .HorizontalOptions(LayoutOptions.Fill)
                .HeightRequest(36);

            _titleLabel = new Label()
                .Text(Title)
                .Column(1)
                .HorizontalOptions(LayoutOptions.Fill)
                .VerticalOptions(LayoutOptions.Fill);

            if (HasIcon)
            {
                _iconButton = new AppImageButton(themeService)
                    .Column(0)
                    .HorizontalOptions(LayoutOptions.Fill)
                    .VerticalOptions(LayoutOptions.Fill)
                    .Icon(Icon);
                _titleContainer.Children.Add(_iconButton);
            }
            else
            {
                _titleLabel
                    .ColumnSpan(2)
                    .Column(0);
            }

            _minimizeButton = new AppImageButton(themeService)
                .Column(2)
                .HorizontalOptions(LayoutOptions.Fill)
                .VerticalOptions(LayoutOptions.Fill)
                .Icon(MaterialIcons.Minimize);
            _minimizeButton.Clicked += MinimizeButton_Clicked;

            _maximizeOrRestoreButton = new AppImageButton(themeService)
                .Column(3)
                .HorizontalOptions(LayoutOptions.Fill)
                .VerticalOptions(LayoutOptions.Fill)
                .Icon(MaterialIcons.Window);
            _maximizeOrRestoreButton.Clicked += MaximizeOrRestoreButton_Clicked;

            _closeButton = new AppImageButton(themeService)
                .Column(4)
                .HorizontalOptions(LayoutOptions.Fill)
                .VerticalOptions(LayoutOptions.Fill)
                .Icon(MaterialIcons.Close);
            _closeButton.Clicked += CloseButton_Clicked;

            _titleContainer.Children.Add(_titleLabel);
            _titleContainer.Children.Add(_minimizeButton);
            _titleContainer.Children.Add(_maximizeOrRestoreButton);
            _titleContainer.Children.Add(_closeButton);

            _titleBorder.Content = _titleContainer;
            _movementPanGestureRecognizer = new();
            _movementPanGestureRecognizer.PanUpdated += MovementPanGestureRecognizer_PanUpdated;

            _titleContainer.GestureRecognizers.Add(_movementPanGestureRecognizer);
            Children.Add(_titleBorder);
        }
        else
        {
            if (HasMovement)
            {
                _movementPanGestureRecognizer = new();
                _movementPanGestureRecognizer.PanUpdated += MovementPanGestureRecognizer_PanUpdated;
                GestureRecognizers.Add(_movementPanGestureRecognizer);
            }
            _mainView
                .RowSpan(2)
                .Row(0);
        }

        if (HasBottomBar)
        {
            _bottomBar = new Grid()
                .ColumnDefinitions([new(), new(32)])
                .HorizontalOptions(LayoutOptions.Fill)
                .VerticalOptions(LayoutOptions.Fill)
                .Row(2);

            if (HasResizeElement)
            {
                _resizeField = new Label()
                    .Text("//")
                    .FontSize(20)
                    .TextCenter()
                    .Column(1)
                    .BackgroundColor(_themeService.GetColor(ColorDefinition.ModuleTitleBackground))
                    .HorizontalOptions(LayoutOptions.Fill)
                    .VerticalOptions(LayoutOptions.Fill);

                _resizePanGestureRecognizer = new();
                _resizePanGestureRecognizer.PanUpdated += ResizePanGestureRecognizer_PanUpdated;
                _resizeField.GestureRecognizers.Add(_resizePanGestureRecognizer);

                _bottomBar.Children.Add(_resizeField);
            }

            Children.Add(_bottomBar);
        }

        if (IsMaximized)
        {
            Maximize();
        }

        _themeService.ThemeChanged += ThemeService_ThemeChanged;
    }

    /// <summary>
    /// The content of the module window.
    /// </summary>
    /// <value>
    /// The content.
    /// </value>
    public new IList<IView> Content { get; }

    /// <summary>
    /// Gets or sets a value indicating whether this instance has bottom bar.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance has bottom bar; otherwise, <c>false</c>.
    /// </value>
    public virtual bool HasBottomBar { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether this instance has icon.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance has icon; otherwise, <c>false</c>.
    /// </value>
    public virtual bool HasIcon { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether this instance has movement.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance has movement; otherwise, <c>false</c>.
    /// </value>
    public virtual bool HasMovement { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether this instance has resize element.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance has resize element; otherwise, <c>false</c>.
    /// </value>
    public virtual bool HasResizeElement { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether this instance has taskbar entry.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance has taskbar entry; otherwise, <c>false</c>.
    /// </value>
    public virtual bool HasTaskbarEntry { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether this instance has title.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance has title; otherwise, <c>false</c>.
    /// </value>
    public virtual bool HasTitle { get; set; } = true;

    /// <summary>
    /// Gets or sets the icon.
    /// </summary>
    /// <value>
    /// The icon.
    /// </value>
    public virtual Enum Icon { get; set; } = MaterialIcons.Circle;

    /// <summary>
    /// Gets or sets a value indicating whether this instance is maximized.
    /// </summary>
    /// <value>
    /// <c>true</c> if this instance is maximized; otherwise, <c>false</c>.
    /// </value>
    public virtual bool IsMaximized { get; set; }

    /// <summary>
    /// Gets a value indicating whether this instance is minimized.
    /// </summary>
    /// <value>
    /// <c>true</c> if this instance is minimized; otherwise, <c>false</c>.
    /// </value>
    public bool IsMinimized { get; set; }

    /// <summary>
    /// Gets the module unique identifier.
    /// </summary>
    /// <value>
    /// The module unique identifier.
    /// </value>
    public Guid ModuleGuid { get; } = Guid.NewGuid();

    /// <summary>
    /// Gets or sets the title.
    /// </summary>
    /// <value>
    /// The title.
    /// </value>
    public virtual string Title { get; set; } = "";

    /// <summary>
    /// Adds the specified child.
    /// </summary>
    /// <param name="child">The child.</param>
#pragma warning disable CS0109 // Element blendet kein vererbtes Element aus; neues Schlüsselwort erforderlich

    public new void Add(IView child)
#pragma warning restore CS0109 // Element blendet kein vererbtes Element aus; neues Schlüsselwort erforderlich
    {
        Content.Add(child);
    }

    /// <summary>
    /// Gets the Zindex.
    /// </summary>
    /// <returns></returns>
    public int GetZIndex()
    {
        return ZIndex;
    }

    /// <summary>
    /// Hides this instance.
    /// </summary>
    public void Hide()
    {
        IsVisible = false;
        IsMinimized = true;
    }

    /// <summary>
    /// Maximizes this instance.
    /// </summary>
    public void Maximize()
    {
        _lastBounds = this.GetAbsoluteLayoutBoundsValue().ToRect();
        this.AbsoluteLayoutFlags(Microsoft.Maui.Layouts.AbsoluteLayoutFlags.All);
        this.AbsoluteLayoutBounds(0, 0, 1, 1);
        _isMaximized = true;
        IsMinimized = false;
    }

    /// <summary>
    /// Quits this instance.
    /// </summary>
    public void Quit()
    {
        TaskHelper.Run(() => _desktopView.Stop(this));
    }

    /// <summary>
    /// Restores this instance.
    /// </summary>
    public void Restore()
    {
        this.AbsoluteLayoutFlags(Microsoft.Maui.Layouts.AbsoluteLayoutFlags.None);
        this.AbsoluteLayoutBounds(_lastBounds.FromRect());
        _isMaximized = false;
        IsMinimized = false;
    }

    /// <summary>
    /// Sets the ZIndex.
    /// </summary>
    /// <param name="zIndex">Index.</param>
    public void SetZIndex(int zIndex)
    {
        this.ZIndex(zIndex);
    }

    /// <summary>
    /// Shows this instance.
    /// </summary>
    public void Show()
    {
        IsVisible = true;
        IsMinimized = false;
    }

    private void CloseButton_Clicked(object? sender, EventArgs e)
    {
        Quit();
    }

    private void Item_Tapped(object? sender, TappedEventArgs e)
    {
        _desktopView.BringToFront(this);
    }

    private void MaximizeOrRestore()
    {
        if (!_isMaximized)
        {
            Maximize();
        }
        else
        {
            Restore();
        }
    }

    private void MaximizeOrRestoreButton_Clicked(object? sender, EventArgs e)
    {
        MaximizeOrRestore();
    }

    private void MinimizeButton_Clicked(object? sender, EventArgs e)
    {
        Hide();
    }

    private void ModuleView_Focused(object? sender, FocusEventArgs e)
    {
        _desktopView.BringToFront(this);
    }

    /// <summary>
    /// Adds a child view to the end of this layout.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="Microsoft.Maui.Controls.PanUpdatedEventArgs" /> instance containing the event data.</param>
    private void MovementPanGestureRecognizer_PanUpdated(object? sender, PanUpdatedEventArgs e)
    {
        if (_isMaximized) return;

        Rect position = this.GetAbsoluteLayoutBoundsValue().ToRect();
        switch (e.StatusType)
        {
            case GestureStatus.Started:
                _lastBounds = position;
                break;

            case GestureStatus.Running:
                double x = _lastBounds.X + e.TotalX;
                double y = _lastBounds.Y + e.TotalY;
                if (x < 0) x = 0;
                if (y < 0) y = 0;

                AbsoluteLayout parent = (AbsoluteLayout)Parent;

                if (x + position.Width > parent.Width) x = parent.Width - position.Width;
                if (y > parent.Height - 70) y = parent.Height - 70;

                this.AbsoluteLayoutBounds(x, y, position.Width, position.Height);
                break;

            case GestureStatus.Completed or GestureStatus.Canceled:
                break;
        }
    }

    private void ResizePanGestureRecognizer_PanUpdated(object? sender, PanUpdatedEventArgs e)
    {
        if (_isMaximized) return;

        Rect position = this.GetAbsoluteLayoutBoundsValue().ToRect();
        switch (e.StatusType)
        {
            case GestureStatus.Started:
                _lastBounds = position;
                break;

            case GestureStatus.Running:
                double width = _lastBounds.Width + e.TotalX + 10;
                double height = _lastBounds.Height + e.TotalY + 10;
                if (width < 200) width = 200;
                if (height < 100) height = 100;

                this.AbsoluteLayoutBounds(position.X, position.Y, width, height);
                break;

            case GestureStatus.Completed or GestureStatus.Canceled:
                break;
        }
    }

    private void ThemeService_ThemeChanged(object? sender, EventArgs e)
    {
        this.BackgroundColor(_themeService.GetColor(ColorDefinition.ModuleBackground));
        _titleContainer?.BackgroundColor(_themeService.GetColor(ColorDefinition.ModuleBackground));
    }
}