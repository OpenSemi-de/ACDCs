using System.Collections.Generic;

namespace ACDCs.Views.Components.Menu;

public interface IMenuItem
{
    string Text { get; set; }
    string MenuCommand { get; set; }

    double ItemHeight { get; set; }
}