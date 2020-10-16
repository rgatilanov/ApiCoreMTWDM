using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        //https://localhost:44369/api/user
        // GET: api/<UserController>
        [HttpGet]
        public IEnumerable<User> GetUsers()
        {
            List<User> users = new List<User>();
            users.Add(new Models.User()
            {
                CreateDate = DateTime.Now,
                ID = 1,
                Name = "Ramón Gerardo",
                Nick = "rgatilanov",
                Password = null,
                accountType = AccountType.Administrator
            });

            users.Add(new Models.User()
            {
                CreateDate = DateTime.Now,
                ID = 2,
                Name = "Juan Perez",
                Nick = "juan.perez",
                Password = null,
                accountType = AccountType.Basic,
            });

            return users;
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public User GetUser(int id)
        {
            Models.User user = null;
            if (id == 1)
                user = new Models.User()
                {
                    CreateDate = DateTime.Now,
                    ID = 1,
                    Nick = "rgatilanov",
                    Password = null,
                    Name = "Ramón Gerardo",
                    accountType = AccountType.Administrator
                };
            else
                user = new Models.User()
                {
                    CreateDate = DateTime.Now,
                    ID = 2,
                    Name = "Juan Perez",
                    Nick = "juan.perez",
                    Password = null,
                    accountType = AccountType.Basic,
                };

            return user;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"> {"id": 3,"nick": "leones2019","password": "123123","createDate": "2019-08-02T12:43:02.9396464-05:00"}
        /// </param>
        /// <returns></returns>
        // POST api/<UserController>
        [HttpPost]
        public User PostUser([FromBody] User value)
        {
            /*Lógica a base de datos*/
            value.Name = "ACTUALIZADO!!!";
            return value;
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public User DeleteUser(int id)
        {
            /*Lógica a base de datos*/
            return new Models.User();
        }
    }
}
