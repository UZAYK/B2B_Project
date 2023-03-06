using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Concrete;
using Core.Utilities.Result.Abstract;
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
