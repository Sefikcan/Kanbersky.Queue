using Kanbersky.Queue.Service.DTO.Request;
using Kanbersky.Queue.Service.DTO.Response;
using System.Threading.Tasks;

namespace Kanbersky.Queue.Service.Abstract
{
    public interface ICustomerService
    {
        Task<CreateCustomerResponse> AddAsync(CreateCustomerRequest request);
    }
}
