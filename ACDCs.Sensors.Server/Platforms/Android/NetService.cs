namespace ACDCs.Sensors.Server.Services;

using System.Net;
using global::Android.App;
using global::Android.Net.Wifi;

public partial class NetService
{
    public partial string ConvertHostIP()
    {
        WifiManager wifiManager = (WifiManager)Application.Context.GetSystemService(Service.WifiService);
        int ip = wifiManager.ConnectionInfo.IpAddress;

        IPAddress ipAddr = new IPAddress(ip);

        //  System.out.println(host);
        return ipAddr.ToString();
    }
}
