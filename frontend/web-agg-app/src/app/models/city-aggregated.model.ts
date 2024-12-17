import { CityDataItemPopulated } from "./city-data-item-populated.model";
import { CitySalary } from "./city-salary.model";
import { RegionTax } from "./region-tax.model";

export interface CityAggregated {
    id: string;

    name: string;
    region: string;
    country: string;

    dataItems: CityDataItemPopulated[];
    salaries: CitySalary;
    applicableTaxes: RegionTax[];

    personalAll: number;
    personalWithMortgageAndChildcare: number;
    personalWithoutChildcare: number;

    sustainableSalaryNet?: number; //1500 per month savings

    averageExpatSalaryNet: number;
    apartmentFirstPayment: number;
    mortgageMonthlyPayment: number;
    averageApartmentsPrice: number;
    averageRentPrice: number;

    averageExpatSalaryGrossYearly?: number;
    sustainableSalaryGross?: number;
    p75SalaryNet?: number;
    millionaireTerm?: number;    //in months
    apartmentFirstPaymentTerm?: number; //save 20% of apartment

}
