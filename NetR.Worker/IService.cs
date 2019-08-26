using System.Collections.Generic;
using System.Threading.Tasks;
using NetR.Worker.Models;
using Refit;

namespace NetR.Worker
{
    public interface IService
    {
         [Get("/api/configuration/{server}/services")]
         Task<IEnumerable<ServiceConfig>> GetServices(string server);
          [Post("/api/configuration/{id}/status/{status}")]
         Task UpdateStatus(int id,string status);
         [Get("/api/configuration/interval")]
         Task<int> GetInterval();
    }
}