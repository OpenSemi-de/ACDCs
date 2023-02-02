using ACDCs.ApplicationLogic.Components.Window;
using ACDCs.ApplicationLogic.Interfaces;
using Window = ACDCs.ApplicationLogic.Components.Window.Window;

namespace ACDCs.ApplicationLogic.Services;

public class WorkbenchService : IWorkbenchService
{
    public Page GetWorkbenchPage()
    {
        Workbench workbenchPage = new(API.Instance);
        WindowContainer workbenchPageContent = new WindowContainer();
        workbenchPage.Content = workbenchPageContent;
        Window test = new(workbenchPageContent, "Test", "menu_main.json");

        return workbenchPage;
    }
}
