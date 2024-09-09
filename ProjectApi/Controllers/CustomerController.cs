using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectApi.Data;
using ProjectApi.Model;
using System.Linq;

namespace ProjectApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly EntityDbContext context;
        public CustomerController(EntityDbContext _context)
        {
            context=_context;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll() 
        {
            var customer =await context.Customers.ToListAsync();
            return Ok(customer);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult>GetByID(int id)
        {
            var customer= context.Customers.FirstOrDefault(c=>c.Id==id);
            if (customer == null) 
            {
                return NotFound("The customer is Not found");
            }
            return Ok(customer);
        }
        [HttpPost]
        public async Task<IActionResult>CreateCustomer(CustomerDTO customerDTO)
        {
            if (context.Products.Any(p => p.Name == customerDTO.Name))
            {
                return BadRequest("customer already Exists Try to Update it ");
            }
            if (ModelState.IsValid) 
            {
                var customer = new Customer
                { 
                    Name = customerDTO.Name,
                    Email = customerDTO.Email,
                    PhoneNumber = customerDTO.PhoneNumber
                };
                 context.Customers.Add(customer);
                await context.SaveChangesAsync();
                return Created(nameof(GetByID), new { id = customer.Id });
            }
            return BadRequest(ModelState);
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult>UpdateCustomer(int id,[FromBody]CustomerUpdateDTO customerDTO)
        {
            if (ModelState.IsValid)
            {
                var customer =await context.Customers.FindAsync(id);
                if (customer == null)
                {
                    return NotFound("The customer is Not found");
                }

                customer.Email = customerDTO.Email;
                customer.PhoneNumber = customerDTO.PhoneNumber;

                context.Entry(customer).State = EntityState.Modified;
                await context.SaveChangesAsync();


                return Created(nameof(GetByID), new { id = customer.Id });
            }
        return BadRequest(ModelState);
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult>Deleteitem(int id)
        {
            var customer=await context.Customers.FindAsync(id);
            if (customer == null)
                return NotFound("the customer is not found");
            context.Customers.Remove(customer);
            await context.SaveChangesAsync();
            return Ok("the Delete is successful");
            

        }
    }
}
