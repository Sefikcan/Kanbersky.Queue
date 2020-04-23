using AutoMapper;
using Kanbersky.Queue.Core.Messaging.Abstract;
using Kanbersky.Queue.Core.Results.Exceptions;
using Kanbersky.Queue.DAL.Concrete.EntityFramework.GenericRepository;
using Kanbersky.Queue.Entities.Concrete;
using Kanbersky.Queue.Service.Abstract;
using Kanbersky.Queue.Service.DTO.Request;
using Kanbersky.Queue.Service.DTO.Response;
using System.Threading.Tasks;

namespace Kanbersky.Queue.Service.Concrete
{
    public class CustomerService : ICustomerService
    {
        #region fields

        private readonly IGenericRepository<Customer> _repository;
        private readonly IMapper _mapper;
        private readonly IExchangeFactory<CreateCustomerResponse> _exchangeFactory;

        #endregion

        #region ctor

        public CustomerService(IGenericRepository<Customer> repository,
            IMapper mapper,
            IExchangeFactory<CreateCustomerResponse> exchangeFactory)
        {
            _repository = repository;
            _mapper = mapper;
            _exchangeFactory = exchangeFactory;
        }

        #endregion

        #region fields

        public async Task<CreateCustomerResponse> AddAsync(CreateCustomerRequest request)
        {
            var entity = _mapper.Map<Customer>(request);
            var response = await _repository.Add(entity);
            if (await _repository.SaveChangesAsync()>0)
            {
                var mappedResponse = _mapper.Map<CreateCustomerResponse>(response);
                _exchangeFactory.CreateExchangeAndSend(mappedResponse);
                return mappedResponse;
            }
            else
            {
                throw new BadRequestException("Customer Insert Fail!");
            }
        }

        #endregion
    }
}
