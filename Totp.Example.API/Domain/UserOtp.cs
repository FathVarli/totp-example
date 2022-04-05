namespace Totp.Example.API.Domain;

public class UserOtp
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public string? TransactionId { get; set; }
    public byte[]? OtpSecretKey { get; set; }
    
}