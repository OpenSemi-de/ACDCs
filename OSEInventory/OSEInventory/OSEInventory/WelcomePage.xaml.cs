using OSECircuitRender;
using OSECircuitRender.Drawables;
using OSECircuitRender.Items;
using OSECircuitRender.Sheet;

namespace OSEInventory;

public partial class WelcomePage : ContentPage
{
    public WelcomePage()
    {
        InitializeComponent();
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        Workbook wb = new Workbook();
        OSECircuitRender.Log.Method = Console.WriteLine;
        Worksheet ws = wb.AddNewSheet();
        ResistorItem ri = new("10k", 10, 2);
        ws.Items.AddItem(ri);
        TerminalItem pi = new(TerminalDrawableType.Gnd, 10, 8);

        ws.Items.AddItem(pi);

        InductorItem ii = new("10m", 10, 16);

        ws.Items.AddItem(ii);

        //Console.WriteLine(JsonConvert.SerializeObject(wb, Formatting.Indented));
        //	Console.WriteLine(JsonConvert.SerializeObject(wb.Sheets.First().GetDrawableComponents(), Formatting.Indented));
        ws.CalculateScene();
        //Console.WriteLine(JsonConvert.SerializeObject(wb.Sheets[0].SceneManager.GetSceneForBackend(), Formatting.Indented));
        graphicsView.Drawable = (IDrawable)ws.SceneManager.GetSceneForBackend();
    }
}