using System;
using System.Collections.Generic;

namespace PromoCodeFactory.WebHost.Models
{
    public class CustomerResponse
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = default!;
        public string LastName  { get; set; } = default!;
        public string Email     { get; set; } = default!;
        public string? Nickname { get; set; }
        
        public List<PreferenceResponse> Preference { get; set; }
        
        public List<PromoCodeShortResponse> PromoCodes { get; set; }
    }
}