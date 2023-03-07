using Business.Aspects.Secured;
using Business.Repositories.CustomerRepository.Constants;
using Business.Repositories.CustomerRepository.Validation;
using Core.Aspects.Caching;
using Core.Aspects.Performance;
using Core.Aspects.Validation;
using Core.Utilities.Business;
using Core.Utilities.Hashing;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using DataAccess.Repositories.CustomerRepository;
using Entities.Concrete;
using Entities.Dtos;

namespace Business.Repositories.CustomerRepository
{
    public class CustomerManager : ICustomerService
    {
        private readonly ICustomerDal _customerDal;

        public CustomerManager(ICustomerDal customerDal)
        {
            _customerDal = customerDal;
        }

        ////[SecuredAspect()]
        [ValidationAspect(typeof(CustomerValidator))]
        [RemoveCacheAspect("ICustomerService.Get")]
        public async Task<IResult> Add(CustomerRegisterDto customerRegister)
        {
            IResult result = BusinessRules.Run(await CheckIfEmailExists(customerRegister.Email));
            if (result is not null)
                return result;

            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePassword(customerRegister.Password, out passwordHash, out passwordSalt);

            Customer customer = new Customer()
            {
                Email = customerRegister.Email,
                Name = customerRegister.Name,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
            };
            await _customerDal.Add(customer);
            return new SuccessResult(CustomerMessages.Added);
        }

        public async Task<Customer> GetByEmail(string email)
        {
            var result = await _customerDal.Get(p => p.Email == email);
            return result;
        }

        //[SecuredAspect()]
        [ValidationAspect(typeof(CustomerValidator))]
        [RemoveCacheAspect("ICustomerService.Get")]
        public async Task<IResult> Update(Customer customer)
        {
            await _customerDal.Update(customer);
            return new SuccessResult(CustomerMessages.Updated);
        }

        //[SecuredAspect()]
        [RemoveCacheAspect("ICustomerService.Get")]
        public async Task<IResult> Delete(Customer customer)
        {
            await _customerDal.Delete(customer);
            return new SuccessResult(CustomerMessages.Deleted);
        }

        ////[SecuredAspect()]
        [CacheAspect()]
        [PerformanceAspect()]
        public async Task<IDataResult<List<Customer>>> GetList()
        {
            return new SuccessDataResult<List<Customer>>(await _customerDal.GetAll());
        }

        //[SecuredAspect()]
        public async Task<IDataResult<Customer>> GetById(int id)
        {
            return new SuccessDataResult<Customer>(await _customerDal.Get(p => p.Id == id));
        }

        private async Task<IResult> CheckIfEmailExists(string email)
        {
            var list = await GetByEmail(email);
            if (list != null)
                return new ErrorResult("Bu mail adresi daha önce kullanýlmýþ");

            return new SuccessResult();
        }
    }
}