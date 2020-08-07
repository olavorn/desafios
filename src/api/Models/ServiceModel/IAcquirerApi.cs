using api.Model;
using System.Threading.Tasks;

namespace api.Models.ServiceModel
{
    internal interface IAcquirerApi
    {
        Task Process(Payment payment);
    }
}