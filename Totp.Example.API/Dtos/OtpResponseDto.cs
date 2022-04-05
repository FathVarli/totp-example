namespace Totp.Example.API.Dtos;

public class OtpResponseDto
{
    public string? TransactionId { get; set; }
    public string? Totp { get; set; }
}