using Microsoft.AspNetCore.Mvc;
using PromoCodeFactory.WebHost.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;

namespace PromoCodeFactory.WebHost.Controllers
{
    /// <summary>
    /// Управление клиентами: список, получение по Id, создание, обновление и удаление.
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly IRepository<Customer> customerRepository;
        private readonly IRepository<Preference> preferenceRepository;
        private readonly IRepository<PromoCode> promocodesRepository;

        public CustomersController(
            IRepository<Customer> customerRepository,
            IRepository<Preference> preferenceRepository,
            IRepository<PromoCode> promocodesRepository)
        {
            this.customerRepository = customerRepository;
            this.preferenceRepository = preferenceRepository;
            this.promocodesRepository = promocodesRepository;
        }

        /// <summary>Возвращает список всех клиентов.</summary>
        [HttpGet]
        public async Task<ActionResult<List<CustomerShortResponse>>> GetCustomersAsync()
        {
            var customers = await customerRepository.GetAllAsync();

            var result = customers.Select(c => new CustomerShortResponse
            {
                Id = c.Id,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Email = c.Email
            }).ToList();

            return result;
        }

        /// <summary>Возвращает клиента по идентификатору вместе с его промокодами.</summary>
        [HttpGet("{id:guid}", Name = "GetCustomerById")]
        public async Task<ActionResult<CustomerResponse>> GetCustomerAsync(Guid id)
        {
            var customer = await customerRepository.GetByIdAsync(id);
            if (customer == null) return NotFound();

            var allPromocodes = await promocodesRepository.GetAllAsync();
            var customerCodes = allPromocodes.Where(p => p.CustomerId == id).ToList();

            var dto = new CustomerResponse
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                PromoCodes = customerCodes.Select(p => new PromoCodeShortResponse
                {
                    Id = p.Id,
                    Code = p.Code,
                    ServiceInfo = p.ServiceInfo,
                    BeginDate = p.BeginDate.ToString("yyyy.MM.dd"),
                    EndDate = p.EndDate.ToString("yyyy.MM.dd"),
                    PartnerName = p.PartnerName
                }).ToList()
            };

            return Ok(dto);
        }

        /// <summary>Создаёт нового клиента и назначает предпочтения.</summary>
        [HttpPost]
        public async Task<IActionResult> CreateCustomerAsync(CreateOrEditCustomerRequest request)
        {
            var prefIds = (request.PreferenceIds ?? Enumerable.Empty<Guid>()).Distinct().ToList();

            var customer = new Customer
            {
                Id = Guid.NewGuid(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Preferences = prefIds.Select(pid => new CustomerPreference
                {
                    CustomerId = Guid.Empty,
                    PreferenceId = pid
                }).ToList()
            };

            foreach (var cp in customer.Preferences)
                cp.CustomerId = customer.Id;

            await customerRepository.AddAsync(customer);
            return CreatedAtRoute("GetCustomerById", new { id = customer.Id }, null);
        }

        /// <summary>Обновляет данные клиента и его предпочтения.</summary>
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> EditCustomersAsync(Guid id, CreateOrEditCustomerRequest request)
        {
            var existing = await customerRepository.GetByIdAsync(id);
            if (existing == null) return NotFound();

            existing.FirstName = request.FirstName;
            existing.LastName = request.LastName;
            existing.Email = request.Email;

            var prefIds = (request.PreferenceIds ?? Enumerable.Empty<Guid>()).Distinct().ToList();
            existing.Preferences = prefIds.Select(pid => new CustomerPreference
            {
                CustomerId = id,
                PreferenceId = pid
            }).ToList();

            await customerRepository.UpdateAsync(existing);
            return NoContent();
        }

        /// <summary>Удаляет клиента и связанные с ним промокоды.</summary>
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            var customer = await customerRepository.GetByIdAsync(id);
            if (customer == null) return NotFound();

            var allPromocodes = await promocodesRepository.GetAllAsync();
            var toDelete = allPromocodes.Where(p => p.CustomerId == id).Select(p => p.Id).ToList();
            if (toDelete.Count > 0)
                await promocodesRepository.DeleteRangeAsync(toDelete);

            await customerRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}