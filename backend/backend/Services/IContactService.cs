using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace ContactManager.Services
{
    public interface IContactService
    {
        Task ProcessCsvFileAsync(IFormFile file);
    }
}