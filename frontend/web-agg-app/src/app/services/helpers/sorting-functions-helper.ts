import { CityAggregated } from "../../models/city-aggregated.model";

function genericSortingFunction(ascending: boolean, selector: (c: CityAggregated) => number)
{
  return (a: CityAggregated, b: CityAggregated) =>
    (selector(a) - selector(b)) * (ascending ? 1 : -1)
}

function genericSortingTermFunction(ascending: boolean, termSelector: (c: CityAggregated) => number)
{
  const selector = (c: CityAggregated) => termSelector(c) > 0 ? termSelector(c) : 10000000;

  return (a: CityAggregated, b: CityAggregated) =>
    (selector(a) - selector(b)) * (ascending ? 1 : -1)
}

function genericSortingProximityAbsoluteFunction(ascending: boolean, selector: (c: CityAggregated) => number)
{
  return (a: CityAggregated, b: CityAggregated) =>
    (selector(a) - a.chosenSalaryNetMonthly! - (selector(b) - b.chosenSalaryNetMonthly!)) * (ascending ? 1 : -1)
}

function genericSortingProximityRelativeFunction(ascending: boolean, selector: (c: CityAggregated) => number)
{
  return (a: CityAggregated, b: CityAggregated) => {

      if (a.chosenSalaryNetMonthly! === 0 && b.chosenSalaryNetMonthly! === 0)
        return 0;

      if (a.chosenSalaryNetMonthly === 0)
        return 1;

      if (b.chosenSalaryNetMonthly === 0)
        return -1;

      return ((selector(a) - a.chosenSalaryNetMonthly!) * 100.0 / a.chosenSalaryNetMonthly! -
        (selector(b) - b.chosenSalaryNetMonthly!)  * 100.0 / b.chosenSalaryNetMonthly!) * (ascending ? 1 : -1)
    }
}

export const SortingFunctionsHelper =
{
  chosenSalaryAbsoulteValue: (ascending: boolean) =>
    genericSortingFunction(ascending, c => c.chosenSalaryNetMonthly!),

  expensesAsPairAbsolute: (ascending: boolean) =>
    genericSortingFunction(ascending, c => c.personalWithoutChildcare!),
  expensesAsPairProximity: (ascending: boolean) =>
    genericSortingProximityAbsoluteFunction(ascending, c => c.personalWithoutChildcare!),
  expensesAsPairProximityRelative: (ascending: boolean) =>
    genericSortingProximityRelativeFunction(ascending, c => c.personalWithoutChildcare!),

  expensesWithChildAbsolute: (ascending: boolean) =>
    genericSortingFunction(ascending, c => c.personalAll!),
  expensesWithChildProximity: (ascending: boolean) =>
    genericSortingProximityAbsoluteFunction(ascending, c => c.personalAll!),
  expensesWithChildProximityRelative: (ascending: boolean) =>
    genericSortingProximityRelativeFunction(ascending, c => c.personalAll!),

  expensesWithMortgageAndChildcareAbsolute: (ascending: boolean) =>
    genericSortingFunction(ascending, c => c.personalWithMortgageAndChildcare!),
  expensesWithMortgageAndChildcareProximity: (ascending: boolean) =>
    genericSortingProximityAbsoluteFunction(ascending, c => c.personalWithMortgageAndChildcare!),
  expensesWithMortgageAndChildcareProximityRelative: (ascending: boolean) =>
    genericSortingProximityRelativeFunction(ascending, c => c.personalWithMortgageAndChildcare!),

  sustainableSalaryNetAbsolute: (ascending: boolean) =>
    genericSortingFunction(ascending, c => c.sustainableSalaryNet!),
  sustainableSalaryNetProximity: (ascending: boolean) =>
    genericSortingProximityAbsoluteFunction(ascending, c => c.sustainableSalaryNet!),
  sustainableSalaryNetProximityRelative: (ascending: boolean) =>
    genericSortingProximityRelativeFunction(ascending, c => c.sustainableSalaryNet!),

  sustainableSalaryGrossAbsolute: (ascending: boolean) =>
    genericSortingFunction(ascending, c => c.sustainableSalaryGross!),

  millionaire30YSalaryNetAbsolute: (ascending: boolean) =>
    genericSortingFunction(ascending, c => c.millionaire30YSalaryNet!),
  millionaire30YSalaryNetProximity: (ascending: boolean) =>
    genericSortingProximityAbsoluteFunction(ascending, c => c.millionaire30YSalaryNet!),
  millionaire30YSalaryNetProximityRelative: (ascending: boolean) =>
    genericSortingProximityRelativeFunction(ascending, c => c.millionaire30YSalaryNet!),

  millionaire30YSalaryGrossAbsolute: (ascending: boolean) =>
    genericSortingFunction(ascending, c => c.sustainableSalaryGross!),

  millionaireTerm: (ascending: boolean) =>
    genericSortingTermFunction(ascending, c => c.millionaireTerm!),
  millionaireEasyTerm: (ascending: boolean) =>
    genericSortingTermFunction(ascending, c => c.millionaireEasyTerm!),
  buyCarTerm: (ascending: boolean) =>
    genericSortingTermFunction(ascending, c => c.buyCarTerm!),
  apartmentFirstPaymentTerm: (ascending: boolean) =>
    genericSortingTermFunction(ascending, c => c.apartmentFirstPayment!),

}
