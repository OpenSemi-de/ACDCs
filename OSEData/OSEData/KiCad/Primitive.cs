namespace OSEData.KiCad
{
    public class Primitive
    {
        public Pt[] pts { get; set; }
        public float width { get; set; }
        public string fill { get; set; }
        public Center center { get; set; }
        public End end { get; set; }
        public Start start { get; set; }
        public Mid mid { get; set; }
    }
}