
using Microsoft.AspNetCore.Mvc;
using MeusJogosFavoritos.model;
using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.AspNetCore.Authorization;
using System.Net;

namespace MeusJogosFavoritos.routes {
    [Route("api/Game")]
    [Authorize]
    public class GameRoutes : Controller {
        // Contexto do Entity framework
        private readonly Context _context;
        // Construtor
        public GameRoutes (Context context){
            _context = context;
        }

        // Coleta toddos os jogos adicionados a plataforma
        [HttpGet]
        [Authorize]
        public dynamic Get (){
            var games = _context.games.ToList();
            if(games.Count == 0){
                return new List<dynamic>();
            }
            var gamesSerialized = games.Select(x =>{
                return new {
                    creatorId = x.creator.id,
                    game = new {
                        name = x.name,
                        produces = x.produces,
                        id = x.id
                    }
                };
            }).ToList(); 
            return gamesSerialized;
        }
        [HttpGet("{userId}")]
        public dynamic GetOne([FromRoute]int userId){
            var relation = _context.user_games.Where(x => x.id_User == userId).ToList();
            if(relation.Count == 0){
                return HttpStatusCode.NotFound;
            }
            var games = relation.Select(x => {
                var game = _context.games.Where(game => game.id == x.id_game).First();
                return new{
                    id = game.id,
                    name = game.name,
                    produces = game.produces
                };
            }).ToList();          
            return games;
        }
        // api/Game/{userId}
        // Cria um jogo na plataforma e adiciona aos favoritos
        [HttpPost("{userId}")]
        public dynamic Post([FromBody] Games value , [FromRoute] int userId){
            var user = _context.users.Where(user => user.id == userId).FirstOrDefault();
            if(user == null){
                return HttpStatusCode.NotFound;
            }
            var relation = new User_Games();
            var game = value;
            game.creator = user;
            relation.games = game;
            relation.user = user;
            game.favorite.Add(relation);
            _context.games.Add(game);
            _context.user_games.Add(relation);
            _context.SaveChanges();
            return HttpStatusCode.NoContent;
        }

        // adicionar um jogo da plataforma aos favoritos 
        [HttpPost("{userId}/{gameId}")]
        public dynamic addGameToFavorite([FromRoute] int userId, int gameId){
            var game = _context.games.Where(x => x.id == gameId).FirstOrDefault();
            var user = _context.users.Where(x => x.id == userId).FirstOrDefault();
            if(game == null || user == null){
                return HttpStatusCode.NotFound;
            }
            var relation = new User_Games();
            relation.games = game;
            relation.user = user;
            _context.user_games.Add(relation);
            user.favoritos.Add(relation);
            game.favorite.Add(relation);
            _context.SaveChanges();
            return HttpStatusCode.NoContent;
        }
        
    }
}