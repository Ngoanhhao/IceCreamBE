using IceCreamBE.DTO;
using IceCreamBE.Modules;
using IceCreamBE.Repository.Irepository;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IceCreamBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResponseCodeController : ControllerBase
    {
        private IMailHandle _IMailHandle;
        private IHandleResponseCode _HandleResponseCode;

        public ResponseCodeController(IMailHandle MailHandle, IHandleResponseCode IResponseHandler)
        {
            _IMailHandle = MailHandle;
            _HandleResponseCode = IResponseHandler;
        }
        // POST api/<ResponCodeController>
        [HttpPost]
        public async Task<IActionResult> Post([FromQuery] string email)
        {
            Random random = new Random();
            int randomNumber = random.Next(10000000, 100000000);
            var htmlRegisterCode = HtmlMail.get(randomNumber);
            _IMailHandle.send("Register code: " + randomNumber, htmlRegisterCode, email);
            await _HandleResponseCode.CreateAsync(new Models.ResponseCode
            {
                Email = email,
                Code = randomNumber.ToString(),
                Status = false,
                ExpirationDate = DateTime.UtcNow.AddMinutes(5)
            }); ;
            return Ok(new Response<string>
            {
                Succeeded = true,
                Data = randomNumber.ToString()
            });
        }
    }
}
