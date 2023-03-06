using System;
using System.Collections.Generic;
using FluentValidation;
using System.Text;
using System.Threading.Tasks;
using Entities.Concrete;

namespace Business.Repositories.CustomerRelationshipsRepository.Validation
{
    public class CustomerRelationshipValidator : AbstractValidator<CustomerRelationship>
    {
        public CustomerRelationshipValidator()
        {
        }
    }
}
