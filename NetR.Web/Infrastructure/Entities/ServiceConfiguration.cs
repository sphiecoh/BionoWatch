using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetR.Web.Infrastructure.Entities
{
    public class ServiceConfiguration
    {
        public int Id { get; set; }
        public string ServerName { get; set; }
        public string ServiceName { get; set; }
        public string ResponsibleServer { get; set; }
        public ServiceStatus Status { get; set; }
        public bool Enabled { get; set; }
    }

    public enum ServiceStatus
    {
        Unchecked,
        Checking,
        Up,
        Down
    }
}
