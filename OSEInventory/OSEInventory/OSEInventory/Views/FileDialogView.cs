namespace OSEInventory.Views
{
    public class FileDialogView : View
    {
        public FileDialogView()
        {
            App.MenuButtonView.SetHandler("open", OpenFile);
        }

        public void OpenFile()
        {

        }
    }
}
