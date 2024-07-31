using APIMongo_Tarde.Domains;
using APIMongo_Tarde.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace APIMongo_Tarde.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IMongoCollection<Client> _client;

        public ClientController(MongoDbService mongoDbService)
        {
            _client = mongoDbService.GetDatabase.GetCollection<Client>("client");
        }

        [HttpGet("Listar")]
        public async Task<ActionResult<List<Client>>> Get()
        {
            try
            {
                var client = await _client.Find(FilterDefinition<Client>.Empty).ToListAsync();
                return Ok(client);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("Cadastrar")]
        public async Task<ActionResult> Post(Client client)
        {
            try
            {
                await _client.InsertOneAsync(client);
                return StatusCode(201, client);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("BuscarPorId/{id}")]
        public async Task<ActionResult<Client>> Get(string id)
        {
            try
            {
                var filter = Builders<Client>.Filter.Eq(p => p.Id, id);

                var client = await _client.Find(filter).FirstOrDefaultAsync();

                // Verificando se o produto foi encontrado
                if (client == null)
                {
                    return NotFound($"Produto com ID {id} não encontrado.");
                }

                return Ok(client);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("AtualizarPorId/{id}")]
        public async Task<ActionResult<Client>> Put(string id, [FromBody] Client clientToUpdate)
        {
            try
            {
                var filter = Builders<Client>.Filter.Eq(p => p.Id, id);

                var client = await _client.Find(filter).FirstOrDefaultAsync();

                // Verificando se o cliente foi encontrado
                if (client == null)
                {
                    return NotFound($"Cliente com ID {id} não encontrado.");
                }

                var result = await _client.ReplaceOneAsync(filter, clientToUpdate);
                if (!result.IsAcknowledged || result.ModifiedCount == 0)
                {
                    return NotFound($"Cliente com ID {id} não encontrado ou não atualizado.");
                }

                return Ok(clientToUpdate);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpDelete("DeletarPorId/{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                var filter = Builders<Client>.Filter.Eq(p => p.Id, id);

                var client = await _client.Find(filter).FirstOrDefaultAsync();

                if (client == null)
                {
                    return NotFound($"Produto com ID {id} não encontrado.");
                }

                var deleteResult = await _client.DeleteOneAsync(filter);

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
