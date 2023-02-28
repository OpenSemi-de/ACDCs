namespace ACDCs.Sensors.Server;

#if ANDROID

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;

public interface IBackgroundService
{
    void Start();

    void Stop();
}

public class BackgroundService : Service, IBackgroundService
{
    public override IBinder? OnBind(Intent? intent) => throw new NotImplementedException();

    [return: GeneratedEnum]
    public override StartCommandResult OnStartCommand(Intent? intent, [GeneratedEnum] StartCommandFlags flags, int startId)
    {
        if (intent == null)
        {
            return StartCommandResult.NotSticky;
        }

        switch (intent.Action)
        {
            case "START_SERVICE":
                RegisterNotification();
                break;

            case "STOP_SERVICE":
                StopForeground(true);
                StopSelfResult(startId);
                break;
        }

        return StartCommandResult.NotSticky;
    }

    public void Start()
    {
        Intent startService = new(MainActivity.ActivityCurrent, typeof(BackgroundService));
        startService.SetAction("START_SERVICE");
        MainActivity.ActivityCurrent.StartService(startService);
    }

    public void Stop()
    {
        Intent stopIntent = new(MainActivity.ActivityCurrent, this.Class);
        stopIntent.SetAction("STOP_SERVICE");
        MainActivity.ActivityCurrent.StartService(stopIntent);
    }

    private void RegisterNotification()
    {
        NotificationChannel channel = new("ServiceChannel", "ServiceDemo", NotificationImportance.Max);
        NotificationManager? manager = MainActivity.ActivityCurrent.GetSystemService(Context.NotificationService) as NotificationManager;
        manager?.CreateNotificationChannel(channel);
        Notification notification = new Notification.Builder(this, "ServiceChannel")
            .SetContentTitle("Service Working")
            .SetSmallIcon(Resource.Drawable.abc_ab_share_pack_mtrl_alpha)
            .SetOngoing(true)
            .Build();

        StartForeground(100, notification);
    }
}

#endif
