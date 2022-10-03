using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Sentinel.API.Controllers.V1
{
    [Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class AuthController : ControllerBase
    {
    }
}
