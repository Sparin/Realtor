using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Realtor.Model.DTO.Customer;
using Realtor.Model.DTO.Phone;
using Realtor.Model.Entities;
using Realtor.Services.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Realtor.Controllers
{
    [Route("api/[controller]")]
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customers;
        private readonly IPhoneService _phones;
        private readonly IMapper _mapper;
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(
            ICustomerService customers,
            IPhoneService phones,
            IMapper mapper,
            ILogger<CustomerController> logger)
        {
            _phones = phones;
            _customers = customers;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerAsync(int id)
        {
            _logger.LogInformation($"User trying to get customer info with id {id}");
            var entity = await _customers.FindCustomerAsync(id);

            if (entity == default(Customer))
            {
                _logger.LogWarning($"User requested not existing customer by id {id}");
                return NotFound();
            }

            //TODO: Sanitize entities for avoid OWASP Top 10 A7:2017-Cross-Site Scripting (XSS)
            _logger.LogInformation($"User received customer info with id {id}");
            var result = _mapper.Map<CustomerDto>(entity);
            return Ok(result);
        }

        [HttpGet("{customerId}/phones")]
        public async Task<IActionResult> GetPhonesAsync(int customerId)
        {
            _logger.LogInformation($"User trying to get customer's phones with id {customerId}");
            var customer = await _customers.FindCustomerAsync(customerId);

            if (customer == default(Customer))
            {
                _logger.LogWarning($"User requested not existing customer by id {customerId}");
                return NotFound();
            }

            _logger.LogInformation($"User received customer's phones with id {customerId}");
            var result = _mapper.Map<IEnumerable<PhoneDto>>(customer.Phones);
            return Ok(result);
        }

        
        [Authorize]
        [HttpGet("phones")]
        public async Task<IActionResult> GetPhonesAsync()
        {
            var userId = int.Parse(this.User.FindFirstValue(ClaimTypes.NameIdentifier));
            return await GetPhonesAsync(userId);
        }

        [Authorize]
        [HttpPost("phones")]
        public async Task<IActionResult> CreatePhoneAsync([FromBody]CreatePhoneDto requestDto)
        {
            var userId = int.Parse(this.User.FindFirstValue(ClaimTypes.NameIdentifier));
            _logger.LogInformation($"User trying to create new phone");

            var userPhonesCount = await _phones.GetPhonesCountAsync(userId);                

            var entity = _mapper.Map<Phone>(requestDto);
            entity.CustomerId = userId;

            //TODO: Sanitize entities for avoid OWASP Top 10 A7:2017-Cross-Site Scripting (XSS)
            _logger.LogInformation($"Validating new phone");

            if (ModelState.IsValid)
                TryValidateModel(entity);
            if (!ModelState.IsValid || userPhonesCount >= 3)
            {
                if (userPhonesCount >= 3)
                    ModelState.AddModelError("general", "Maximum 3 phone numbers per customer");
                var errors = ModelState.FormatModelErrors();
                _logger.LogWarning($"New phone did not pass entity validation", errors);
                return BadRequest(errors);
            }


            entity = await _phones.CreatePhoneAsync(entity);
            _logger.LogInformation($"User added new phone with identificator {entity.Id}");

            var result = _mapper.Map<PhoneDto>(entity);
            return Ok(result);
        }

        [Authorize]
        [HttpPut("phones/{id}")]
        public async Task<IActionResult> UpdatePhoneAsync(int id, [FromBody]UpdatePhoneDto requestDto)
        {
            _logger.LogInformation($"User trying to update existing phone with id {id}");
            var entity = await _phones.FindPhoneAsync(id);
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (entity == default(Phone))
            {
                _logger.LogWarning($"User requested not existing phone");
                return NotFound();
            }

            if (entity.CustomerId.ToString() != userId)
            {
                _logger.LogWarning($"User is tried to update not his own phone");
                return Forbid();
            }

            entity = _mapper.Map(requestDto, entity);
            //TODO: Sanitize entities for avoid OWASP Top 10 A7:2017-Cross-Site Scripting (XSS)

            _logger.LogInformation($"Validating updated phone");


            if (ModelState.IsValid)
                TryValidateModel(entity);
            if (!ModelState.IsValid)
            {
                var errors = ModelState.FormatModelErrors();
                _logger.LogWarning($"Updated advertisement did not pass entity validation", errors);
                return BadRequest(errors);
            }

            entity = await _phones.UpdatePhoneAsync(entity);
            _logger.LogInformation($"Phone with identificator {entity.Id} updated");

            var result = _mapper.Map<PhoneDto>(entity);
            return Ok(result);
        }

        [Authorize]
        [HttpDelete("phones/{id}")]
        public async Task<IActionResult> DeletePhoneAsync(int id)
        {
            _logger.LogInformation($"User trying to delete phone number with identificator {id}");
            var entity = await _phones.FindPhoneAsync(id);
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (entity == default(Phone))
            {
                _logger.LogWarning($"User requested not existing phone number");
                return NotFound();
            }

            if (entity.CustomerId.ToString() != userId)
            {
                _logger.LogWarning($"User is tried to delete not his own phone number");
                return Forbid();
            }

            await _phones.DeletePhoneAsync(entity);
            _logger.LogInformation($"Phone number with identificator {entity.Id} was deleted");

            return NoContent();
        }
    }
}
