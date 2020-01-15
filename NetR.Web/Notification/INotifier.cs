using NetR.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetR.Web
{
    public interface INotifier
    {
        Task Notify(NotificationModel notification);
    }
}
