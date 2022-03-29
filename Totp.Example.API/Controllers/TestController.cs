using Microsoft.AspNetCore.Mvc;
using OtpNet;

namespace Totp.Example.API.Controllers;

[ApiController]
[Route("test")]
public class TestController : Controller
{
    [HttpGet]
    public IActionResult Test()
    {
        var key = KeyGeneration.GenerateRandomKey(OtpHashMode.Sha256);

        var totp = new OtpNet.Totp(key, 60, OtpHashMode.Sha256);

        var random = totp.ComputeTotp(DateTime.UtcNow);
        Console.WriteLine($"First: {totp.RemainingSeconds()}");


        return Ok(new Response
        {
            Key = key,
            Random = random
        });
    }

    [HttpPost]
    public IActionResult Test([FromBody] Response randomResponse)
    {
        var totp = new OtpNet.Totp(randomResponse.Key, 60, OtpHashMode.Sha256);

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
    public string Random { get; set; }
}