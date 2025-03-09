
using Microsoft.AspNetCore.Mvc;

namespace BodyLedger.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProgressController : ControllerBase
    {

        private readonly ILogger<ProgressController> _logger;

        public ProgressController(ILogger<ProgressController> logger)
        {
            _logger = logger;
        }


    }
}
