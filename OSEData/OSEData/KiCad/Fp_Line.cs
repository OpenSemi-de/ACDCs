namespace OSEData.KiCad
{
    public class Fp_Line
    {
        public End end { get; set; }
        public string layer { get; set; }
        public Start start { get; set; }
        public string tstamp { get; set; }
        public float width { get; set; }
    }
}