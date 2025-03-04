using AutoMapper;
using MemoryBox.Application.Services;
using MemoryBox.Application.Utils.OTP;
using MemoryBox.Application.ViewModels.Request.Accounts;
using MemoryBox.Application.ViewModels.Response.Accounts;
using MemoryBox.Domain.CustomException;
using MemoryBox.Domain.Entities;
using MemoryBox.Domain.Interfaces;
using MemoryBox.Domain.Interfaces.Email;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace MemoryBox.Application.ServiceImplements
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthentication _authentication;
        private readonly IMapper _mapper;
        private readonly UserManager<Account> _userManager;
        private readonly IFirebaseConfig _firebaseConfig;
        private readonly ISendMail _sendMail;


        public AccountService(IFirebaseConfig firebaseConfig, IUnitOfWork unitOfWork, IMapper mapper, UserManager<Account> userManager, IAuthentication authentication, ISendMail sendMail)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
            _firebaseConfig = firebaseConfig;
            _authentication = authentication;
            _sendMail = sendMail;
        }

        //Register
        public async Task<bool> RegisterUser(ViewModels.Request.Accounts.RegisterRequest registerRequest)
        {
            if (registerRequest.Password != registerRequest.ConfirmPassword)
            {
                throw new CustomException.InvalidDataException("Password và ConfirmPassword không trùng khớp.");
            }

            var existingUser = await _userManager.FindByEmailAsync(registerRequest.Email);
            if (existingUser != null)
            {
                throw new CustomException.InvalidDataException("Email đã tồn tại trong hệ thống.");
            }

            var account = _mapper.Map<Account>(registerRequest);
            account.Email = account.UserName = registerRequest.Email;
            account.Avatar = "";
            account.EmailConfirmed = false;
            var result = await _userManager.CreateAsync(account, registerRequest.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                throw new CustomException.InvalidDataException($"Đăng ký thất bại: {errors}");
            }

            //var roleResult = await _userManager.AddToRoleAsync(account, "Customer");
            //if (!roleResult.Succeeded)
            //{
            //    throw new CustomException.InvalidDataException("Gán vai trò thất bại.");
            //}

            /* var token = await _userManager.GenerateEmailConfirmationTokenAsync(account);
             await _sendMail.SendEmailAsync(registerRequest.Email, "hehe" ,token);*/

            await SendOtpAsync(account);
            return true;

        }

        //Create Account
        public async Task<bool> CreateAccountAsync(AccountRequest request)
        {
            // 1. Kiểm tra mật khẩu có khớp với xác nhận mật khẩu không
            if (request.Password != request.ConfirmPassword)
            {
                throw new CustomException.InvalidDataException("Password và ConfirmPassword không trùng khớp.");
            }

            // 2. Kiểm tra email đã tồn tại trong hệ thống chưa
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
            {
                throw new CustomException.InvalidDataException("Email đã tồn tại trong hệ thống.");
            }

            // 3. Kiểm tra và upload Avatar (nếu có)
            string? AvatarImgUrl = null; // Biến lưu URL ảnh (nếu có)
            if (request.Avatar != null)
            {
                AvatarImgUrl = await _firebaseConfig.UploadFiles(request.Avatar);
            }

            // 4. Tạo tài khoản
            var account = _mapper.Map<Account>(request);
            account.Email = request.Email;
            account.UserName = request.Email;
            account.Avatar = AvatarImgUrl; // Có thể null nếu Avatar không được upload
            account.EmailConfirmed = true;

            var result = await _userManager.CreateAsync(account, request.Password);

            if (!result.Succeeded)
            {
                // Nối các lỗi lại nếu có
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                throw new CustomException.InvalidDataException($"Đăng ký thất bại: {errors}");
            }

            /*var roleResult = await _userManager.AddToRoleAsync(account, request.Role);
            if (!roleResult.Succeeded)
            {
                throw new CustomException.InvalidDataException("Gán vai trò thất bại.");
            }*/

            return true;
        }

        //GetAccountInfo
        public async Task<AccountResponse> GetAccountInfoAsync(Guid userId)
        {
            var account = await _unitOfWork.AccountRepository.GetByIdAsync(userId);
            if (account == null)
            {
                throw new CustomException.DataNotFoundException("Người dùng này không tồn tại.");
            }
            //var roles = await _userManager.GetRolesAsync(account);
            var accountResponse = _mapper.Map<AccountResponse>(account);
            //accountResponse.Roles = roles.ToList();
            return accountResponse;
        }

        //Get All
        public async Task<IEnumerable<AccountResponse>> GetAllAccountsAsync()
        {
            var accounts = await _unitOfWork.AccountRepository.GetAllAsync();
            if (accounts == null)
            {
                throw new CustomException.DataNotFoundException("List người dùng trống.");
            }

            //var accountsResponse = new List<AccountResponse>();
            var accountResponse = _mapper.Map<IEnumerable<AccountResponse>>(accounts);
            /*foreach (var account in accounts)
            {
                var accountResponse = _mapper.Map<AccountResponse>(account);

                // Get roles for each user
                var roles = await _userManager.GetRolesAsync(account);
                accountResponse.Roles = roles.ToList(); // Add roles to the response

                accountsResponse.Add(accountResponse);
            }*/

            return accountResponse;
        }

        //Login
        public async Task<LoginResponse> Login(ViewModels.Request.Accounts.LoginRequest loginDTO)
        {
            var account = _unitOfWork.AccountRepository.Get(r => r.Email == loginDTO.Email).FirstOrDefault();

            if (account == null)
            {
                throw new CustomException.InvalidDataException("Email hoặc mật khẩu không hợp lệ.");
            }

            if (!account.EmailConfirmed)
            {
                throw new CustomException.ForbbidenException("Tài khoản chưa được xác nhận. Vui lòng kiểm tra email để xác nhận.");
            }

            if (account.LockoutEnabled && account.LockoutEnd.HasValue && account.LockoutEnd.Value > DateTimeOffset.UtcNow.ToOffset(TimeSpan.FromHours(7)))
            {
                var remainingLockoutTime = account.LockoutEnd.Value - DateTimeOffset.UtcNow.ToOffset(TimeSpan.FromHours(7));
                throw new CustomException.ForbbidenException($"Tài khoản của bạn đã bị khóa. Vui lòng thử lại sau {remainingLockoutTime.TotalMinutes:N0} phút.");
            }

            if (!_authentication.VerifyPassword(loginDTO.Password, account.PasswordHash, account))
            {

                account.AccessFailedCount++;

                if (account.AccessFailedCount >= 3)
                {
                    account.LockoutEnd = DateTimeOffset.UtcNow.ToOffset(TimeSpan.FromHours(7)).AddMinutes(15);
                    await _unitOfWork.SaveAsync();
                    throw new CustomException.ForbbidenException("Bạn đã đăng nhập sai quá số lần quy định. Tài khoản đã bị khóa trong 15 phút.");
                }

                await _unitOfWork.SaveAsync();
                throw new CustomException.InvalidDataException("Email hoặc mật khẩu không hợp lệ.");
            }

            if (account.LockoutEnd.HasValue && account.LockoutEnd.Value > DateTimeOffset.UtcNow.ToOffset(TimeSpan.FromHours(7)))
            {
                var remainingLockoutTime = account.LockoutEnd.Value - DateTimeOffset.UtcNow.ToOffset(TimeSpan.FromHours(7));
                throw new CustomException.ForbbidenException($"Tài khoản của bạn đã bị khóa. Vui lòng thử lại sau {remainingLockoutTime.TotalMinutes:N0} phút.");
            }

            account.AccessFailedCount = 0;
            account.LockoutEnd = null;
            await _unitOfWork.SaveAsync();

            string token = _authentication.GenerateJWTToken(account);

            return new LoginResponse { token = token };
        }

        //SendOTP
        public async Task SendOtpAsync(Account account)
        {

            // Kiểm tra nếu OTP hiện tại còn hiệu lực, không cần gửi lại trừ khi yêu cầu resend
            //if (account.OtpExpiryTime.HasValue && account.OtpExpiryTime > DateTime.UtcNow && !isResend)
            //{
            //    throw new CustomException.InvalidDataException("OTP hiện tại vẫn còn hiệu lực.");
            //}
            if (account == null)
            {
                throw new ArgumentNullException(nameof(account), "Account không được null.");
            }

            if (string.IsNullOrEmpty(account.Email))
            {
                throw new InvalidOperationException("Account không có email hợp lệ.");
            }

            // Gọi phương thức GenerateOtp từ lớp OtpGenerator trong thư mục Utils
            var otp = OtpGenerator.GenerateOtp();


            // Lưu OTP và thời gian hết hạn
            account.EmailConfirmationOtp = otp;
            account.OtpExpiryTime = DateTime.UtcNow.AddMinutes(1); // OTP hết hạn sau 1 phút

            // Cập nhật thông tin vào tài khoản
            await _userManager.UpdateAsync(account);

            // Gửi OTP qua email
            await _sendMail.SendEmailAsync(account.Email, "OTP xác nhận", $"Mã OTP của bạn là: {otp}");
        }

        //Verify account
        public async Task<bool> VerifyOtpAsync(string email, string otp)
        {
            var account = await _userManager.FindByEmailAsync(email);
            if (account == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy tài khoản với email này.");
            }

            // Kiểm tra OTP có khớp và còn hiệu lực không
            if (account.EmailConfirmationOtp != otp)
            {
                throw new CustomException.InvalidDataException("Mã OTP không chính xác.");
            }

            if (account.OtpExpiryTime < DateTime.UtcNow)
            {
                throw new CustomException.InvalidDataException("Mã OTP đã hết hạn.");
            }

            // Nếu OTP đúng và còn hiệu lực, xóa OTP và xác nhận email
            account.EmailConfirmed = true;
            account.EmailConfirmationOtp = null;
            account.OtpExpiryTime = null;
            await _userManager.UpdateAsync(account);

            return true;
        }

        //Forgot password
        public async Task<bool> ForgotPasswordAsync(string email)
        {
            var account = await _userManager.FindByEmailAsync(email);
            if (account == null) return false;

            // Tạo và gửi OTP thay vì token
            await SendOtpAsync(account);

            return true;
        }

        //Reset password
        public async Task<bool> ResetPasswordAsync(ResetPasswordRequest request)
        {
            var account = await _userManager.FindByEmailAsync(request.Email);

            if (account == null)
            {
                throw new CustomException.InvalidDataException("Email không tồn tại trong hệ thống");
            }

            // Xác thực OTP
            if (account.EmailConfirmationOtp != request.ResetCode)
            {
                throw new CustomException.InvalidDataException("Mã OTP không chính xác.");
            }

            if (account.OtpExpiryTime < DateTime.UtcNow)
            {
                throw new CustomException.InvalidDataException("Mã OTP đã hết hạn.");
            }

            // Đặt lại mật khẩu
            var result = await _userManager.RemovePasswordAsync(account); // Xóa mật khẩu cũ trước khi đặt mật khẩu mới
            if (!result.Succeeded)
            {
                throw new CustomException.InvalidDataException("Không thể xóa mật khẩu cũ.");
            }

            result = await _userManager.AddPasswordAsync(account, request.NewPassword);  // Thêm mật khẩu mới
            if (!result.Succeeded)
            {
                throw new CustomException.InvalidDataException("Đặt lại mật khẩu thất bại.");
            }

            // Xóa OTP sau khi sử dụng
            account.EmailConfirmationOtp = null;
            account.OtpExpiryTime = null;

            await _userManager.UpdateAsync(account);

            return true;
        }

        // Resend OTP
        public async Task<bool> ResendOtpAsync(string email)
        {
            var account = await _userManager.FindByEmailAsync(email);
            if (account == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy tài khoản với email này.");
            }

            // Gửi lại OTP
            await SendOtpAsync(account);

            return true;
        }
    }
}
