namespace PromoCodeFactory.WebHost.Models;

public class CreateEmployeeRequest
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Email { get; set; }

    public int AppliedPromocodesCount { get; set; }
}