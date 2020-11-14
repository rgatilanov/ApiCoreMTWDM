using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        [HttpGet, Authorize(Roles = "Manager")]
        //[HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "Ramón Atilano", "Maricela Méndez" };
        }
    }
}
