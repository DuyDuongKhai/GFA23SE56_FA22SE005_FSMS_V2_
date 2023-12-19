using FSMS.Service.ViewModels.Authentications;

namespace FSMS.Service.Services.AuthServices
{
    public interface IAuthService
    {
        Task<SignInAccount> SignInAsync(Account account, JwtAuth jwtAuth);
        Task<RefreshTokenResponse> RefreshTokenAsync(string refreshToken, JwtAuth jwtAuth);
        Task<bool> SendPasswordResetEmailAndCheckUserAsync(string userEmail);
        string GenerateOTP();
        Task<bool> ResetPasswordAsync(string otp, string password);
        Task<bool> LogoutAsync(string userEmail, SignInAccount signInAccount);
        Task RegisterAccountAsync(RegisterAccount registerAccount);
        Task<bool> ConfirmAccountAsync(string otp);




    }
}
