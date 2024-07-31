using APIMongo_Tarde.Domains;
using APIMongo_Tarde.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace APIMongo_Tarde.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMongoCollection<Order> _order;
        private readonly IMongoCollection<Client> _client;
        private readonly IMongoCollection<Product> _product;

        public OrderController(MongoDbService mongoDbService)
        {
            _order = mongoDbService.GetDatabase.GetCollection<Order>("order");
            _product = mongoDbService.GetDatabase.GetCollection<Product>("product");
            _client = mongoDbService.GetDatabase.GetCollection<Client>("client");
        }

        [HttpGet("Listar")]
        public async Task<ActionResult<List<Order>>> Get()
        {
            try
            {
                var orders = await _order.Find(FilterDefinition<Order>.Empty).ToListAsync();

                foreach (var order in orders)
                {
                    if (order.ProductId != null)
                    {
                        var filter = Builders<Product>.Filter.Eq(p => p.Id, order.ProductId);

                        order.Products = await _product.Find(filter).ToListAsync();
                    }

                    if (order.ClientId != null)
                    {
                        var filter = Builders<Client>.Filter.Eq(p => p.Id, order.ClientId);

                        order.Clients = await _client.Find(filter).ToListAsync();
                    }
                }
                return Ok(orders);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpPost("Cadastrar")]
        public async Task<ActionResult> Post(Order order)
        {
            try
            {
                await _order.InsertOneAsync(order);
                return StatusCode(201, order);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("BuscarPorId/{id}")]
        public async Task<ActionResult<Order>> Get(string id)
        {
            try
            {
                var filter = Builders<Order>.Filter.Eq(p => p.Id, id);
                var order = await _order.Find(filter).FirstOrDefaultAsync();

                if (order == null)
                {
                    return NotFound($"Ordem com ID {id} não encontrada.");
                }

                if (order.ProductId != null)
                {
                    var productFilter = Builders<Product>.Filter.Eq(p => p.Id, order.ProductId);
                    order.Products = await _product.Find(productFilter).ToListAsync();
                }

                if (order.ClientId != null)
                {
                    var clientFilter = Builders<Client>.Filter.Eq(p => p.Id, order.ClientId);
                    order.Clients = await _client.Find(clientFilter).ToListAsync();
                }

                return Ok(order);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("AtualizarPorId/{id}")]
        public async Task<ActionResult<Order>> Put(string id, [FromBody] Order updateOrder)
        {
            try
            {
                // Validação do ProductId e ClientId
                if (!ObjectId.TryParse(updateOrder.ProductId, out _) ||
                    !ObjectId.TryParse(updateOrder.ClientId, out _))
                {
                    return BadRequest("Os campos ProductId e ClientId devem ser ObjectIds válidos.");
                }

                var filter = Builders<Order>.Filter.Eq(p => p.Id, id);
                var order = await _order.Find(filter).FirstOrDefaultAsync();

                if (order == null)
                {
                    return NotFound($"Ordem com ID {id} não encontrada.");
                }

                // Atualização dos campos da ordem
                order.Date = updateOrder.Date;
                order.Status = updateOrder.Status;

                var result = await _order.ReplaceOneAsync(filter, order);
                if (!result.IsAcknowledged || result.ModifiedCount == 0)
                {
                    return NotFound($"Ordem com ID {id} não encontrada ou não atualizada.");
                }

                order = await _order.Find(filter).FirstOrDefaultAsync();

                return Ok(order); 
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
                var filter = Builders<Order>.Filter.Eq(p => p.Id, id);

                var order = await _order.Find(filter).FirstOrDefaultAsync();

                if (order == null)
                {
                    return NotFound($"Produto com ID {id} não encontrado.");
                }

                var deleteResult = await _order.DeleteOneAsync(filter);

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
