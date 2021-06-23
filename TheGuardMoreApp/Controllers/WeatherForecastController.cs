using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TheGuardMoreApp.API.Service.Utilities;

namespace TheGuardMoreApp.Controllers
{
    [ApiController]
    [Route(BaseRoute)]
    [Produces("application/json")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly MySqlConnection _conn;
        public const string BaseRoute = "api/";
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        private string _connectionString = "Server=localhost;Database=sampledb;Uid=root;Pwd=Password01;";



        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
            _conn = new MySqlConnection(_connectionString);
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {


            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiSuccessResponseModel<string>), 201)]
        public async Task<ApiResult> SaveUser(User request)
        {
            try
            {
                var index = 0;
                var sql = "SELECT idUser FROM sampledb.user order by 1 desc LIMIT 1; ";
                var result = await _conn.QueryAsync<IEnumerable<int>>(sql);

                if (result.ToList().Count > 0)
                    index = Convert.ToInt32(result.ToList().First()) > 0 ? Convert.ToInt32(result.ToList().First()) + 1 : 1;
                request.idUser = index;

                _ = this._conn.ExecuteAsync("INSERT INTO user (idUser, Name, CompanyName, Address,PinCode) VALUES (@idUser,@Name,@CompanyName,@Address,@PinCode)", new { request.idUser, request.Name, request.CompanyName, request.Address, request.PINCode });




                return new ApiResult(result, HttpStatusCode.Created);


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return HandleException(ex);
            }


        }

        protected ApiResult HandleException(Exception exception)
        {
            var statusCode = HttpStatusCode.InternalServerError;
            if (exception.GetType() == typeof(RecordNotFoundException))
                statusCode = HttpStatusCode.NotFound;
            else if (exception.GetType() == typeof(RecordConflictException))
                statusCode = HttpStatusCode.Conflict;
            else if (exception.GetType() == typeof(BadRequestException))
                statusCode = HttpStatusCode.BadRequest;

            var error = new ApiErrorModel(exception.Message, statusCode.ToString());
            return new ApiResult(null, statusCode, error);
        }
    }
}
