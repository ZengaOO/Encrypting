using Encrypting.Models;
using Encrypting.Repositories.Interfases;
using Encrypting.Repository;
using Encrypting.Services;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Text;
using System.Xml.Linq;

namespace Encrypting.Controllers
{
    [Route("[controller]")]
    [ApiController]
    
    public class HomeController : ControllerBase
    {
        private readonly IDataProtector _protector;
        private readonly IEncryptService _encryptService;
        private readonly IRepository _repository;


        public HomeController(IDataProtectionProvider provider, IEncryptService encryptService, IRepository repository)
        {
            _protector = provider.CreateProtector(GetType().FullName);
            _encryptService = encryptService;
            _repository = repository;
        }

       
        [HttpPost]
        public async Task<IActionResult> EncryptText(string clearText)
        
        {
            var encryptionService = new EncryptService();
            var encrypted = await encryptionService.EncryptAsync(clearText);


            var result = BitConverter.ToString(encrypted).Replace("-",string.Empty);
           //var result = Convert.ToHexString(Byte[]).
            var model = new ContractModel() { Name = result };
            await _repository.EncryptAsync(model);

            //var decrypted = await encryptionService.DecryptAsync(encrypted, enteredText) ;
            
            return Ok(encrypted);
        }

        [HttpGet]
        public async Task<IActionResult>DecryptText([FromQuery]byte[] enteredText)
        {
            var encryptionService = new EncryptService();
            var decrypted = await encryptionService.DecryptAsync(enteredText);

            //var result = BitConverter.ToString(decrypted);
            //  var model = new ContractModel() { Name = result };
            //await _repository.DecryptAsync(model);


            return Ok(decrypted);
        }
    }
}
