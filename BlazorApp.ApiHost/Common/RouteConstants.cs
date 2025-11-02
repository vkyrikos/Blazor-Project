namespace BlazorApp.ApiHost.Common;

internal static class RouteConstants
{
	internal static class V1
	{
		internal const string CustomerController = "api/customer";
		internal const string Heartbeat = "heartbeat";
		internal const string UpsertCustomer = $"upsert";
		internal const string GetCustomer = $"{{customerId}}";
		internal const string GetCustomers = $"page/{{pageNumber}}";
		internal const string DeleteCustomer = $"delete";
	}
}
