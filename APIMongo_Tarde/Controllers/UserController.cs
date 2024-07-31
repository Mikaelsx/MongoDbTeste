using APIMongo_Tarde.Domains;
using APIMongo_Tarde.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace APIMongo_Tarde.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMongoCollection<User> _user;

        public UserController(MongoDbService mongoDbService)
        {
            _user = mongoDbService.GetDatabase.GetCollection<User>("user");
        }

        [HttpGet("Listar")]
        public async Task<ActionResult<List<User>>> Get()
        {
            try
            {
                var user = await _user.Find(FilterDefinition<User>.Empty).ToListAsync();
                return Ok(user);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("Cadastrar")]
        public async Task<ActionResult> Post(User user)
        {
            try
            {
                await _user.InsertOneAsync(user);
                return StatusCode(201, user);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("BuscarPorId/{id}")]
        public async Task<ActionResult<User>> Get(string id)
        {
            try
            {
                var filter = Builders<User>.Filter.Eq(p => p.Id, id);

                var user = await _user.Find(filter).FirstOrDefaultAsync();

                // Verificando se o User foi encontrado
                if (user == null)
                {
                    return NotFound($"Produto com ID {id} não encontrado.");
                }

                return Ok(user);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("AtualizarPorId/{id}")]
        public async Task<ActionResult<User>> Put(string id, [FromBody] User updateUser)
        {
            try
            {
                var filter = Builders<User>.Filter.Eq(p => p.Id, id);

                var user = await _user.Find(filter).FirstOrDefaultAsync();

                // Verificando se o User foi encontrado
                if (user == null)
                {
                    return NotFound($"Produto com ID {id} não encontrado.");
                }

                var result = await _user.ReplaceOneAsync(filter, updateUser);
                if (!result.IsAcknowledged || result.ModifiedCount == 0)
                {
                    return NotFound($"Produto com ID {id} não encontrado ou não atualizado.");
                }

                return Ok(updateUser);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("DeletePorId/{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                var filter = Builders<User>.Filter.Eq(p => p.Id, id);

                var user = await _user.Find(filter).FirstOrDefaultAsync();

                // Verificando se o User foi encontrado
                if (user == null)
                {
                    return NotFound($"Produto com ID {id} não encontrado.");
                }

                var deleteResult = await _user.DeleteOneAsync(filter);

                if (deleteResult.DeletedCount > 0)
                {
                    return NoContent();
                }
                else
                {
                    return StatusCode(500, "Erro ao deletar o produto.");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
