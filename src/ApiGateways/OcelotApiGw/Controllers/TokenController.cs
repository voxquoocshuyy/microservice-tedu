using Contracts.Identity;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.Identity;

namespace OcelotApiGw.Controllers;
[ApiController]
[Route("api/[controller]")]
public class TokenController : ControllerBase
{
    private readonly ITokenService _tokenService;

    public TokenController(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    [HttpGet]
    public IActionResult GetToken()
    {
        var token = _tokenService.GetToken(new TokenRequest());
        return Ok(token);
    }
}