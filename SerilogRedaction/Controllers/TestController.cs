namespace SerilogRedaction.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;

        public TestController(ILogger<TestController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult TestRedaction()
        {
            _logger.LogInformation("User CNIC: {Cnic}", "12345-1234567-1");
            _logger.LogInformation("Credit Card: {CreditCard}", "1234-5678-9012-3456");
            _logger.LogInformation("Message without sensitive data.");
            return Ok("Check logs for redacted data.");
        }
        
        [HttpGet("TestJsonPacketRedaction")]
        public IActionResult TestJsonPacketRedaction()
        {
            var obj = new
            {
                cnic = "12345-1234567-1",
                creditCardNumber = "1234-5678-9012-3456",
                email = "Hamza5656.user@email.com",
                name = "Hamza",
            };
            _logger.LogInformation("User Data: {@UserData}", obj);
            return Ok("Check logs for redacted data.");
        }


    }
}
