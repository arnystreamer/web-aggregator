import { CityDataItemPopulated } from "./city-data-item-populated.model";
import { CitySalary } from "./city-salary.model";
import { RegionTax } from "./region-tax.model";

export interface CityAggregated {
    id: string;

    name: string;
    region: string;
    country: string;

    dataItems: CityDataItemPopulated[];
    salaries: CitySalary | undefined;
    applicableTaxes: RegionTax[];

    hasFreeApartment: boolean;
    personalAll: number;
    personalWithMortgageAndChildcare: number;
    personalWithoutChildcare: number;

    averageExpatSalaryNet: number;
    apartmentFirstPayment: number;
    mortgageMonthlyPayment: number;
    averageApartmentsPrice: number;
    averageRentPrice: number;
    averageOrdinalCarPrice: number;

    isShown: boolean;

    chosenSalaryNetMonthly?: number;

    sustainableSalaryNet?: number; //1500 per month savings
    sustainableSalaryGross?: number;

    millionaire30YSalaryNet?: number;
    millionaire30YSalaryGross?: number;

    averageExpatSalaryGrossYearly?: number;
    p25SalaryNet?: number;
    p75SalaryNet?: number;
    millionaireTerm?: number;    //in months
    millionaireEasyTerm?: number;    //in months
    buyCarTerm?: number;  //in months
    apartmentFirstPaymentTerm?: number; //save 20% of apartment

}


