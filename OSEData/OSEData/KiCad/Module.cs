namespace OSEData.KiCad
{
    public class Module
    {
        public string name { get; set; }
        public string[] version { get; set; }
        public string[] generator { get; set; }
        public string layer { get; set; }
        public string tedit { get; set; }
        public string descr { get; set; }
        public string tags { get; set; }
        public string[] module_attribute { get; set; }
        public Fp_Text[] fp_text { get; set; }
        public Fp_Line[] fp_line { get; set; }
        public Fp_Arc[] fp_arc { get; set; }
        public Fp_Circle[] fp_circle { get; set; }
        public Pad[] pad { get; set; }
        public Model[] model { get; set; }
        public Zone[] zone { get; set; }
        public Fp_Rect[] fp_rect { get; set; }
        public Fp_Poly[] fp_poly { get; set; }
        public float[] solder_paste_margin { get; set; }
        public float[] solder_mask_margin { get; set; }
        public float[] solder_paste_ratio { get; set; }
        public float[] clearance { get; set; }
    }
}