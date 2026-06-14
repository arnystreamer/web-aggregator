namespace Jimx.WebAggregator.Domain.CityCosts;

public record CityDataTimeStamp(int Year, int Month) : IComparable<CityDataTimeStamp>
{
    public int CompareTo(CityDataTimeStamp? other)
    {
        if (other == null)
        {
            return int.MaxValue;
        }
        
        return (Year - other.Year) * 1000 + Month - other.Month; 
    }
}