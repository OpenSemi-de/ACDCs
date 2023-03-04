namespace ACDCs.API.Core.Services;

using Instance;
using Interfaces;

public class WorkbenchService : IWorkbenchService
{
    public Page GetWorkbenchPage()
    {
        Workbench workbenchPage = new(API.Instance);

        return workbenchPage;
    }
}
