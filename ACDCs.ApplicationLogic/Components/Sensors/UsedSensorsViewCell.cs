namespace ACDCs.API.Core.Components.Sensors;

using Interfaces;

[SharpObject]
public partial class UsedSensorsTemplate : DataTemplate, IUsedSensorsTemplateProperties
{
    public UsedSensorsTemplate()
    {
        this.Add(OnLoadTemplate);
    }

    private object OnLoadTemplate()
    {
        return new ViewCell();
    }
}
