using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SchengenCalculator.Api.Data;
using SchengenCalculator.Api.Services;
using SchengenCalculator.Core;
using SchengenCalculator.Core.Dto;

namespace SchengenCalculator.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(
    UserManager<AppUser> userManager,
    SignInManager<AppUser> signInManager,
    TokenService tokenService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register(RegisterRequest request)
    {
        if (await userManager.FindByEmailAsync(request.Email) is not null)
            return BadRequest(new { message = "Email is already registered." });

        var user = new AppUser
        {
            UserName    = request.Email,
            Email       = request.Email,
            DisplayName = request.DisplayName,
            Tier        = UserTier.Free,
        };

        var result = await userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
            return BadRequest(new { errors = result.Errors.Select(e => e.Description) });

        return Ok(BuildResponse(user));
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest request)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null)
            return Unauthorized(new { message = "Invalid email or password." });

        var result = await signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: false);
        if (!result.Succeeded)
            return Unauthorized(new { message = "Invalid email or password." });

        return Ok(BuildResponse(user));
    }

    private AuthResponse BuildResponse(AppUser user) =>
        new(tokenService.CreateToken(user),
            user.Email!,
            user.DisplayName,
            user.Tier,
            AppConfig.TripLimitFor(user.Tier));
}
