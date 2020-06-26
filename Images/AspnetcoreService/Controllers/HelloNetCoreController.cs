using Microsoft.AspNetCore.Mvc;
using System;

namespace AspnetcoreService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HelloNetCoreController : ControllerBase
    {
        // GET: api/<HelloNetCoreController>
        [HttpGet]
        public string Get()
        {
            return $"Hello from ASP.NET Core App in { Environment.MachineName }";
        }
    }
}
