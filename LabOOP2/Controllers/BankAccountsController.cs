using LabOOP2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LabOOP2.Controllers
{
    [Route("api/")]
    [ApiController]
    public class BankAccountsController : ControllerBase
    {
        private readonly Context _context;

        public BankAccountsController(Context context)
        {
            _context = context;
        }

        [HttpGet("bankAccounts")]
        public BankAccount[] GetBankAccounts()
        {
            return _context.BankAccounts.ToArray();
        }

        [HttpGet("customers/{id}/bankAccount")]
        public IActionResult GetBankAccount(int id)
        {
            var dbCustomer = _context.Customers.Where(p => p.Id == id)
                .Include(p => p.BankAccount)
                .FirstOrDefault();

            if (dbCustomer is null)
            {
                return NotFound();
            }

            return Ok(dbCustomer.BankAccount);
        }

        [HttpPost("customers/{id}/bankAccount/add")]
        public IActionResult Create(int id, BankAccount bankAccount)
        {
            var dbCustomer = _context.Customers.Where(p => p.Id == id)
                .Include(p => p.BankAccount)
                .FirstOrDefault();

            if (dbCustomer is null)
            {
                return NotFound();
            }

            dbCustomer.BankAccount = bankAccount;
            _context.SaveChanges();
            return Ok();
        }

        [HttpGet("customers/{id}/bankAccount/update")]
        public IActionResult Update(int id, BankAccount bankAccount)
        {
            var dbCustomer = _context.Customers.Where(p => p.Id == id)
                .Include(p => p.BankAccount)
                .FirstOrDefault();

            if (dbCustomer is null || dbCustomer.BankAccount is null)
            {
                return NotFound();
            }

            _context.Update(bankAccount);
            _context.SaveChanges();
            return Ok();
        }
    }
}
