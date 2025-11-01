namespace BlazorApp.Application.Cache;

internal static class CacheRoutingKeys
{
    internal const string CustomerRoutingKey = "customer:{0}";
    internal const string CustomerPageRoutingKey = "customer:page:{0}";

    internal static string GetCustomerRoutingKey(int customerId)
    {
        return string.Format(CustomerRoutingKey, customerId.ToString());
    }

    internal static string GetCustomersPageRoutingKey(int pageNumber)
    {
        return string.Format(CustomerPageRoutingKey, pageNumber.ToString());
    }
}
