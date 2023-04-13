using LabOOP2.Models;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace LabOOP2.Controllers
{
    [Route("api/")]
    [ApiController]
    public class LoansController : ControllerBase
    {
        private readonly Context _context;

        public LoansController(Context context)
        {
            _context = context;
        }

        [HttpGet("loans")]
        public Loan[] GetLoans()
        {
            return _context.Loans.Include(p => p.Customer)
                .ToArray();
        }

        [HttpPost("customer/{id}/loans/new")]
        public IActionResult Create(int id, decimal amount)
        {
            var dbCustomer = _context.Customers.Where(p => p.Id == id)
               .Include(p => p.Passport)
               .Include(p => p.BankAccount)
               .FirstOrDefault();

            if (dbCustomer is null)
            {
                return NotFound();
            }
            else if (dbCustomer.BankAccount is null || dbCustomer.Passport is null)
            {
                return BadRequest();
            }

            Random random = new Random();
            var percentage = random.Next(0, 250);
            var length = random.Next(1, 12);

            Loan loan = new Loan()
            {
                Amount = amount,
                FromDate = DateTime.Now.Date,
                TillDate = DateTime.Now.AddMonths(length).Date,
                Percentage = percentage,
                FinalSum = amount * (decimal)Math.Pow(1 + percentage / 100, length),
            };
            Transaction transaction = new()
            {
                Amount = amount,
                FromDescription = "From Loan",
                ToDescription = "To Acount Balance",
                Time = DateTime.Now
            };

            dbCustomer.Loans.Add(loan);
            dbCustomer.Balance += amount;
            dbCustomer.Transactions.Add(transaction);
            _context.SaveChanges();
            return Created(nameof(Create), loan);
        }

        [HttpPost("customers/{customerId}/loans/{loanId}/payoff")]
        public IActionResult PayOff(int customerId, int loanId, decimal amount)
        {
            var dbCustomer = _context.Customers.Where(p => p.Id == customerId)
               .Include(p => p.Passport)
               .FirstOrDefault();
            var dbLoan = _context.Loans.Where(p => p.Id == loanId)
               .FirstOrDefault();
            if (dbCustomer is null || dbLoan is null)
            {
                return NotFound();
            }

            dbLoan.PayedOff += amount;
            dbCustomer.Balance -= amount;
            Transaction transaction = new()
            {
                Amount = amount,
                FromDescription = "From Balance",
                ToDescription = "To Loan",
                Time = DateTime.Now
            };
            dbCustomer.Transactions.Add(transaction);
            _context.SaveChanges();
            return Ok();
        }
    }
}
