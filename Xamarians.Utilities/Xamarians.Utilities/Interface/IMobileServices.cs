using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xamarians.Utilities.Interface
{
    public interface IMobileServices
    {
        void SendSms(string[] number, string message);
        void DoCall(string number);
        void SendEmail(string message, string[] reciever, string[] cc, string[] bcc, string subject);
        bool CheckInternetConnection();
        event EventHandler OnStatusChanged;
    }
}
