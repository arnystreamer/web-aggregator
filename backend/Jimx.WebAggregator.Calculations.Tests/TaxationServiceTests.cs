using Jimx.WebAggregator.Calculations.Helpers;
using Jimx.WebAggregator.Domain.CityCosts;

namespace Jimx.WebAggregator.Calculations.Tests;

public class TaxationServiceTests
{
    private readonly TaxationService _taxationService = new();
    
    private readonly Func<string[], IncomeTaxItem> _incomeTaxFactory =
        tags =>
            new IncomeTaxItem("Some_tax", "Tax", tags, "gross", true, 335, null, null, null, null, "", []);

    private readonly UserFamily _userFamily = new()
    {
        FamilyMembersCount = 3,
        ToddlersCount = 1,
        PrescholarsCount = 0,
        ScholarsCount = 0
    };

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void IsTaxApplicable_MarginalValues_Test()
    {
        var taxUser = new UserTaxProfile([
            "taxpayer_status:single",
            "taxpayer_age:35",
            "taxpayer_residency:luzern"
        ], _userFamily);
        
        var disabledIncomeTax = new IncomeTaxItem("Some_tax", "Tax", [], "gross", false, 335, null, null, null, null, "", []);
        Assert.That(
            TaxFunctions.IsTaxApplicable(taxUser, disabledIncomeTax),
            Is.False);
        
        var enabledNoTagsIncomeTax = new IncomeTaxItem("Some_tax", "Tax", [], "gross", true, null, 1300, null, null, null, "comment", []);
        Assert.That(
            TaxFunctions.IsTaxApplicable(new UserTaxProfile([], _userFamily), enabledNoTagsIncomeTax),
            Is.True);
        
        var enabledAllDefaultTagsIncomeTax = new IncomeTaxItem("Some_tax", "Tax", [
            "taxpayer_status:default",
            "taxpayer_residency:default"
        ], "gross", true, 210, null, null, null, null, "", []);
        Assert.That(
            TaxFunctions.IsTaxApplicable(new UserTaxProfile([], _userFamily), enabledAllDefaultTagsIncomeTax),
            Is.True);
        
        var enabledMultiValueTagsIncomeTax = new IncomeTaxItem("Some_tax", "Tax", [
            "taxpayer_status:married,default",
            "taxpayer_residency:zurich,default"
        ], "gross", true, null, 4200, null, null, null, "", []);
        Assert.That(
            TaxFunctions.IsTaxApplicable(new UserTaxProfile([], _userFamily), enabledMultiValueTagsIncomeTax),
            Is.True);
        
        Assert.Pass();
    }

    [Test]
    public void IsTaxApplicable_StatusValues_Test()
    {
        var singleTaxUser = new UserTaxProfile([
            "taxpayer_status:single",
            "taxpayer_age:35",
            "taxpayer_residency:luzern"
        ], _userFamily);
        
        var marriedTaxUser = new UserTaxProfile([
            "taxpayer_status:married",
            "taxpayer_age:42",
            "taxpayer_residency:zurich"
        ], _userFamily);
        
        var noStatusTaxUser = new UserTaxProfile([
            "taxpayer_age:35",
            "taxpayer_residency:zug"
        ], _userFamily);

        var marriedTax = _incomeTaxFactory(["taxpayer_status:married"]);
        
        Assert.That(TaxFunctions.IsTaxApplicable(singleTaxUser, marriedTax), Is.False);
        Assert.That(TaxFunctions.IsTaxApplicable(marriedTaxUser, marriedTax), Is.True);
        Assert.That(TaxFunctions.IsTaxApplicable(noStatusTaxUser, marriedTax), Is.False);

        var marriedOrDefaultTax = _incomeTaxFactory(["taxpayer_status:married,default"]);
        
        Assert.That(TaxFunctions.IsTaxApplicable(singleTaxUser, marriedOrDefaultTax), Is.False);
        Assert.That(TaxFunctions.IsTaxApplicable(marriedTaxUser, marriedOrDefaultTax), Is.True);
        Assert.That(TaxFunctions.IsTaxApplicable(noStatusTaxUser, marriedOrDefaultTax), Is.True);

        var singleTax = _incomeTaxFactory(["taxpayer_status:single"]);
        
        Assert.That(TaxFunctions.IsTaxApplicable(singleTaxUser, singleTax), Is.True);
        Assert.That(TaxFunctions.IsTaxApplicable(marriedTaxUser, singleTax), Is.False);
        Assert.That(TaxFunctions.IsTaxApplicable(noStatusTaxUser, singleTax), Is.False);
        
        Assert.Pass();
    }

    [Test]
    public void IsTaxApplicable_AgeValues_Test()
    {
        var age17TaxUser = new UserTaxProfile([
            "taxpayer_status:single",
            "taxpayer_age:17",
            "taxpayer_residency:luzern"
        ], _userFamily);
        
        var age40TaxUser = new UserTaxProfile([
            "taxpayer_status:single",
            "taxpayer_age:40",
            "taxpayer_residency:zug"
        ], _userFamily);
        
        var noAgeTaxUser = new UserTaxProfile([
            "taxpayer_status:single",
            "taxpayer_residency:luzern"
        ], _userFamily);

        var ageRangeTax = _incomeTaxFactory(["taxpayer_age:15-32"]);
                
        Assert.That(TaxFunctions.IsTaxApplicable(age17TaxUser, ageRangeTax), Is.True);
        Assert.That(TaxFunctions.IsTaxApplicable(age40TaxUser, ageRangeTax), Is.False);
        Assert.That(TaxFunctions.IsTaxApplicable(noAgeTaxUser, ageRangeTax), Is.False);
        
        var ageRangeOrDefaultTax = _incomeTaxFactory(["taxpayer_age:15-32,default"]);
                
        Assert.That(TaxFunctions.IsTaxApplicable(age17TaxUser, ageRangeOrDefaultTax), Is.True);
        Assert.That(TaxFunctions.IsTaxApplicable(age40TaxUser, ageRangeOrDefaultTax), Is.False);
        Assert.That(TaxFunctions.IsTaxApplicable(noAgeTaxUser, ageRangeOrDefaultTax), Is.True);

        Assert.Pass();
    }

    [Test]
    public void IsTaxApplicable_ResidencyValues_Test()
    {
        var luzernTaxUser = new UserTaxProfile([
            "taxpayer_status:single",
            "taxpayer_age:35",
            "taxpayer_residency:luzern"
        ], _userFamily);
        
        var zurichTaxUser = new UserTaxProfile([
            "taxpayer_status:single",
            "taxpayer_age:42",
            "taxpayer_residency:zurich"
        ], _userFamily);
        
        var noResidencyTaxUser = new UserTaxProfile([
            "taxpayer_status:single"
        ], _userFamily);

        var zurichTax = _incomeTaxFactory(["taxpayer_residency:zurich"]);
        
        Assert.That(TaxFunctions.IsTaxApplicable(luzernTaxUser, zurichTax), Is.False);
        Assert.That(TaxFunctions.IsTaxApplicable(zurichTaxUser, zurichTax), Is.True);
        Assert.That(TaxFunctions.IsTaxApplicable(noResidencyTaxUser, zurichTax), Is.False);

        var zurichOrDefaultTax = _incomeTaxFactory(["taxpayer_residency:zurich,default"]);

        Assert.That(TaxFunctions.IsTaxApplicable(luzernTaxUser, zurichOrDefaultTax), Is.False);
        Assert.That(TaxFunctions.IsTaxApplicable(zurichTaxUser, zurichOrDefaultTax), Is.True);
        Assert.That(TaxFunctions.IsTaxApplicable(noResidencyTaxUser, zurichOrDefaultTax), Is.True);
        
        var luzernTax = _incomeTaxFactory(["taxpayer_residency:luzern"]);
        
        Assert.That(TaxFunctions.IsTaxApplicable(luzernTaxUser, luzernTax), Is.True);
        Assert.That(TaxFunctions.IsTaxApplicable(zurichTaxUser, luzernTax), Is.False);
        Assert.That(TaxFunctions.IsTaxApplicable(noResidencyTaxUser, luzernTax), Is.False);
        
        Assert.Pass();
    }

    [Test]
    public void IsTaxApplicable_Year_Test()
    {   
        var noTagsUser = new UserTaxProfile([], _userFamily);
        
        var taxUser = new UserTaxProfile([
            "taxpayer_age:35"
        ], _userFamily);
        
        var noYearTax = _incomeTaxFactory([]);
        var currentYearTax = _incomeTaxFactory(["year:2025"]);
        var outdatedTax = _incomeTaxFactory(["year:2024"]);
        
        Assert.That(TaxFunctions.IsTaxApplicable(noTagsUser, noYearTax), Is.True);
        Assert.That(TaxFunctions.IsTaxApplicable(taxUser, noYearTax), Is.True);
        Assert.That(TaxFunctions.IsTaxApplicable(noTagsUser, currentYearTax), Is.True);
        Assert.That(TaxFunctions.IsTaxApplicable(taxUser, currentYearTax), Is.True);
        Assert.That(TaxFunctions.IsTaxApplicable(noTagsUser, outdatedTax), Is.False);
        Assert.That(TaxFunctions.IsTaxApplicable(taxUser, outdatedTax), Is.False);

    }

    [Test]
    public void GetMortgageMonthlyPayment_Test()
    {
        var monthlyPayment = TaxFunctions.GetMortgageMonthlyPayment(6.732, 30, 320000);
        Assert.That(monthlyPayment, Is.InRange(2071.0, 2072.0));
        
        monthlyPayment = TaxFunctions.GetMortgageMonthlyPayment(5.5, 20, 800000);
        Assert.That(monthlyPayment, Is.InRange(5503.0, 5504.0));
        
        monthlyPayment = TaxFunctions.GetMortgageMonthlyPayment(5.5, 20, 0);
        Assert.That(monthlyPayment, Is.Zero);
        
        Assert.Pass();
    }

    [Test]
    public void ApplyTaxes_Simple_Test()
    {
        var finlandTaxes = new RegionTaxDeduction(null, "Finland", "FI", "EUR",
            new RegionTaxDeductionSource(null, null, null, null, null),
            [
                new IncomeTaxItem("National", "National income tax", [ "year:2025" ],
                    null, true, null, null, null, null, null, null,
                    [
                        new IncomeTaxLevel(0, 21200, 0.1264m, null, null),
                        new IncomeTaxLevel(21200, 31500, 0.19m, null, null),
                        new IncomeTaxLevel(31500, 52100, 0.3025m, null, null),
                        new IncomeTaxLevel(52100, 88200, 0.34m, null, null),
                        new IncomeTaxLevel(88200, 150000, 0.4125m, null, null),
                        new IncomeTaxLevel(150000, null, 0.4425m, null, null)
                    ]),
                new IncomeTaxItem("Municipal", "Helsinki City municipal income tax",
                    ["taxpayer_residency:helsinki,default"],
                    null, true, null, null, 0.053m, null, null, null, []),
                new IncomeTaxItem(null, "Pension insurance", [ "taxpayer_age:17-52" ],
                    null, true, null, null, 0.0715m, null, null, null, []),
                new IncomeTaxItem(null, "Medicare", [],
                    null, true, null, null, 0.0106m, null, null, null, []),
                new IncomeTaxItem(null, "Daily Allowance contribution", [],
                    "gross", true, null, null, 0.0084m, null, null, null, []),
                new IncomeTaxItem(null, "Unemployment insurance", [],
                    null, true, null, null, 0.0059m, null, null, "some comment", []),
                new IncomeTaxItem(null, "Public broadcasting tax", [],
                    "housing", true, null, 160.0m, null, null, null, null, [])
            ]);

        var netSalary = _taxationService.GetCalculation(new UserTaxProfile([], _userFamily))
            .WithTaxes([finlandTaxes]).ApplyTaxes(60000);
        Assert.That(netSalary, Is.InRange(41611.0m, 41612.0m));
        
        netSalary = _taxationService.GetCalculation(new UserTaxProfile([ "taxpayer_age:40" ], _userFamily))
            .WithTaxes([finlandTaxes]).ApplyTaxes(60000);
        Assert.That(netSalary, Is.InRange(37321.0m, 37322.0m));
        
        netSalary = _taxationService.GetCalculation(new UserTaxProfile([ "taxpayer_residency:oulu", "taxpayer_age:40" ], _userFamily))
            .WithTaxes([finlandTaxes]).ApplyTaxes(60000);
        Assert.That(netSalary, Is.InRange(40501.0m, 40502.0m));
        
        Assert.Pass();
    }

    [Test]
    public void ApplyTaxes_Multiple_Test()
    {
        var switzerlandFederalTaxes = new RegionTaxDeduction(null, "Switzerland", "CH", "CHF",
            new RegionTaxDeductionSource(null, null, null, null, null),
            [
                new IncomeTaxItem("Federal", "Federal income tax", ["taxpayer_status:married,default", "year:2025"],
                    null, true, null, null, null, null, null, null,
                    [
                        new IncomeTaxLevel(0, 32000, 0.0m, null, null),
                        new IncomeTaxLevel(32000, 53400, 0.01m, null, null),
                        new IncomeTaxLevel(53400, 61300, 0.02m, null, null),
                        new IncomeTaxLevel(61300, 79100, 0.03m, null, null),
                        new IncomeTaxLevel(79100, 94900, 0.04m, null, null),
                        new IncomeTaxLevel(94900, 108600, 0.05m, null, null),
                        new IncomeTaxLevel(108600, 120500, 0.06m, null, null),
                        new IncomeTaxLevel(120500, 130500, 0.07m, null, null),
                        new IncomeTaxLevel(130500, 138300, 0.08m, null, null),
                        new IncomeTaxLevel(138300, 144200, 0.09m, null, null),
                        new IncomeTaxLevel(144200, 148200, 0.10m, null, null),
                        new IncomeTaxLevel(148200, 150300, 0.11m, null, null),
                        new IncomeTaxLevel(150300, 152300, 0.12m, null, null),
                        new IncomeTaxLevel(152300, 940800, 0.13m, null, null),
                        new IncomeTaxLevel(940800, null, 0.115m, null, null)
                    ]),
                new IncomeTaxItem(null, "Old Age and Survivors Insurance (AHV)", [],
                    null, true, null, null, 0.0435m, null, null, null, []),
                new IncomeTaxItem(null, "Disability Insurance (IV)", [],
                    null, true, null, null, 0.007m, null, null, null, []),
                new IncomeTaxItem(null, "Income Compensation Insurance (EO)", [],
                    null, true, null, null, 0.0025m, null, null, null, []),
                new IncomeTaxItem(null, "Unemployment Insurance (ALV)", [],
                    "gross", true, null, null, null, null, null, null, [
                        new IncomeTaxLevel(0, 148200, 0.011m, null, null)
                    ]),
                new IncomeTaxItem(null, "Occupational Pension (BVG - 2nd Pillar)",
                    ["taxtype:pension", "taxpayer_age:35-44"],
                    null, true, null, null, null, null, null, "some comment", [
                        new IncomeTaxLevel(29400, 88200, 0.10m, null, null)
                    ]),
                new IncomeTaxItem(null, "Compulsory health Insurance", [],
                    "housing", true, 400.0m, null, null, null, null, null, []),
                new IncomeTaxItem(null, "Radio and Television License Fee", ["taxtype:household"],
                    "housing", true, null, 335.0m, null, null, null, null, [])
            ]);
        
        var luzernCantonalTaxes = new RegionTaxDeduction("Lucerne", "Switzerland", "CH-LU", "CHF",
            new RegionTaxDeductionSource(null, null, null, null, null),
            [
                new IncomeTaxItem("Cantonal", "Luzern cantonal income tax", ["taxpayer_status:married,default", "year:2025"],
                    null, true, null, null, null, 1.55m, null, null,
                    [
                        new IncomeTaxLevel(0, 19900, 0.0m, null, null),
                        new IncomeTaxLevel(19900, 24000, 0.005m, null, null),
                        new IncomeTaxLevel(24000, 25000, 0.015m, null, null),
                        new IncomeTaxLevel(25000, 26200, 0.025m, null, null),
                        new IncomeTaxLevel(26200, 28300, 0.03m, null, null),
                        new IncomeTaxLevel(28300, 32500, 0.035m, null, null),
                        new IncomeTaxLevel(32500, 99000, 0.045m, null, null),
                        new IncomeTaxLevel(99000, 137900, 0.05m, null, null),
                        new IncomeTaxLevel(137900, 159000, 0.055m, null, null),
                        new IncomeTaxLevel(159000, 1424300, 0.058m, null, null),
                        new IncomeTaxLevel(1424300, null, 0.056m, null, null)
                    ]),
                new IncomeTaxItem("Municipal", "Luzern municipal income tax", ["taxpayer_residency:luzern,default", "year:2025"],
                    "tax(Cantonal)", true, null, null, 1.55m, null, null, null, [])
                
            ]);
        
        var netSalary = _taxationService.GetCalculation(new UserTaxProfile(["taxpayer_age:36"], _userFamily))
            .WithTaxes([switzerlandFederalTaxes, luzernCantonalTaxes]).ApplyTaxes(100000);
        Assert.That(netSalary, Is.InRange(70506.0m, 70507.0m));
        
        netSalary = _taxationService.GetCalculation(new UserTaxProfile(["taxpayer_age:36", "taxpayer_residency:littau"], _userFamily))
            .WithTaxes([switzerlandFederalTaxes, luzernCantonalTaxes]).ApplyTaxes(100000);
        Assert.That(netSalary, Is.InRange(75649.0m, 75650.0m));
        
        netSalary = _taxationService.GetCalculation(new UserTaxProfile(["taxpayer_residency:littau"], _userFamily))
            .WithTaxes([switzerlandFederalTaxes, luzernCantonalTaxes]).ApplyTaxes(100000);
        Assert.That(netSalary, Is.InRange(81529.0m, 81530.0m));
        
        Assert.Pass();
    }

    [Test]
    public void ApplyTaxes_VariableRate_Test()
    {
        var germanyTaxes = new RegionTaxDeduction(null, "Germany", "DE", "EUR",
            new RegionTaxDeductionSource(null, null, null, null, null),
            [
                new IncomeTaxItem("Federal", "Federal income tax", ["taxpayer_status:married,default", "year:2025"],
                    null, true, null, null, null, null, null, null,
                    [
                        new IncomeTaxLevel(0, 23208, 0.0m, null, null),
                        new IncomeTaxLevel(23208, 34010, null, 0.14m, 0.24m),
                        new IncomeTaxLevel(34010, 133520, null, 0.24m, 0.42m),
                        new IncomeTaxLevel(133520, 555650, 0.42m, null, null),
                        new IncomeTaxLevel(555650, null, 0.45m, null, null)
                    ]),
                new IncomeTaxItem(null, "Solidaritatszuschlag", ["taxpayer_status:married,default", "year:2025"],
                    "tax(Federal)", true, null, null, null, null, null, null,
                    [
                        new IncomeTaxLevel(0, 39900, 0.0m, null, null),
                        new IncomeTaxLevel(39900, 55961, null, 0.0m, 0.055m),
                        new IncomeTaxLevel(55961, null, 0.055m, null, null)
                    ]),
                new IncomeTaxItem(null, "Fake Pension Insurance Tax", [],
                    null, true, null, null, null, null, 11000, null,
                    [
                        new IncomeTaxLevel(0, 84600, 0.093m, null, null)
                    ])
            ]);
        
        var netSalary = _taxationService.GetCalculation(new UserTaxProfile(["taxpayer_age:36"], _userFamily))
            .WithTaxes([germanyTaxes]).ApplyTaxes(100000);
        Assert.That(netSalary, Is.InRange(70917.0m, 70918.0m)); //calculate this value by hand
    }
}