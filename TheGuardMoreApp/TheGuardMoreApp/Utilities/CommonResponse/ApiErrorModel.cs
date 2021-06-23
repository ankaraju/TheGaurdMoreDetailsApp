namespace TheGuardMoreApp.API.Service.Utilities
{
	public class ApiErrorModel
	{
		#region constructors
		public ApiErrorModel(string message, string type)
		{
			this.Message = message;
			this.Type = type;
		}
		#endregion

		#region properties
		public string Message { get; set; }

		public string Type { get; set; }
		#endregion
	}
}
