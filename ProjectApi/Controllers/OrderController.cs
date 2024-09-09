using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectApi.Data;
using ProjectApi.Model;

namespace ProjectApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly EntityDbContext context;
    public OrderController(EntityDbContext _context)
    {
        context = _context;
    }
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var order = await context.Orders.ToListAsync();
        return Ok(order);
    }
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetByID(int id)
    {
        var order = await context.Orders
                                        .Include(o => o.OrderDetails)
                                        .ThenInclude(od => od.Product)
                                        .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null)
        {
            return NotFound();
        }

        return Ok(order);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] OrderCreateDTO orderDto)
    {

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var customer = await context.Customers.FirstOrDefaultAsync(c => c.Id == orderDto.CustomerId);


        if (customer == null)
        {
            return NotFound("Customer not found.");
        }


        var stockErrors = new List<string>();


        var order = new Order
        {
            OrderDate = DateTime.UtcNow,
            Status = orderDto.Status,
            CustomerId = orderDto.CustomerId,
            Customers = customer
        };

        order.OrderDetails = new List<OrderDetail>();


        foreach (var detailDto in orderDto.OrderDetails)
        {
            var product = await context.Products.FirstOrDefaultAsync(p => p.Id == detailDto.ProductId);

            if (product == null)
            {
                stockErrors.Add($"Product with ID {detailDto.ProductId} not found.");
                continue;
            }

            if (detailDto.Quantity > product.QuantityInStock)
            {

                stockErrors.Add($"Insufficient stock for Product ID {detailDto.ProductId}. Available: {product.QuantityInStock}, Requested: {detailDto.Quantity}");
                continue;
            }

            var orderDetail = new OrderDetail
            {
                ProductId = detailDto.ProductId,
                Quantity = detailDto.Quantity,
                PriceAtPurchase = product.Price * detailDto.Quantity
            };
            order.OrderDetails.Add(orderDetail);
        }

        if (stockErrors.Any())
        {
            return BadRequest(string.Join("; ", stockErrors));
        }


        foreach (var orderDetail in order.OrderDetails)
        {
            var product = await context.Products.FirstOrDefaultAsync(p => p.Id == orderDetail.ProductId);
            if (product != null)
            {
                product.QuantityInStock -= orderDetail.Quantity;
                context.Update(product);
            }
        }


        context.Orders.Add(order);
        await context.SaveChangesAsync();


        return CreatedAtAction(nameof(GetByID), new { id = order.Id }, order);
    }
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateOrder(int id, [FromBody] OrderUpdateDTO updateDTO)
    {
        var order = await context.Orders
                                    .Include(o => o.OrderDetails)
                                    .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null)
        {
            return NotFound("the order not found please enter the correct order number");
        }
        order.Status = updateDTO.Status;


        context.Entry(order).State = EntityState.Modified;


        await context.SaveChangesAsync();

        return NoContent();
    }
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        var Order = await context.Orders.FirstOrDefaultAsync(o => o.Id == id);

        if (Order == null)
            return NotFound("Order Doesn't exists");

        context.Orders.Remove(Order);
        await context.SaveChangesAsync();

        return NoContent();
    }
}
