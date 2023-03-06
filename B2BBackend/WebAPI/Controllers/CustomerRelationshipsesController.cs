using Business.Repositories.CustomerRelationshipsRepository;
using Entities.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerRelationshipsesController : ControllerBase
    {
        private readonly ICustomerRelationshipService _customerRelationshipsService;

        public CustomerRelationshipsesController(ICustomerRelationshipService customerRelationshipsService)
        {
            _customerRelationshipsService = customerRelationshipsService;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Add(CustomerRelationship customerRelationships)
        {
            var result = await _customerRelationshipsService.Add(customerRelationships);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Update(CustomerRelationship customerRelationships)
        {
            var result = await _customerRelationshipsService.Update(customerRelationships);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Delete(CustomerRelationship customerRelationships)
        {
            var result = await _customerRelationshipsService.Delete(customerRelationships);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetList()
        {
            var result = await _customerRelationshipsService.GetList();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _customerRelationshipsService.GetById(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

    }
}
