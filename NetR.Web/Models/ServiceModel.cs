using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetR.Web.Models
{
    public class ServiceModel
    {
        public int Id { get; set; }
        public string ServerName { get; set; }
        public string ServiceName { get; set; }
        public string ResponsibleServer { get; set; }
        public string Status { get; set; }
        public bool Enabled { get; set; }
    }
}
