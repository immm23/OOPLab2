using LabOOP2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LabOOP2.Controllers
{
    [Route("api/")]
    [ApiController]
    public class PassportsController : ControllerBase
    {
        private readonly Context _context;

        public PassportsController(Context context)
        {
            _context = context;
        }

        [HttpGet("passports")]
        public Passport[] GetPassports()
        {
            return _context.Passports.ToArray();
        }

        [HttpGet("customers/{id}/passport")]
        public IActionResult GetPassport(int id)
        {
            var dbCustomer = _context.Customers.Where(p => p.Id == id)
                .Include(p => p.Passport)
                .FirstOrDefault();

            if (dbCustomer is null)
            {
                return NotFound();
            }

            return Ok(dbCustomer.Passport);
        }

        [HttpPost("customers/{id}/passport/add")]
        public IActionResult Create(int id, Passport passport)
        {
            var dbCustomer = _context.Customers.Where(p => p.Id == id)
               .Include(p => p.Passport)
               .FirstOrDefault();

            if (dbCustomer is null)
            {
                return NotFound();
            }
            dbCustomer.Passport = passport;
            _context.SaveChanges();
            return Ok();
        }
    }
}
