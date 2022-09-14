using Dapr.Client;
using Microsoft.AspNetCore.Mvc;

namespace QueueStorageDapr.Controllers
{
    [ApiController]
    public class QueueStorageController : ControllerBase
    {
        private readonly ILogger<QueueStorageController> _logger;
        private readonly DaprClient _daprClient;

        public QueueStorageController(ILogger<QueueStorageController> logger, DaprClient daprClient)
        {
            _logger = logger;
            _daprClient = daprClient;
        }

        [HttpOptions("/queuestorage")]
        public ActionResult Options()
        {
            _logger.LogInformation("options called");
            // ...
            // Acknowledge message
            return Ok();
        }

        [HttpGet("/queuestorage")]
        public async Task<ActionResult> Get(string msg)
        {
            _logger.LogInformation(msg);

            //await _daprClient.InvokeBindingAsync("queuestorage", "create", "valami");

            // Acknowledge message
            return Ok();
        }

        [HttpPost("/queuestorage")]
        public ActionResult Post([FromBody]string msg)
        {
            _logger.LogInformation(msg);
            // Acknowledge message
            return Ok();
        }

    }
}
