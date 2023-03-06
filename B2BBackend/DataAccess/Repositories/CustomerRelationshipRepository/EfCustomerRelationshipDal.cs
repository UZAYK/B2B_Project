using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DataAccess.EntityFramework;
using Entities.Concrete;
using DataAccess.Repositories.CustomerRelationshipsRepository;
using DataAccess.Context.EntityFramework;

namespace DataAccess.Repositories.CustomerRelationshipsRepository
{
    public class EfCustomerRelationshipDal : EfEntityRepositoryBase<CustomerRelationship, SimpleContextDb>, ICustomerRelationshipDal
    {
    }
}
