using HomeStream.Application.Request;
using Microsoft.AspNetCore.Mvc;

namespace HomeStream.Api.Controllers.v1;

public class TestController : BaseController
{
    /// <summary>
    /// Gets the addition result for testing the code.
    /// </summary>
    /// <returns></returns>
    [HttpGet("test")]
    public async Task<IActionResult> GetTestResult([FromQuery] int leftOperand, [FromQuery] int rightOperand)
    {
        return await Execute(new TestQuery()
        {
            LeftOperand = leftOperand,
            RightOperand = rightOperand
        });
    }
}
