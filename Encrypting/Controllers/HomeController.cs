using Encrypting.Models;
using Encrypting.Repositories.Interfases;
using Encrypting.Repository;
using Encrypting.Services;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace Encrypting.Controllers
{

    [ApiController]
    [Route("[controller]")]
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
        public async Task<IActionResult> EncryptText(string clearText, string enteredText)
        
        {
            var encryptionService = new EncryptService();
            var encrypted = await encryptionService.EncryptAsync(//"We use encryption to obscure a piece of information.",
                                                                 clearText, enteredText);

            var result = BitConverter.ToString(encrypted).Replace("-",string.Empty);
           //var result = Convert.ToHexString(Byte[]).
            var model = new ContractModel() { Name = result };
            await _repository.SaveEncriptTextToDatabaseAsync(model);

            //var decrypted = await encryptionService.DecryptAsync(encrypted, enteredText) ;
            
            return Ok(encrypted);
        }

        [HttpGet]
        public async Task<IActionResult> DecryptAsync([FromQuery] byte[] encrypted, string enteredText)
        {
            var encryptionService = new EncryptService();
            var decrypted = await encryptionService.DecryptAsync(encrypted, enteredText);

            //var result = BitConverter.ToString(encrypted);
      
            //var model = new ContractModel() { Name = result };
            //await _repository.SaveEncriptTextToDatabaseAsync(model);


            return Ok(decrypted);
        }
    }
}
