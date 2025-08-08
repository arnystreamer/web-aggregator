using Jimx.Common.WebApi.Models;

namespace Jimx.WebAggregator.API.Models;

public static class CollectionRequestApiHelper
{
    public static (int Skip, int Take) ToSkipTake(this CollectionRequestApi collectionRequestApi)
    {
        return (collectionRequestApi.Skip ?? 0, collectionRequestApi.Take ?? 10);
    }
}