namespace ACDCs.Sensors.Server.Services;

using System.Net;
using System.Net.Sockets;

public partial class NetService
{
    public partial string ConvertHostIP()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }

        return "";
    }
}
