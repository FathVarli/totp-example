namespace Totp.Example.API.Domain;

public class User
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public bool IsVerify { get; set; }
}