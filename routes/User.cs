using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using MeusJogosFavoritos.model;
using MeusJogosFavoritos.services;
using Microsoft.AspNetCore.Authorization;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MeusJogosFavoritos.routes
{
    [Route("api/[controller]")]
    public class User : Controller
    {
        private readonly Context _context;
        public User(Context context){
            _context = context;
        }
        // GET: api/<controller>
        [HttpGet]
        [Authorize(Roles="Super-Admin")]
        public List<UserModel> Get()
        {
            return _context.users.ToList();
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        [Authorize]
        public dynamic Get(int id)
        {
            var user = _context.users.Where(user => user.id == id).FirstOrDefault();
            if (user == null) {
                return HttpStatusCode.NoContent;
            }
            return new {
                id = user.id,
                name = user.name,
                email = user.email,
            };
        }

        // POST api/<controller>
        [HttpPost]
        [AllowAnonymous]
        public void Post([FromBody] UserModel value)
        {
            _context.users.Add(value);
            _context.SaveChanges();
        }

        [HttpGet("friend/{id}")]
        public dynamic getFriend([FromRoute] int id){
            var relation = _context.amigos.Where(x => x.userId == id).ToList();
            if(relation.Count == 0){
                return HttpStatusCode.NoContent;
            }
            var amigos = relation.Select(x =>{
                var amigo = _context.users.Where(user => x.amigoId == user.id).First();
                return new {
                        name = amigo.name,
                        id = amigo.id,
                        email = amigo.email,
                };
            }).ToList();
            return amigos;
        }

        [HttpPost("friend/{id}/{friendId}")]
        [Authorize]
        public dynamic addFriend([FromRoute]int id , int friendId){
            var user = _context.users.Find(id);
            var amigo = _context.users.Find(friendId);
            if (user == null || amigo == null){
                return HttpStatusCode.NotFound;
            }
            var relation = new Amigo();
            relation.amigo = amigo;
            relation.user = user;
            user.amigos.Add(relation);
            amigo.amigoDe.Add(relation);
            _context.SaveChanges();
            return HttpStatusCode.Created;
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        // POST /api/User/login/ - endpoint para se fazer login devolve um token
        [HttpPost("login/")]
        [AllowAnonymous]
        public  dynamic login([FromBody] Dictionary<string,string> request){
            var user = _context.users.Where(user => user.email == request["username"] && user.password == request["password"]).FirstOrDefault();
            if (user == null){
                return NotFound(new { message = "Usuario não existe" });
            }
            var token = TokenService.GenerateToken(user.id);
            return new {
                user = new {
                    id = user.id,
                    name = user.name,
                    email = user.email,
                },
                token = token
            };
        }
    }
}
