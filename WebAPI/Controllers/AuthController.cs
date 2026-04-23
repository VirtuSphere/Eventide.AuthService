using Eventide.AuthService.Application.Commands.Login;
using Eventide.AuthService.Application.Commands.Logout;
using Eventide.AuthService.Application.Commands.RefreshToken;
using Eventide.AuthService.Application.Commands.Register;
using Eventide.AuthService.Application.Queries.GetUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Eventide.AuthService.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator) => _mediator = mediator;

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterCommand command)
    {
        var result = await _mediator.Send(command);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.ErrorMessage);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        var result = await _mediator.Send(command);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.ErrorMessage);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand command)
    {
        var result = await _mediator.Send(command);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.ErrorMessage);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim is null) return Unauthorized();

        var command = new LogoutCommand { UserId = Guid.Parse(userIdClaim) };
        var result = await _mediator.Send(command);
        return result.IsSuccess ? Ok() : BadRequest(result.ErrorMessage);
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentUser()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim is null) return Unauthorized();

        var query = new GetUserQuery { UserId = Guid.Parse(userIdClaim) };
        var result = await _mediator.Send(query);
        return result.IsSuccess ? Ok(result.Value) : NotFound(result.ErrorMessage);
    }
}