using APIMongo_Tarde.Domains;
using APIMongo_Tarde.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace APIMongo_Tarde.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ProductContoller : ControllerBase
    {
        private readonly IMongoCollection<Product> _product;

        public ProductContoller(MongoDbService mongoDbService)
        {
            _product = mongoDbService.GetDatabase.GetCollection<Product>("product");
        }

        [HttpGet("Listar")]
        public async Task<ActionResult<List<Product>>> Get()
        {
            try
            {
                var products = await _product.Find(FilterDefinition<Product>.Empty).ToListAsync();
                return Ok(products);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("Cadastrar")]
        public async Task<ActionResult> Post(Product product)
        {
            try
            {
                await _product.InsertOneAsync(product);
                return StatusCode(201, product);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("BuscarPorId/{id}")]
        public async Task<ActionResult<Product>> Get(string id)
        {
            try
            {
                var filter = Builders<Product>.Filter.Eq(p => p.Id, id);

                var product = await _product.Find(filter).FirstOrDefaultAsync();

                // Verificando se o produto foi encontrado
                if (product == null)
                {
                    return NotFound($"Produto com ID {id} não encontrado.");
                }

                return Ok(product);
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        [HttpPut("AtualizarPorId/{id}")]
        public async Task<ActionResult<Product>> Put(string id, [FromBody] Product updatedProduct)
        {
            try
            {
                var filter = Builders<Product>.Filter.Eq(p => p.Id, id);
                var product = await _product.Find(filter).FirstOrDefaultAsync();

                if (product == null)
                {
                    return NotFound($"Produto com ID {id} não encontrado.");
                }

                var result = await _product.ReplaceOneAsync(filter, updatedProduct);
                if (!result.IsAcknowledged || result.ModifiedCount == 0)
                {
                    return NotFound($"Produto com ID {id} não encontrado ou não atualizado.");
                }

                return Ok(updatedProduct);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("DeletarPorId/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var filter = Builders<Product>.Filter.Eq(p => p.Id, id);
                var product = await _product.Find(filter).FirstOrDefaultAsync();

                if (product == null)
                {
                    return NotFound($"Produto com ID {id} não encontrado.");
                }

                var deleteResult = await _product.DeleteOneAsync(filter);

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
