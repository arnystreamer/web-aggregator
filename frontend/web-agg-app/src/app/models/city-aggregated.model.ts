import { CityDataItemPopulated } from "./city-data-item-populated.model";

export interface CityAggregated {
    id: string;

    name: string;
    region: string;
    country: string;

    dataItems: CityDataItemPopulated[];

    personalAll: number;
    personalWithMortgageAndChildcare: number;
    personalWithoutChildcare: number;

    sustainableSalary?: number; //1500 per month savings

    averageExpatSalary: number;
    apartmentFirstPayment: number;
    mortgageMonthlyPayment: number;
    averageApartmentsPrice: number;
    averageRentPrice: number;

    millionaireTerm?: number;    //in months
    apartmentFirstPaymentTerm?: number; //save 20% of apartment

}
