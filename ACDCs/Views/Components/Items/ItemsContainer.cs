﻿using System.Reflection;
using ACDCs.CircuitRenderer.Interfaces;
using ACDCs.CircuitRenderer.Items;
using ACDCs.CircuitRenderer.Sheet;
using Microsoft.Maui.Layouts;

namespace ACDCs.Views.Components.Items;

public class ItemsContainer : StackLayout
{
    public ItemsContainer()
    {
        IsInserting = false;
        Orientation = StackOrientation.Horizontal;
        AbsoluteLayout.SetLayoutBounds(this, new(0, 1, 1, 60));
        AbsoluteLayout.SetLayoutFlags(this, AbsoluteLayoutFlags.WidthProportional | AbsoluteLayoutFlags.YProportional);
        _scroll = new()
        {
            HorizontalScrollBarVisibility = ScrollBarVisibility.Always,
            VerticalScrollBarVisibility = ScrollBarVisibility.Never,
            Orientation = ScrollOrientation.Horizontal
        };


        _layout = new()
        {
            Orientation = StackOrientation.Horizontal
        };
        
        _scroll.Content = _layout;
        Add(_scroll);

        Loaded += OnLoaded;
    }

    public Func<float, float, WorksheetItem?>? DoInsert { get; set; }

    public bool IsInserting
    {
        get => App.Com<bool>("Items", "IsInserting");
        set => App.Com<bool>("Items", "IsInserting", value);
    }

    public AbsoluteLayout PopupTarget
    {
        get => (AbsoluteLayout)GetValue(PopupTargetProperty);

        set => SetValue(PopupTargetProperty, value);
    }

    public ItemButton? SelectedButton { get; set; }

    public Color? SelectedButtonBorderColor { get; set; }

    public Color? SelectedButtonColor { get; set; }

    public async Task InsertToPosition(float x, float y)
    {
        await App.Call(async () =>
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

    public async Task SetupView()
    {
        await App.Call(() =>
        {
            foreach (Type type in typeof(IWorksheetItem).Assembly.GetTypes())
            {
                bool TypeFilter(Type filterType, object? criteria) => filterType == typeof(IWorksheetItem);

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

                    bool isInsertable =
                        (bool)(isInsertableProp.GetValue(Activator.CreateInstance(type), BindingFlags.Instance, null,
                            null,
                            null) ?? false);

                    if (!isInsertable)
                    {
                        continue;
                    }

                    ItemButton button = new(type) { WidthRequest = 60, HeightRequest = 60 };
                    button.Clicked += OnItemButtonClicked;
                    button.SetBackground(BackgroundColor);
                    button.Draw();
                    _layout.Add(
                        button
                               );
                    _layout.WidthRequest += 60;
                }
                catch
                {
                }
            }

            return Task.CompletedTask;
        });
    }

    private static readonly BindableProperty PopupTargetProperty =
        BindableProperty.Create(nameof(PopupTarget), typeof(AbsoluteLayout), typeof(CircuitSheetPage));

    private readonly StackLayout _layout;
    private readonly ScrollView _scroll;

    private async Task DeselectSelectedButton()
    {
        await App.Call(() =>
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
        await App.Call(() =>
        {
            if (item != null)
            {
                App.Com<Func<float, float, Worksheet, WorksheetItem?>?>("Items", "DoInsert",
                    (float x, float y, Worksheet sheet) =>
                    {
                        item.DrawableComponent.Position.X = x;
                        item.DrawableComponent.Position.Y = y;
                        sheet?.Items.AddItem(item);
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
        await App.Call(async () =>
        {
            bool justDeselectAndReturn = SelectedButton == selectedButton;
            IsInserting = false;
            DoInsert = (x, y) => null;
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
        App.Call(async () =>
        {
            if (sender is ItemButton button)
            {
                if (button.ItemType != null)
                {
                    if (InsertItem != null)
                    {
                        await InsertItem(button.ItemType, button);
                    }
                }
            }
        }).Wait();
    }

    private async void OnLoaded(object? sender, EventArgs e)
    {
        await SetupView();
    }

    private async Task SelectButton(ItemButton selectedButton)
    {
        await App.Call(() =>
        {
            SelectedButton = selectedButton;
            SelectedButtonColor = SelectedButton?.BackgroundColor;
            SelectedButtonBorderColor = SelectedButton?.BorderColor;
            if (SelectedButton != null)
            {
                SelectedButton.BorderColor = Color.Parse("#20307f");
                SelectedButton.BackgroundColor = Color.Parse("#dfefff");
            }

            return Task.CompletedTask;
        }, true);
    }
}