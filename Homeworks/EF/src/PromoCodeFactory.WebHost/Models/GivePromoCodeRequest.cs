using System;
using System.ComponentModel.DataAnnotations;

namespace PromoCodeFactory.WebHost.Models
{
    public class GivePromoCodeRequest
    {
        [Required]
        public Guid PreferenceId { get; set; }

        [Required, MaxLength(256)]
        public string ServiceInfo { get; set; } = default!;

        [Required, MaxLength(128)]
        public string PartnerName { get; set; } = default!;

        [Required, MaxLength(64)]
        public string PromoCode { get; set; } = default!;
    }
}