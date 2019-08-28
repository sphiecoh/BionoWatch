using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetR.Web.Infrastructure.Entities
{
    public class ServiceGroup
    {
        public int ServiceId { get; set; }
        public ServiceConfiguration Service { get; set; }
        public int GroupId { get; set; }
        public Group Group { get; set; }
    }
}
