namespace OSEData.KiCad
{
    public class Zone
    {
        public string net { get; set; }
        public string net_name { get; set; }
        public string layer { get; set; }
        public string tstamp { get; set; }
        public Hatch hatch { get; set; }
        public Connect_Pads connect_pads { get; set; }
        public string min_thickness { get; set; }
        public Keepout keepout { get; set; }
        public Fill fill { get; set; }
        public Polygon[] polygon { get; set; }
        public string[] layers { get; set; }
    }
}