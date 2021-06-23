namespace TheGuardMoreApp.API.Service.Utilities
{
	public class ApiResponseModel
	{
		public object Data { get; set; }

		public ApiErrorModel Error { get; set; }
	}

	public class ApiSuccessResponseModel<T>
	{
		public T Data { get; set; }
	}

	public class ApiErrorResponseModel
	{
		public ApiErrorModel Error { get; set; }
	}
}
