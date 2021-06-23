using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Net;

namespace TheGuardMoreApp.API.Service.Utilities
{
	public class ModelValidationFilter : IActionFilter
	{
		public void OnActionExecuting(ActionExecutingContext context)
		{
			if (context.ModelState.IsValid)
			{
				return;
			}
			// get list of errors
			var errors = context.ModelState.Values
				.SelectMany(value => value.Errors)
				.Select(error => error.Exception?.Message ?? error.ErrorMessage);

			// build error message
			var message = $"Model Validation Failed. {string.Join("\n", errors)}";

			// build answer
			context.Result = new ApiResult(null, HttpStatusCode.BadRequest,
				new ApiErrorModel(message, "BadRequest"));
		}

		public void OnActionExecuted(ActionExecutedContext context)
		{ }
	}
}
