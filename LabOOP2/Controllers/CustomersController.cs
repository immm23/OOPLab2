using AutoMapper;
using LabOOP2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LabOOP2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly Context _context;
        private readonly IMapper _mapper;

        public CustomersController(Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet()]
        public Customer[] GetCustomers()
        {
            return _context.Customers
                .Include(p => p.Transactions)
                .Include(p => p.Loans)
                .Include(p => p.Passport)
                .Include(p => p.BankAccount)
                .ToArray();
        }

        [HttpGet("{id}")]
        public IActionResult GetCustomer(int id)
        {
            var dbCustomer = _context.Customers.Where(p => p.Id == id)
                .Include(p => p.Transactions)
                .Include(p => p.Loans)
                .Include(p => p.Passport)
                .Include(p => p.BankAccount)
                .FirstOrDefault();

            if (dbCustomer is null)
            {
                return NotFound();
            }

            return Ok(dbCustomer);
        }

        [HttpPost("new")]
        public IActionResult Create(CustomerInputModel customerInput)
        {
            var customer = _mapper.Map<Customer>(customerInput);
            _context.Add(customer);
            _context.SaveChanges();
            return Ok();
        }

        [HttpPut("update")]
        public IActionResult Update(CustomerInputModel customerInput)
        {
            var customer = _mapper.Map<Customer>(customerInput);
            var dbCustomer = _context.Customers.Where(p => p.Id == customer.Id)
                .FirstOrDefault();
            if(dbCustomer is null)
            {
                return NotFound();
            }
            _context.Entry<Customer>(dbCustomer).State = EntityState.Detached;

            _context.Update(customer);
            _context.SaveChanges();
            return Ok();
        }

        [HttpDelete("{id}/delete")]
        public IActionResult Delete(int id)
        {
            var dbCustomer = _context.Customers.Where(p => p.Id == id)
                .FirstOrDefault();

            if (dbCustomer is null)
            {
                return NotFound();
            }

            _context.Remove(dbCustomer);
            _context.SaveChanges();
            return Ok();
        }

        [HttpPost("{id}/balance/move")]
        public IActionResult MoveBalance(int id, decimal amount)
        {
            var dbCustomer = _context.Customers.Where(p => p.Id == id)
                .FirstOrDefault();

            if (dbCustomer is null || dbCustomer.BankAccount is null)
            {
                return NotFound();
            }
            else if (dbCustomer.Balance >= amount)
            {
                return BadRequest();
            }

            Transaction transaction = new()
            {
                Amount = amount,
                FromDescription = "From Balance",
                ToDescription = "To Bank Acount",
                Time = DateTime.Now
            };
            dbCustomer.Balance -= amount;
            dbCustomer.Transactions.Add(transaction);

            //sending money to real bank acount

            _context.SaveChanges();

            return Ok();
        }
    }
}
