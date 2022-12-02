using OSECircuitRender.Interfaces;
using OSECircuitRender.Items;
using CommunityToolkit.Maui.Markup;

namespace OSEInventory.Views
{
    public class FileDialogView : ContentView
    {
        public static readonly BindableProperty SheetPageProperty =
            BindableProperty.Create("SheetPage", typeof(SheetPage), typeof(FileDialogView), null);
        public SheetPage? SheetPage { get; set; }
        public FileDialogView()
        {
            this.Bind(SheetPageProperty, "SheetPage");
            App.MenuButtonView.SetHandler("open", OpenFile);
        }

        public async void OpenFile()
        {
            PickOptions options = new();
            var result = await FilePicker.Default.PickAsync(options);
            if (result != null)
            {
                if (result.FileName.EndsWith("jpg", StringComparison.OrdinalIgnoreCase) ||
                    result.FileName.EndsWith("png", StringComparison.OrdinalIgnoreCase))
                {
                    //                    using var stream = await result.OpenReadAsync();
                    //                    var image = ImageSource.FromStream(() => stream);
                }
            }
        }
    }

    public class EditDialogView : ContentView
    {
        public EditDialogView()
        {
            App.MenuButtonView.SetHandler("duplicate", Duplicate);
            App.MenuButtonView.SetHandler("delete", Delete);
        }

        private void Delete()
        {
            App.CurrentSheet?.SelectedItems.ToList().ForEach(
                item =>
                {
                    App.CurrentSheet?.DeleteItem((WorksheetItem)item);
                });
            App.CurrentSheetPage?.Paint();
        }

        private void Duplicate()
        {
            List<WorksheetItem?> newItems = new();
            App.CurrentSheet?.SelectedItems.ToList().ForEach(
                item => { newItems.Add(App.CurrentSheet?.DuplicateItem((WorksheetItem)item)); }
            );

            App.CurrentSheet?.SelectedItems.Clear();
            newItems.ForEach(item =>
            {
                if (item != null)
                {
                    App.CurrentSheet?.SelectedItems.AddItem(item);
                }
            });

            App.CurrentSheetPage?.Paint();
        }
    }
}
