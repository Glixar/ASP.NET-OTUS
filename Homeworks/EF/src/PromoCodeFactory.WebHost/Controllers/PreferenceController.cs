using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.WebHost.Models;

namespace PromoCodeFactory.WebHost.Controllers;

/// <summary>
/// Справочник предпочтений клиентов.
/// </summary>
[Route("api/v1/[controller]")]
[ApiController]
public class PreferenceController : ControllerBase
{
    private readonly IRepository<Preference> preferenceRepository;

    public PreferenceController(IRepository<Preference> preferenceRepository)
    {
        this.preferenceRepository = preferenceRepository;
    }
    
    /// <summary>
    /// Возвращает список доступных предпочтений.
    /// </summary>
    /// <returns>Список предпочтений.</returns>
    /// <response code="200">Список успешно получен.</response>
    [HttpGet]
    public async Task<IEnumerable<PreferenceResponse>> GetPreferenceAsync()
    {
        var preference = await preferenceRepository.GetAllAsync();
        return preference.Select(x => new PreferenceResponse
        {
            Id = x.Id,
            Name = x.Name
        });
    }
}