namespace ACDCs.Data.KiCad;

public class Pad
{
    public At at { get; set; }
    public float clearance { get; set; }
    public int die_length { get; set; }
    public Drill drill { get; set; }
    public string[] layers { get; set; }
    public bool locked { get; set; }
    public Options options { get; set; }
    public string pad_id { get; set; }
    public string pad_shape { get; set; }
    public string pad_type { get; set; }
    public Primitive[] primitives { get; set; }
    public Rect_Delta rect_delta { get; set; }
    public float roundrect_rratio { get; set; }
    public Size size { get; set; }
    public float solder_mask_margin { get; set; }
    public float solder_paste_margin { get; set; }
    public float solder_paste_margin_ratio { get; set; }
    public string tstamp { get; set; }
    public int zone_connect { get; set; }
}
