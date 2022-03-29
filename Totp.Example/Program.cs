using OtpNet;



var a = 0;
while (a < 10)
{
    var totp = new Totp(new byte[12], step: 30);
    
    var random = totp.ComputeTotp(DateTime.UtcNow);


    Console.WriteLine(random);

    Console.Write("Code: ");
    var num = random;

    var windows = new VerificationWindow(previous: 1, future: 1);

    var list = windows.ValidationCandidates(0);

    var result = totp.VerifyTotp(num, out var remainingTime, VerificationWindow.RfcSpecifiedNetworkDelay);
    Console.WriteLine(remainingTime);

    if (result)
    {
        Console.WriteLine("True");
    }

    a++;
}

Console.ReadKey();