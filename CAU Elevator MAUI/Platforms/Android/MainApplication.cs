using Android.App;
using Android.Gms.Ads;
using Android.Runtime;

namespace CAU_Elevator_MAUI;

[Application]
public class MainApplication : MauiApplication
{
	public MainApplication(IntPtr handle, JniHandleOwnership ownership)
		: base(handle, ownership)
	{
	}

	protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

    public override void OnCreate()
    {
		MobileAds.Initialize(this);
        base.OnCreate();
    }
}
