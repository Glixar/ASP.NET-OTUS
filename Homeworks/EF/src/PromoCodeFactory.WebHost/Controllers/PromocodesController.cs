using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.WebHost.Models;

namespace PromoCodeFactory.WebHost.Controllers
{
    /// <summary>
    /// Промокоды
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PromocodesController : ControllerBase
    {
        private readonly IRepository<PromoCode> _promocodesRepository;
        private readonly IRepository<Preference> _preferenceRepository;
        private readonly IRepository<Customer> _customerRepository;

        public PromocodesController(
            IRepository<PromoCode> promocodesRepository,
            IRepository<Preference> preferenceRepository,
            IRepository<Customer> customerRepository)
        {
            _promocodesRepository = promocodesRepository;
            _preferenceRepository = preferenceRepository;
            _customerRepository = customerRepository;
        }

        /// <summary>Получить все промокоды</summary>
        [HttpGet]
        public async Task<ActionResult<List<PromoCodeShortResponse>>> GetPromocodesAsync()
        {
            var promocodes = await _promocodesRepository.GetAllAsync();

            var list = promocodes.Select(p => new PromoCodeShortResponse
            {
                Id = p.Id,
                Code = p.Code,
                ServiceInfo = p.ServiceInfo,
                BeginDate = p.BeginDate.ToString("yyyy.MM.dd"),
                EndDate = p.EndDate.ToString("yyyy.MM.dd"),
                PartnerName = p.PartnerName
            }).ToList();

            return list;
        }

        /// <summary>
        /// Создать промокод и выдать его всем клиентам с указанным предпочтением
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> GivePromoCodesToCustomersWithPreferenceAsync([FromBody] GivePromoCodeRequest request)
        {
            if (request is null) return BadRequest("Body is required.");

            var preference = await _preferenceRepository.GetByIdAsync(request.PreferenceId);
            if (preference is null)
                return NotFound($"PreferenceId '{request.PreferenceId}' не найден.");

            var customers = await _customerRepository.GetAllAsync();

            var targetCustomers = customers
                .Where(c => c.Preferences != null && c.Preferences.Any(cp => cp.PreferenceId == preference.Id))
                .ToList();

            if (targetCustomers.Count == 0)
                return NotFound($"Нет пользователей с предпочтением '{preference.Name}'.");

            foreach (var c in targetCustomers)
            {
                var promo = new PromoCode
                {
                    Id = Guid.NewGuid(),
                    ServiceInfo = request.ServiceInfo,
                    PartnerName = request.PartnerName,
                    Code = request.PromoCode,
                    CustomerId = c.Id,
                    PreferenceId = preference.Id,
                    BeginDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow.AddDays(14)
                };

                await _promocodesRepository.AddAsync(promo);
            }

            return Ok(new { issued = targetCustomers.Count, preference = preference.Name });
        }
    }
}