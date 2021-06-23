using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;

namespace TheGuardMoreApp.API.Service.Utilities
{
	public class ApiResult : ObjectResult
	{
		public ApiResult(object data = null, HttpStatusCode statusCode = HttpStatusCode.OK,
			ApiErrorModel error = null)
			: base(new ApiResponseModel { Data = data, Error = error })
		{
			StatusCode = (int)statusCode;
		}


	}

}
