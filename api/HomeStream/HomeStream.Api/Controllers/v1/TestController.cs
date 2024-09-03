using HomeStream.Application.Request;
using Microsoft.AspNetCore.Mvc;

namespace HomeStream.Api.Controllers.v1;

/// <summary>
/// Defines the controller for testing the status of the application.
/// </summary>
public class TestController : BaseController
{
    /// <summary>
    /// Gets the addition result for testing the code.
    /// </summary>
    /// <param name="leftOperand">The left operand</param>
    /// <param name="rightOperand">The right operand</param>
    /// <returns>The result of addition operation.</returns>
    [HttpGet("test")]
    public async Task<IActionResult> AddOperation([FromQuery] int leftOperand, [FromQuery] int rightOperand)
    {
        return await Execute(new AddQuery()
        {
            LeftOperand = leftOperand,
            RightOperand = rightOperand
        });
    }
}
