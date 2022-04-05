using Totp.Example.API.Dtos;

namespace Totp.Example.API.ServiceLayer;

public interface IOtpService
{
    OtpResponseDto CreateOtp(long userId);
    bool VerifyOtp(OtpResponseDto otpResponseDto);
}