using Microsoft.AspNetCore.Mvc;
using Totp.Example.API.Dtos;
using Totp.Example.API.ServiceLayer;

namespace Totp.Example.API.Controllers;

[ApiController]
[Route("test")]
public class TestController : Controller
{
    private readonly IOtpService _otpService;

    public TestController(IOtpService otpService)
    {
        _otpService = otpService;
    }

    [HttpGet("otp")]
    public IActionResult GetOtp([FromQuery] long userId)
    {
        //TODO: DB Insert yaparken TransactionId (Guid veya HashId) oluştur 
        //Bu bilgileri UserOtp tablosunda tut. UserOtp -> UserId, TransactionId, Otp bilgileri tutulacak
        //Verify yaparken kullanıcıdan TransactionId ve Otp bilgisini alıp kontrol sağlayacağız

        var result = _otpService.CreateOtp(userId);
        return Ok(result);
    }

    [HttpPost("verify")]
    public IActionResult Verify([FromBody] OtpResponseDto otpResponseDto)
    {
        var result = _otpService.VerifyOtp(otpResponseDto);
        return Ok(result);
    }
}
