using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetR.Web.Infrastructure.Entities
{
    public class NotificationGroup
    {
        public int EmailNotificationId { get; set; }
        public EmailNotification EmailNotification { get; set; }
        public int GroupId { get; set; }
        public Group Group { get; set; }
    }
}
