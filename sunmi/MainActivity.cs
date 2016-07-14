using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using Woyou.Aidlservice.Jiuiv5;

namespace sunmi
{
	[Activity (Label = "sunmi", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity
	{
		private ICallback callback = null;
		private Intent intent = new Intent ();
		private WoyouServiceConnection connService = new WoyouServiceConnection ();

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			// Get our button from the layout resource,
			// and attach an event to it
			Button button = FindViewById<Button> (Resource.Id.myButton);

			intent.SetPackage ("woyou.aidlservice.jiuiv5");
			intent.SetAction ("woyou.aidlservice.jiuiv5.IWoyouService");
			StartService (intent);
			BindService (intent, connService, 0);
			callback = default (ICallback);

			button.Click += delegate {
				try {
					connService.Service.PrinterInit (callback);
					connService.Service.SetAlignment (1, callback);
					connService.Service.PrintQRCode ("http://weixin.qq.com/r/znVheSDEAn19rWQa9yDc", 8, 2, callback);
					connService.Service.LineWrap (3, callback);
				} catch (RemoteException e) {
					e.PrintStackTrace ();
				}
			};
		}

		protected override void OnDestroy ()
		{
			UnbindService (connService);
			base.OnDestroy ();
		}
	}

	public class WoyouServiceConnection : Java.Lang.Object, IServiceConnection
	{

		public IWoyouService Service {
			get; private set;
		}

		public void OnServiceConnected (ComponentName name, IBinder service)
		{
			Service = IWoyouServiceStub.AsInterface (service);
		}

		public void OnServiceDisconnected (ComponentName name)
		{
			Service = null;
		}
	}
}


