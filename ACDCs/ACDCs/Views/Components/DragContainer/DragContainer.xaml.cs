using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace ACDCs.Views.Components.DragContainer;

public partial class DragContainer : ContentView
{
    public static readonly BindableProperty TitleProperty =
        BindableProperty.Create(nameof(Title), typeof(string), typeof(DragContainer), "Title", propertyChanged: propertyChanged);
    
    public static readonly BindableProperty LayoutProperty =
        BindableProperty.Create(nameof(Layout), typeof(IView), typeof(DragContainer), null, propertyChanged: propertyChanged);

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
        BindableProperty.Create(nameof(Orientation), typeof(StackOrientation), typeof(DragContainer), StackOrientation.Vertical, propertyChanged: propertyChanged);


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
    }
}