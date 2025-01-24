using System.ComponentModel.DataAnnotations;
public sealed class CheckPrice : ValidationAttribute
{
    public string BuyPriceVal { get; set; }
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var BuyPriceProp = validationContext.ObjectType.GetProperty(BuyPriceVal);
        var BuyPrice = BuyPriceProp.GetValue(validationContext.ObjectInstance, null);

        if (value != null)
        {
            if (BuyPrice == null || (((decimal)value) >= ((decimal)BuyPrice)))
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Sales Price must be greater than or equal to Buy Price.");
            }
        }
        else
        {
            return new ValidationResult("Sales Price is required.");
        }
    }
}
