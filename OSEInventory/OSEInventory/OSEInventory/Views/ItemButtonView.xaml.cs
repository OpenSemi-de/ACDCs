using System.Reflection;
using OSECircuitRender.Interfaces;
using OSECircuitRender.Items;
using OSEInventory.Components;

namespace OSEInventory.Views
{
    public partial class ItemButtonView : ContentView
    {
        public ItemButtonView()
        {
            InitializeComponent();
        }

        public Func<float, float, WorksheetItem?>? DoInsert { get; set; }

   
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

                    PropertyInfo? isInsertableProp = type.GetProperty("IsInsertable");
                    if (isInsertableProp == null)
                    {
                        continue;
                    }

                    bool isInsertable =
                        (bool)(isInsertableProp.GetValue(null, BindingFlags.Static, null, null, null) ?? false);

                    if (!isInsertable)
                    {
                        continue;
                    }

                    ItemButton button = new(type) { WidthRequest = 60, HeightRequest = 60 };
                    button.Clicked += OnItemButtonClicked;
                    slComponentButtons.Add(
                        button
                    );
                }

                return Task.CompletedTask;
            });
        }

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
            });
        }

        private async Task Insert(WorksheetItem? item)
        {
            await App.Call(() =>
            {
                if (item != null)
                {
                    DoInsert = (x, y) =>
                    {
                        item.DrawableComponent.Position.X = x;
                        item.DrawableComponent.Position.Y = y;
                        App.CurrentSheet?.Items.AddItem(item);
                        return item;
                    };
                }

                IsInserting = true;
                return Task.CompletedTask;
            });
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

        private async Task SelectButton(ItemButton selectedButton)
        {
            await App.Call(() =>
            {
                SelectedButton = selectedButton;
                SelectedButtonColor = SelectedButton?.BackgroundColor;
                SelectedButtonBorderColor = SelectedButton?.BorderColor;
                if (SelectedButton != null)
                {
                    SelectedButton.BorderColor = Colors.SlateGray;
                    SelectedButton.BackgroundColor = Colors.LightSkyBlue;
                }

                return Task.CompletedTask;
            });
        }
    }
}
