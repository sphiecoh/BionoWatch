using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetR.Web.Models
{
    public class NotificationModel
    {
        public SMS? SMS;

        public Email? Email;
        public bool IsEmail => Email.HasValue;
        public bool IsSMS => SMS.HasValue;

    }
    public struct SMS
    {
        public string Message { get; set; }
        public string Recipients { get; set; }
    }
    public struct Email
    {
        public string FromAddress { get; set; }
        public string Subject { get; set; }
        public string Recipients { get; set; }
        public string Message { get; set; }
    }
}
