using Microsoft.AspNetCore.Mvc;
using OtpNet;

namespace Totp.Example.API.Controllers;

[ApiController]
[Route("test")]
public class TestController : Controller
{
    [HttpGet("otp")]
    public IActionResult GetOtp()
    {
        var key = KeyGeneration.GenerateRandomKey(OtpHashMode.Sha512);

        var totp = new OtpNet.Totp(key, 60, OtpHashMode.Sha512);

        var random = totp.ComputeTotp(DateTime.UtcNow);
        
        //TODO: DB Insert yaparken TransactionId (Guid veya HashId) oluştur 
        //Bu bilgileri UserOtp tablosunda tut. UserOtp -> UserId, TransactionId, Otp bilgileri tutulacak
        //Verify yaparken kullanıcıdan TransactionId ve Otp bilgisini alıp kontrol sağlayacağız
        
        return Ok(new Response
        {
            Key = key,
            Random = random
        });
    }

    [HttpPost("verify")]
    public IActionResult Verify([FromBody] Response randomResponse)
    {
        var totp = new OtpNet.Totp(randomResponse.Key, 60, OtpHashMode.Sha512);

        Console.WriteLine($"Second: {totp.RemainingSeconds()}");
        var window = new VerificationWindow(previous:1, future:1);
        var result = totp.VerifyTotp(randomResponse.Random, out long stepMatched,window);
        DateTime start = DateTime.Now;
        DateTime date = start.AddMilliseconds(stepMatched).ToLocalTime();

        Console.WriteLine(date);

        return Ok(result);
    }
}

public class Response
{
    public byte[] Key { get; set; }
    
    public string UserId { get; set; }
    public string Random { get; set; }
}