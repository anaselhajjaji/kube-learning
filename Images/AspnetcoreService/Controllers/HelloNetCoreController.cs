using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace AspnetcoreService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HelloNetCoreController : ControllerBase
    {
        private readonly ILogger<HelloNetCoreController> logger;

        public HelloNetCoreController(ILogger<HelloNetCoreController> logger)
        {
            this.logger = logger;
        }

        // GET: api/<HelloNetCoreController>
        [HttpGet]
        public string Get()
        {
            logger.LogInformation("API GET CALL, returning hello.");
            return $"Hello from ASP.NET Core App in { Environment.MachineName }";
        }
    }
}
