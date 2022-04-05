using HashidsNet;
using OtpNet;
using Totp.Example.API.Domain;
using Totp.Example.API.Dtos;

namespace Totp.Example.API.ServiceLayer;

public class OtpService : IOtpService
{
    private readonly IHashids _hashids;
    private List<User> _userList;
    private List<UserOtp> _userOtpList;

    public OtpService(IHashids hashids)
    {
        _hashids = hashids;
        _userOtpList = new List<UserOtp>();
        _userList = new List<User>
        {
            new User
            {
                Id = 1,
                Name = "Fatih"
            }
        };
    }

    public OtpResponseDto CreateOtp(long userId)
    {
        var user = _userList.SingleOrDefault(x => x.Id == userId);

        if (user == null) throw new NotImplementedException();

        var hashId = _hashids.EncodeLong(user.Id);

        var key = KeyGeneration.GenerateRandomKey(OtpHashMode.Sha512);

        var userOtp = _userOtpList.SingleOrDefault(x => x.TransactionId == hashId);
        if (userOtp == null)
        {
            _userOtpList.Add(new UserOtp
            {
                TransactionId = hashId,
                UserId = userId,
                OtpSecretKey = key
            });
        }
        else
        {
            userOtp.OtpSecretKey = key;
        }
        
        var totp = new OtpNet.Totp(key, 60, OtpHashMode.Sha512);

        var random = totp.ComputeTotp(DateTime.UtcNow);

        return new OtpResponseDto
        {
            Totp = random,
            TransactionId = hashId
        };
    }

    public bool VerifyOtp(OtpResponseDto otpResponseDto)
    {
        var isUserTotpExist = _userOtpList.SingleOrDefault(x => x.TransactionId == otpResponseDto.TransactionId);
        if (isUserTotpExist == null)
            throw new Exception();

        var totp = new OtpNet.Totp(isUserTotpExist.OtpSecretKey, 60, OtpHashMode.Sha512);

        var window = new VerificationWindow(previous: 1, future: 1);
        var result = totp.VerifyTotp(otpResponseDto.Totp, out long stepMatched, window);
        if (!result) return result;
        {
            var id = _hashids.DecodeLong(otpResponseDto.TransactionId).FirstOrDefault();
            var user = _userList.FirstOrDefault(x => x.Id == id);
            if (user != null) user.IsVerify = true;
        }

        return result;
    }
}