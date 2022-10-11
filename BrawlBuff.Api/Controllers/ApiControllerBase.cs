using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BrawlBuff.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class ApiControllerBase : ControllerBase
    {
        private ISender _mediator = null!;

        protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();

        protected void NormalizeTag(ref string tag)
        {
            tag = "%23" + tag;
        }
    }
}
