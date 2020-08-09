using api.Model;
using api.Models.EntityModel;
using System.Threading.Tasks;

namespace api.Models.ServiceModel
{
    public interface IAcquirerApi
    {
        Task Process(ICardTransaction payment);
    }
}