namespace OSEInventory.Views
{
    public class FileDialogView : ContentView
    {
        public FileDialogView()
        {
            App.MenuButtonView.SetHandler("open", OpenFile);
        }

        public async void OpenFile()
        {
            PickOptions options = new();
            var result =  await FilePicker.Default.PickAsync(options);
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
                    App.CurrentSheet?.DeleteItem(item);
                });
            App.CurrentSheetPage?.Paint();
        }

        private void Duplicate()
        {


        }
    }
}
