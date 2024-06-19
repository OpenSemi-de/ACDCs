namespace ACDCs.Sensors.Server.Services;

using System.Net;

#if ANDROID

using Android.App;
using Android.Net.Wifi;

#elif WINDOWS

using System.Net.Sockets;
#endif

public class NetService
{
    public string ConvertHostIP()
    {
#if WINDOWS
       var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
#elif ANDROID

        WifiManager wifiManager = (WifiManager)Application.Context.GetSystemService(Service.WifiService);
        int ip = wifiManager.ConnectionInfo.IpAddress;

        IPAddress ipAddr = new IPAddress(ip);

        //  System.out.println(host);
        return ipAddr.ToString();
#endif
        return "";
    }
}
