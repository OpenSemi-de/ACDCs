using ACDCs.ApplicationLogic.Interfaces;

namespace ACDCs.ApplicationLogic.Services;

public class WorkbenchService : IWorkbenchService
{
    public Page GetWorkbenchPage()
    {
        Workbench workbenchPage = new(API.Instance);

        return workbenchPage;
    }
}
