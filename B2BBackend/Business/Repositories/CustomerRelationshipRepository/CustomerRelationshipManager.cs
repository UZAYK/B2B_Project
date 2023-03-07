using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Repositories.CustomerRelationshipsRepository;
using Entities.Concrete;
using Business.Aspects.Secured;
using Core.Aspects.Validation;
using Core.Aspects.Caching;
using Core.Aspects.Performance;
using Business.Repositories.CustomerRelationshipsRepository.Validation;
using Business.Repositories.CustomerRelationshipsRepository.Constants;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using DataAccess.Repositories.CustomerRelationshipsRepository;

namespace Business.Repositories.CustomerRelationshipsRepository
{
    public class CustomerRelationshipManager : ICustomerRelationshipService
    {
        private readonly ICustomerRelationshipDal _customerRelationshipsDal;

        public CustomerRelationshipManager(ICustomerRelationshipDal customerRelationshipsDal)
        {
            _customerRelationshipsDal = customerRelationshipsDal;
        }

        ////[SecuredAspect()]
        [ValidationAspect(typeof(CustomerRelationshipValidator))]
        [RemoveCacheAspect("ICustomerRelationshipsService.Get")]

        public async Task<IResult> Add(CustomerRelationship customerRelationships)
        {
            await _customerRelationshipsDal.Add(customerRelationships);
            return new SuccessResult(CustomerRelationshipMessages.Added);
        }

        //[SecuredAspect()]
        [ValidationAspect(typeof(CustomerRelationshipValidator))]
        [RemoveCacheAspect("ICustomerRelationshipsService.Get")]

        public async Task<IResult> Update(CustomerRelationship customerRelationships)
        {
            await _customerRelationshipsDal.Update(customerRelationships);
            return new SuccessResult(CustomerRelationshipMessages.Updated);
        }

        //[SecuredAspect()]
        [RemoveCacheAspect("ICustomerRelationshipsService.Get")]

        public async Task<IResult> Delete(CustomerRelationship customerRelationships)
        {
            await _customerRelationshipsDal.Delete(customerRelationships);
            return new SuccessResult(CustomerRelationshipMessages.Deleted);
        }

        //[SecuredAspect()]
        [CacheAspect()]
        [PerformanceAspect()]
        public async Task<IDataResult<List<CustomerRelationship>>> GetList()
        {
            return new SuccessDataResult<List<CustomerRelationship>>(await _customerRelationshipsDal.GetAll());
        }

        //[SecuredAspect()]
        public async Task<IDataResult<CustomerRelationship>> GetById(int id)
        {
            return new SuccessDataResult<CustomerRelationship>(await _customerRelationshipsDal.Get(p => p.Id == id));
        }

    }
}
