using HomeStream.Application.Request;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HomeStream.Api.Controllers;

[ApiController]
[Route("api/v1")]
public class BaseController : ControllerBase
{
    private ISender? _mediatR;
    public ISender Sender => _mediatR ??= HttpContext.RequestServices.GetRequiredService<ISender>();

    public async Task<IActionResult> Execute<ITresponse>(IRequest<ITresponse> request)
    {
        var response = await Sender.Send(request);
        return Ok(response);
    }
}
