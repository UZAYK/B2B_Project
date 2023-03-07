using Core.Utilities.Result.Abstract;
using Entities.Concrete;
using Entities.Dtos;

namespace Business.Repositories.CustomerRepository
{
    public interface ICustomerService
    {
        Task<IResult> Add(CustomerRegisterDto customerRegister);

        Task<IResult> Update(Customer customerRegister);

        Task<IResult> Delete(Customer customerRegister);

        Task<IDataResult<List<Customer>>> GetList();

        Task<IDataResult<Customer>> GetById(int id);

        Task<Customer> GetByEmail(string email);
    }
}