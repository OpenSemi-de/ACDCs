namespace OSEData.KiCad
{
    public class Model
    {
        public string filename { get; set; }
        public Offset offset { get; set; }
        public Scale scale { get; set; }
        public Rotate rotate { get; set; }
        public At at { get; set; }
    }
}