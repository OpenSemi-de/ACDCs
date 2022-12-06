using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSECircuitRender.Interfaces
{
    public interface IHaveAParent
    {
        IWorksheetItem? ParentItem { get; set; }
    }
}
