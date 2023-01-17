using Sharp.UI;

namespace ACDCs.Views.Items
{
    [BindableProperties]
    public interface IItemsViewProperties
    {
        double ButtonHeight { get; set; }
        double ButtonWidth { get; set; }
    }
}
