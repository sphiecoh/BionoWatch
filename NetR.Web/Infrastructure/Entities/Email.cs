using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetR.Web.Infrastructure.Entities
{
    public class EmailNotification
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public ICollection<NotificationGroup> NotificationGroups { get; set; }
    }
}
