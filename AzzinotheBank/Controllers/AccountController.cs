using AzzinotheBank.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IO;
using System.Text;

namespace AzzinotheBank.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {


        private readonly ILogger<AccountController> _logger;

        public AccountController(ILogger<AccountController> logger)
        {
            _logger = logger;
        }

        [HttpPost(Name = "CreateAccount")]
        public IActionResult CreateAccount([FromBody] CreateAccountRequest request)
        {
            if (request.Amount <= 0)
            {
                return BadRequest();
            }

            if (request.Nif.ToString().Length != 9)
            {
                return BadRequest();
            }

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return BadRequest();
            }

            string path = $"{Directory.GetCurrentDirectory()}\\Storage\\{request.Nif}.json";

            if (System.IO.File.Exists(path))
            {
                return BadRequest();
            }

            List<Statement> statement = [new Statement { Date = DateTime.Now, PreviousAmount = request.Amount }];
            Account account = new()
            {
                AccountNumer = Guid.NewGuid(),
                Nif = request.Nif,
                Name = request.Name,
                Amount = request.Amount,
                Statements = statement
            };

            string createAccount = JsonConvert.SerializeObject(account);

            StreamWriter sw = new(path);

            sw.WriteLine(createAccount);

            sw.Close();

            return Ok();
        }

        [HttpDelete("/{{nif}}", Name = "DeleteAccount")]
        public IActionResult DeleteAccount(int request)
        {
            if (request.ToString().Length != 9)
            {
                return BadRequest();
            }

            string path = $"{Directory.GetCurrentDirectory()}\\Storage\\{request}.json";

            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);

                return Ok();
            }

            return BadRequest();
        }

        [HttpGet("/{{nif}}", Name = "GetAccount")]
        public IActionResult GetAccount(int request)
        {
            if (request.ToString().Length != 9)
            {
                return BadRequest();
            }

            string path = $"{Directory.GetCurrentDirectory()}\\Storage\\{request}.json";

            if (System.IO.File.Exists(path))
            {
                StreamReader sr = new(path);

                StringBuilder stringBuilder = new();

                string line;
                while ((line = sr.ReadLine()) != null)
                {
                   stringBuilder.Append(line);
                }

                sr.Close();

                Account account = JsonConvert.DeserializeObject<Account>(stringBuilder.ToString());

                return Ok(account);
            }

            return BadRequest();
        }
    }
}
