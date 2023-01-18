namespace ACDCs.Interfaces;

public interface IMenuComponent
{
    double ItemHeight { get; set; }
    double ItemWidth { get; set; }
    string MenuCommand { get; set; }
    string Text { get; set; }
}
