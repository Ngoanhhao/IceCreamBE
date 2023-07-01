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
            int randomNumber = random.Next(100000, 999999);
            var htmlRegisterCode = HtmlMail.get(randomNumber);
            _IMailHandle.send("Register code: " + randomNumber, htmlRegisterCode, email);
            var codeUpdate = await _HandleResponseCode.GetAllAsync(e => e.Email == email);
            if (codeUpdate != null)
            {
                foreach (var code in codeUpdate)
                {
                    await _HandleResponseCode.UpdateAsync(code);
                }
            }
            await _HandleResponseCode.CreateAsync(new Models.ResponseCode
            {
                Email = email,
                Code = randomNumber.ToString(),
                Status = false,
                ExpirationDate = DateTime.Now.AddMinutes(5)
            }); ;
            return Ok(new Response<string>
            {
                Succeeded = true
            });
        }

        // POST api/<ResponCodeController>
        [HttpPost("/api/checkcode")]
        public async Task<IActionResult> CheckCode(string email, string code)
        {
            var Code = await _HandleResponseCode.GetAsync(e => e.Email.Equals(email) && e.Code.Equals(code));
            if (Code == null || Code.Status == true)
            {
                return BadRequest(new Response<List<AccountDetailDTO>>
                {
                    Succeeded = false,
                    Message = "code incorrect"
                });
            }

            if (Code.ExpirationDate < DateTime.Now)
            {
                return BadRequest(new Response<List<AccountDetailDTO>>
                {
                    Succeeded = false,
                    Message = "code has expired, please re-create it"
                });
            }

            await _HandleResponseCode.UpdateAsync(new Models.ResponseCode { Code = Code.Code, Email = email });

            return Ok(new Response<string>
            {
                Succeeded = true
            });
        }
    }
}
