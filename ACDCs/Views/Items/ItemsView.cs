using System.Reflection;
using ACDCs.CircuitRenderer.Interfaces;
using ACDCs.CircuitRenderer.Items;
using ACDCs.CircuitRenderer.Sheet;
using ACDCs.Components.Items;
using ACDCs.Interfaces;
using Microsoft.Maui.Layouts;

namespace ACDCs.Views.Items;

using Sharp.UI;

[SharpObject]
public partial class ItemsView : StackLayout, IItemsViewProperties
{
    private readonly StackLayout _layout;

    private readonly ScrollView _scroll;

    public Func<float, float, WorksheetItem?>? DoInsert { get; set; }

    public bool IsInserting
    {
        get => API.Com<bool>("Items", "IsInserting");
        set => API.Com<bool>("Items", "IsInserting", value);
    }

    public ItemButton? SelectedButton { get; set; }

    public Color? SelectedButtonBorderColor { get; set; }

    public Color? SelectedButtonColor { get; set; }

    public ItemsView()
    {
        IsInserting = false;
        this.Orientation(StackOrientation.Horizontal);
        Microsoft.Maui.Controls.AbsoluteLayout.SetLayoutBounds(this, new Rect(0, 1, 1, 60));
        Microsoft.Maui.Controls.AbsoluteLayout.SetLayoutFlags(this, AbsoluteLayoutFlags.WidthProportional | AbsoluteLayoutFlags.YProportional);

        _layout = new StackLayout()
            .Orientation(StackOrientation.Horizontal);

        _scroll = new ScrollView()
            .HorizontalScrollBarVisibility(ScrollBarVisibility.Always)
            .VerticalScrollBarVisibility(ScrollBarVisibility.Never)
            .Orientation(ScrollOrientation.Horizontal)
            .Content(_layout);

        Add(_scroll);
        Loaded += OnLoaded;
    }

    // ReSharper disable once UnusedMember.Global
    public async Task InsertToPosition(float x, float y)
    {
        await API.Call(async () =>
        {
            WorksheetItem? newItem = DoInsert?.Invoke(x, y);
            if (newItem != null)
            {
                newItem.X -= newItem.Width / 2;
                newItem.Y -= newItem.Height / 2;
            }

            await DeselectSelectedButton();
            IsInserting = false;
        });
    }

    public void SetupView()
    {
#pragma warning disable CS4014
        API.Call(() =>
#pragma warning restore CS4014
        {
            BackgroundColor = BackgroundColor?.WithAlpha(0.5f);

            foreach (Type type in typeof(IWorksheetItem).Assembly.GetTypes())
            {
                if (type.FindInterfaces(TypeFilter, null).Length <= 0)
                {
                    continue;
                }

                try
                {
                    PropertyInfo? isInsertableProp = type.GetProperty("IsInsertable");
                    if (isInsertableProp == null)
                    {
                        continue;
                    }

                    object? instance = Activator.CreateInstance(type);
                    bool isInsertable =
                        (bool)(isInsertableProp.GetValue(instance, BindingFlags.Instance, null, null, null) ?? false);

                    if (!isInsertable)
                    {
                        continue;
                    }

                    ItemButton button = new(type, ButtonWidth, ButtonHeight);
                    button.Clicked += OnItemButtonClicked;
                    if (BackgroundColor != null)
                    {
                        button.SetBackground(BackgroundColor.WithAlpha(0.7f));
                    }

                    button.Draw();
                    _layout.Add(button);
                    _layout.WidthRequest += 60;
                }
                catch (Exception exception)
                {
#pragma warning disable CS4014
                    API.PopupException(exception);
#pragma warning restore CS4014
                }
            }

            return Task.CompletedTask;
        });
    }

    private static bool TypeFilter(Type filterType, object? criteria) => filterType == typeof(IWorksheetItem);

    private async Task DeselectSelectedButton()
    {
        await API.Call(() =>
        {
            if (SelectedButton != null)
            {
                SelectedButton.BackgroundColor = SelectedButtonColor;
            }

            SelectedButton = null;
            return Task.CompletedTask;
        }, true);
    }

    private async Task Insert(WorksheetItem? item)
    {
        await API.Call(() =>
        {
            if (item != null)
            {
                API.Com<Func<float, float, Worksheet, WorksheetItem?>?>("Items", "DoInsert",
                    (float x, float y, Worksheet sheet) =>
                    {
                        item.DrawableComponent.Position.X = x;
                        item.DrawableComponent.Position.Y = y;
                        sheet.Items.AddItem(item);
                        DeselectSelectedButton().Wait();
                        return item;
                    });
            }

            IsInserting = true;
            return Task.CompletedTask;
        });
        IsInserting = true;
    }

    private async Task InsertItem(Type itemType, ItemButton selectedButton)
    {
        await API.Call(async () =>
        {
            bool justDeselectAndReturn = SelectedButton == selectedButton;
            IsInserting = false;
            DoInsert = delegate { return null; };
            await DeselectSelectedButton();
            if (justDeselectAndReturn) return;

            await SelectButton(selectedButton);

            if (Activator.CreateInstance(itemType) is WorksheetItem item)
            {
                await Insert(item);
            }

            if (!IsInserting)
                await DeselectSelectedButton();
        });
    }

    private void OnItemButtonClicked(object? sender, EventArgs e)
    {
        API.Call(async () =>
        {
            if (sender is ItemButton { ItemType: { } } button)
            {
                await InsertItem(button.ItemType, button);
            }
        }).Wait();
    }

    private void OnLoaded(object? sender, EventArgs e)
    {
        SetupView();
    }

    private async Task SelectButton(ItemButton selectedButton)
    {
        await API.Call(() =>
        {
            SelectedButton = selectedButton;
            SelectedButtonColor = SelectedButton?.BackgroundColor;
            SelectedButtonBorderColor = SelectedButton?.BorderColor;
            if (SelectedButton == null)
            {
                return Task.CompletedTask;
            }

            SelectedButton.BorderColor = Color.Parse("#aa20307f");
            SelectedButton.BackgroundColor = Color.Parse("#aadfefff");

            return Task.CompletedTask;
        }, true);
    }
}
