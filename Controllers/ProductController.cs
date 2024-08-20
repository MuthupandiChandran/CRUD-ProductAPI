using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductAPI.Context;
using ProductAPI.DTO;

[Route("api/[controller]")]
[ApiController]
/*[Authorize(Roles = "Admin,User")]*/ // Add this attribute to require authentication
public class ProductController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ProductController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        return await _context.Products.FromSqlRaw("EXEC GetProducts").ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
       
        var products = _context.Products.FromSqlRaw("EXEC GetProductById @ProductId={0}", id).AsEnumerable(); //From SQL raw id not Composable with LINQ So that we're using AsEnumerable
        var product = products.FirstOrDefault();

        if (product == null)
        {
            return NotFound();
        }

        return product;
    }

    [HttpPost]
    [Consumes("application/json")]
    public async Task<ActionResult<Product>> AddProduct(Product product)
    {
        await _context.Database.ExecuteSqlRawAsync("EXEC AddProduct @Name={0}, @Price={1}, @Quantity={2}", product.Name, product.Price, product.Quantity);

        return CreatedAtAction(
            nameof(GetProduct),
            new { id = product.ProductId },
            new Response { Status = "Success", Message = "Successfully Inserted" });
    }

    [HttpPut("{id}")]
    [Consumes("application/json")]
    public async Task<IActionResult> UpdateProduct(int id, Product product)
    {
        if (product.ProductId == 0) {
            product.ProductId = id;
        }

        if (id != product.ProductId)
        {
            return BadRequest();
        }

        await _context.Database.ExecuteSqlRawAsync("EXEC UpdateProduct @ProductId={0}, @Name={1}, @Price={2}, @Quantity={3}", id, product.Name, product.Price, product.Quantity);
        return Ok(new Response { Status = "Success", Message = "Successfully Updated!" });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        await _context.Database.ExecuteSqlRawAsync("EXEC DeleteProduct @ProductId={0}", id);
        return Ok(new Response { Status = "Success", Message = "Successfully Deleted!" });
    }
}
