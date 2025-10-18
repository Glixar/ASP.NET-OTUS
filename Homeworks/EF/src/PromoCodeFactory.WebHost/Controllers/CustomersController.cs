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

            var customerModelList = customers.Select(c => new CustomerShortResponse
            {
                Id = c.Id,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Email = c.Email,
                Nickname = c.Nickname
            }).ToList();

            return customerModelList;
        }

        /// <summary>Возвращает клиента по идентификатору вместе с его промокодами.</summary>
        [HttpGet("{id:guid}", Name = "GetCustomerById")]
        public async Task<ActionResult<CustomerResponse>> GetCustomerAsync(Guid id)
        {
            var customer = await customerRepository.GetByIdAsync(id);
            if (customer == null)
                return NotFound();

            var promoCodes = customer.PromoCodes;

            var customerModel = new CustomerResponse
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                Nickname = customer.Nickname,
                PromoCodes = promoCodes?.Select(p => new PromoCodeShortResponse
                {
                    Id = p.Id,
                    Code = p.Code,
                    ServiceInfo = p.ServiceInfo,
                    BeginDate = p.BeginDate.ToString("yyyy.MM.dd"),
                    EndDate = p.EndDate.ToString("yyyy.MM.dd"),
                    PartnerName = p.PartnerName
                }).ToList()
            };

            return Ok(customerModel);
        }

        /// <summary>Создаёт нового клиента и назначает предпочтения.</summary>
        [HttpPost]
        public async Task<IActionResult> CreateCustomerAsync(CreateOrEditCustomerRequest request)
        {
            var preferences = await preferenceRepository.GetRangeByIdsAsync(request.PreferenceIds);

            var customer = new Customer
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Nickname = request.Nickname
            };

            customer.Preferences = preferences.Select(p => new CustomerPreference
            {
                Customer = customer,
                Preference = p
            }).ToList();

            await customerRepository.AddAsync(customer);

            return CreatedAtRoute("GetCustomerById", new { id = customer.Id }, null);
        }

        /// <summary>Обновляет данные клиента и его предпочтения.</summary>
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> EditCustomersAsync(Guid id, CreateOrEditCustomerRequest request)
        {
            var customer = await customerRepository.GetByIdAsync(id);
            if (customer == null)
                return NotFound();

            var preferences = await preferenceRepository.GetRangeByIdsAsync(request.PreferenceIds);

            customer.FirstName = request.FirstName;
            customer.LastName = request.LastName;
            customer.Email = request.Email;
            customer.Nickname = request.Nickname;

            customer.Preferences = preferences.Select(p => new CustomerPreference
            {
                Customer = customer,
                Preference = p
            }).ToList();

            await customerRepository.UpdateAsync(customer);
            return NoContent();
        }

        /// <summary>Удаляет клиента и связанные с ним промокоды.</summary>
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            var customer = await customerRepository.GetByIdAsync(id);
            if (customer == null)
                return NotFound();

            var promocodes = customer.PromoCodes;
            if (promocodes != null && promocodes.Any())
                await promocodesRepository.DeleteRangeAsync(promocodes.Select(p => p.Id));

            await customerRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
