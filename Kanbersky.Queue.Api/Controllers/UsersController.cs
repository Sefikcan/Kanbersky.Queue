using Kanbersky.Queue.Service.Abstract;
using Kanbersky.Queue.Service.DTO.Request;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Kanbersky.Queue.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        #region fields

        private readonly ICustomerService _customerService;

        #endregion

        #region ctor

        public UsersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        #endregion

        #region methods

        [HttpPost]
        public async Task<IActionResult> Register(CreateCustomerRequest request)
        {
            var response = await _customerService.AddAsync(request);
            return Ok(response);
        }

        #endregion
    }
}