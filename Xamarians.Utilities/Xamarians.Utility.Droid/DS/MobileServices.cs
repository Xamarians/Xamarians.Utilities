using System.Linq;
using Android.Content;
using Xamarians.Utility.Droid.DS;
using Xamarians.Utilities.Interface;
using Android.Net;
using Android.Widget;
using Java.Net;
using Android.App;
using System;
[assembly: Xamarin.Forms.Dependency(typeof(MobileServices))]
namespace Xamarians.Utility.Droid.DS
{
    public class MobileServices : IMobileServices
    {
       static ConnectivityManager connectivityManager;
       static NetworkInfo networkInfo;
        public MobileServices()
        {
            var _broadcastReceiver = new NetworkStatusBroadcastReceiver();
            _broadcastReceiver.ConnectionStatusChanged += OnNetworkStatusChanged;
            Application.Context.RegisterReceiver(_broadcastReceiver,
            new IntentFilter(ConnectivityManager.ConnectivityAction));

        }
        public bool CheckInternetConnection()
        {
            bool connected = false;
             connectivityManager = (ConnectivityManager)Xamarin.Forms.Forms.Context.GetSystemService(Context.ConnectivityService);
             networkInfo = connectivityManager.ActiveNetworkInfo;
            if (networkInfo != null && networkInfo.IsConnectedOrConnecting)
            {
                if (networkInfo.Type == ConnectivityType.Mobile)
                {
                    connected = networkInfo.IsConnected;
                }

                if (networkInfo.Type == ConnectivityType.Wifi )
                {
                    connected = networkInfo.IsConnected;
                }
                else            
                connected = false;
            }
            return connected;
        }

        [BroadcastReceiver()]
        public class NetworkStatusBroadcastReceiver : BroadcastReceiver
        {

            public event EventHandler ConnectionStatusChanged;

            public override void OnReceive( Context context, Intent intent)
            {         
               ConnectionStatusChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public event EventHandler OnStatusChanged;

        private void OnNetworkStatusChanged(object sender, EventArgs e)
        {
                OnStatusChanged?.Invoke(this,e);
        }

        public void DoCall(string number)
        {
            var uri = Android.Net.Uri.Parse("tel:" + number);
            var intent = new Intent(Intent.ActionDial, uri);
            Xamarin.Forms.Forms.Context.StartActivity(intent);
        }

        public void SendEmail(string message, string[] reciever, string[] cc, string[] bcc, string subject)
        {
            var email = new Intent(Intent.ActionSend);
            email.PutExtra(Intent.ExtraEmail, reciever);
            email.PutExtra(Intent.ExtraCc, cc);
            email.PutExtra(Intent.ExtraBcc, bcc);
            email.PutExtra(Intent.ExtraSubject, subject);
            email.PutExtra(Intent.ExtraText, message);
            email.SetType("message/rfc822");
            Xamarin.Forms.Forms.Context.StartActivity(email);
        }

        public void SendSms(string[] number, string message)
        {
            string strNumber = null;
            strNumber = string.Join(";", number);
            if (!string.IsNullOrWhiteSpace(strNumber))
            {
                var smsUri = Android.Net.Uri.Parse("smsto:" + strNumber);
                var smsIntent = new Intent(Intent.ActionSendto, smsUri);
                smsIntent.PutExtra("sms_body", message);
                Xamarin.Forms.Forms.Context.StartActivity(smsIntent);
            }

        }
    }
}