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

            this._conn.Execute("INSERT INTO user (idUser, Name, CompanyName, Address,PinCode) VALUES( 1, 'test', 'test', 'test','1111')");
            var sql = "SELECT * FROM user";
            var result = this._conn.Query<User>(sql).ToList();

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
            User resultItem = new User();
            try
            {
                _ = this._conn.ExecuteAsync("INSERT INTO user (idUser, Name, CompanyName, Address,PinCode) VALUES( 1, 'test', 'test', 'test','1111')");
                var sql = "SELECT * FROM user";
                var result = await _conn.QueryAsync<List<User>>(sql);

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
