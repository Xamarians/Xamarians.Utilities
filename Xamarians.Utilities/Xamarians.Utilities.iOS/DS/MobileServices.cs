using System;
using Foundation;
using UIKit;
using MessageUI;
using System.Threading.Tasks;
using Xamarians.Utilities.iOS.Helpers;
using Xamarians.Utilities.Interface;
using Xamarians.Utilities.iOS.DS;

[assembly: Xamarin.Forms.Dependency(typeof(MobileServices))]

namespace Xamarians.Utilities.iOS.DS
{
    public class MobileServices : IMobileServices
    {
        public event EventHandler OnStatusChanged;
        NetworkStatus remoteHostStatus, internetStatus, localWifiStatus;

		public static void Initialize()
		{
		}
        public MobileServices()
        {
            UpdateStatus(null, null);
            Reachability.ReachabilityChanged += UpdateStatus;

        }
        public void UpdateStatus(object sender, EventArgs e)
        {
            remoteHostStatus = Reachability.RemoteHostStatus();
            internetStatus = Reachability.InternetConnectionStatus();
            localWifiStatus = Reachability.LocalWifiConnectionStatus();
            OnStatusChanged?.Invoke(this, e);

        }
        public bool CheckInternetConnection()
        {

            if (!Reachability.IsHostReachable("http://google.com"))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void DoCall(string number)
        {
            var url = new NSUrl("tel:" + number);
            UIApplication.SharedApplication.OpenUrl(url);
        }

        public async void SendEmail(string message, string[] reciever, string[] cc, string[] bcc, string subject)
        {
            await SendEMAIL(message, reciever, cc, bcc, subject);
        }

        public async void SendSms(string[] number, string message)
        {
            await SendSMS(number, message);
        }

        private UIViewController GetController()
        {
            return UIApplication.SharedApplication.KeyWindow.RootViewController;
        }

        Task<string> SendSMS(string[] number, string message)
        {
            var task = new TaskCompletionSource<string>();
            try
            {
                if (number != null)
                {

                    MFMessageComposeViewController messageController;
                    if (MFMessageComposeViewController.CanSendText)
                    {
                        string[] recepient = number;
                        //Task.Delay(2000);
                        messageController = new MFMessageComposeViewController();
                        messageController.Recipients = recepient;
                        messageController.Body = message;
                        messageController.Finished += (s, args) =>
                        {
                            var result = args.Result.ToString();
                            if (result.Equals("Sent"))
                            {
                                task.SetResult("true");
                            }
                            else if (result.Equals("Cancelled"))
                            {
                                task.SetResult("false");
                            }
                            args.Controller.DismissViewController(true, null);
                        };

                        var controller = GetController();
                        controller.PresentViewController(messageController, true, null);
                    }

                }
            }
            catch (Exception ex)
            {
                task.SetException(ex);
            }
            return task.Task;
        }

        Task<string> SendEMAIL(string message, string[] emailId, string[] cc, string[] bcc, string subject)
        {
            var task = new TaskCompletionSource<string>();
            try
            {
                if (emailId != null)
                {
                    MFMailComposeViewController mailController;
                    if (MFMailComposeViewController.CanSendMail)
                    {
                        mailController = new MFMailComposeViewController();
                        mailController.SetToRecipients(emailId);
                        mailController.SetCcRecipients(cc);
                        mailController.SetBccRecipients(bcc);
                        mailController.SetSubject(subject);
                        var bodyContent = message;

                        mailController.SetMessageBody(bodyContent, true);
                        mailController.Finished += (object s, MFComposeResultEventArgs args) =>
                        {
                            var result = args.Result.ToString();
                            if (result.Equals("Sent"))
                            {
                                task.SetResult("true");
                            }
                            else if (result.Equals("Cancelled"))
                            {
                                task.SetResult("false");
                            }
                            args.Controller.DismissViewController(true, null);

                        };

                        //mailController.Delegate = this;
                        var controller = GetController();
                        controller.PresentViewController(mailController, true, null);

                    }
                }
            }
            catch (Exception ex)
            {
                task.SetResult("false");
            }

            return task.Task;
        }

    }
}
