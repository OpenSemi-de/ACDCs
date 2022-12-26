namespace ACDCs.Data.KiCad;

public class Model
{
    public At at { get; set; }
    public string filename { get; set; }
    public Offset offset { get; set; }
    public Rotate rotate { get; set; }
    public Scale scale { get; set; }
}
