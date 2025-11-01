namespace BlazorApp.ApiHost.Common;

internal static class RouteConstants
{
	internal static class V1
	{
		internal const string Heartbeat = "customer/heartbeat";
		internal const string GetCustomer = $"customer/{{customerId}}";
		internal const string GetCustomers = $"customers/page/{{pageNumber}}";
		internal const string UpsertCustomer = $"customer/upsert";
		internal const string DeleteCustomer = $"customer/delete";
	}
}
