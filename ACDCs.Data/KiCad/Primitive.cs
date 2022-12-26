namespace ACDCs.Data.KiCad;

public class Primitive
{
    public Center center { get; set; }
    public End end { get; set; }
    public string fill { get; set; }
    public Mid mid { get; set; }
    public Pt[] pts { get; set; }
    public Start start { get; set; }
    public float width { get; set; }
}
