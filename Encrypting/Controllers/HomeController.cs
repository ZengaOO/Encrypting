using Encrypting.Models;
using Encrypting.Services;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace Encrypting.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly IDataProtector _protector;
        private readonly IEncryptService _encryptService;

        public HomeController(IDataProtectionProvider provider, IEncryptService encryptService)
        {
            _protector = provider.CreateProtector(GetType().FullName);
            _encryptService = encryptService;
        }

        //[HttpGet]
        //public IActionResult Index()
        //{
        //    var model = _service.GetAll().Select(c => new ContractModel
        //    {
        //        Id = _protector.Protect(c.Id.ToString()),
        //        Name = c.Name
        //    }).ToList();
        //    return Ok(model);
        //}

        //[HttpPost]
        //public IActionResult Details(string id)
        //{
        //    var contract = _service.Find(Convert.ToInt32(_protector.Unprotect(id)));
        //    return Ok(contract);
        //}

        [HttpPost]
        public async Task<string> EncryptText(string enteredText)
        {
            var encryptionService = new EncryptService();
            var encrypted = await encryptionService.EncryptAsync("We use encryption to obscure a piece of information.",
                                                                 enteredText);

           var result = BitConverter.ToString(encrypted);
           return result;
        }
    }

   
}
