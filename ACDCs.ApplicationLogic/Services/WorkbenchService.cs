namespace ACDCs.API.Core.Services;

using ACDCs.API.Interfaces;

using Instance;

public class WorkbenchService : IWorkbenchService
{
    public Page GetWorkbenchPage()
    {
        Workbench workbenchPage = new(API.Instance);

        return workbenchPage;
    }
}
