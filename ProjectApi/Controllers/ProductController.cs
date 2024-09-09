using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectApi.Data;
using ProjectApi.Model;
using static ProjectApi.Model.ProductDTO;

namespace ProjectApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly EntityDbContext context;
    public ProductController(EntityDbContext _context)
    {
        context = _context;
    }
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var product = await context.Products.ToListAsync();
        return Ok(product);
    }
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetByID(int id)
    {
        var Product = context.Products.FirstOrDefault(p => p.Id == id);
        if (Product == null)
        {
            return NotFound();
        }

        return Ok(Product);
    }
    [HttpPost]
    public async Task<IActionResult> AddTheProduct([FromBody] ProductCreateDto productDto)
    {
        if (context.Products.Any(p => p.Name == productDto.Name))
        {
            return BadRequest("Product already Exists Try to Update it ");
        }
        if (ModelState.IsValid)
        {
            var Products = new Product
            {
                Name = productDto.Name
               ,
                Price = productDto.Price
               ,
                QuantityInStock = productDto.QuantityInStock
            };
            context.Products.Add(Products);
            await context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetByID), new { id = Products.Id }, Products);
        }
        return BadRequest(ModelState);
    }
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateProduct(int id, ProductUpdateDto productDto)
    {
        {
            if (ModelState.IsValid)
            {
                var product = await context.Products.FindAsync(id);
                if (product == null) { return NotFound("Product Not found Try to Add it "); }

                product.Price = productDto.Price;
                product.QuantityInStock = productDto.QuantityInStock;

                await context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetByID), new { id = product.Id }, product);
            }
            return BadRequest(ModelState);
        }
    }
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProudect(int id)
    {
        var product = await context.Products.FindAsync(id);
        if (product == null) { return NotFound();};
        context.Products.Remove(product);
        await context.SaveChangesAsync();
        return Ok("the Delete Is Successful");

    }
}
