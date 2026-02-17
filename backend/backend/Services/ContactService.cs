using CsvHelper;
using CsvHelper.Configuration;
using ContactManager.Data;
using ContactManager.Models;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ContactManager.Services
{
    public class ContactService : IContactService
    {
        private readonly AppDbContext _context;

        public ContactService(AppDbContext context)
        {
            _context = context;
        }

        public async Task ProcessCsvFileAsync(IFormFile file)
        {
            using var stream = file.OpenReadStream();
            using var reader = new StreamReader(stream);
            
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
            };

            using var csv = new CsvReader(reader, config);
            csv.Context.RegisterClassMap<ContactMap>();

            var records = csv.GetRecords<Contact>().ToList();

            await _context.Contacts.AddRangeAsync(records);
            await _context.SaveChangesAsync();
        }
    }
}