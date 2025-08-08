namespace Jimx.WebAggregator.Calculations;

public class TaxationService
{
    public UnitOfTaxCalculation GetCalculation(UserTaxProfile userTaxProfile)
    {
        return new UnitOfTaxCalculation(userTaxProfile);
    }
}