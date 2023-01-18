namespace ACDCs.Interfaces;

public interface IMenuItem
{
    double ItemHeight { get; set; }
    double ItemWidth { get; set; }
    string MenuCommand { get; set; }
    string Text { get; set; }
}
