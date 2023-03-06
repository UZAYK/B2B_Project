using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Concrete;
using Core.Utilities.Result.Abstract;

namespace Business.Repositories.CustomerRelationshipsRepository
{
    public interface ICustomerRelationshipService
    {
        Task<IResult> Add(CustomerRelationship customerRelationships);
        Task<IResult> Update(CustomerRelationship customerRelationships);
        Task<IResult> Delete(CustomerRelationship customerRelationships);
        Task<IDataResult<List<CustomerRelationship>>> GetList();
        Task<IDataResult<CustomerRelationship>> GetById(int id);
    }
}
