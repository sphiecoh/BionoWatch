using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetR.Web.Infrastructure.Entities
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<NotificationGroup> NotificationGroups { get; set; }
        public ICollection<ServiceGroup> ServiceGroups { get; set; }
    }
}
