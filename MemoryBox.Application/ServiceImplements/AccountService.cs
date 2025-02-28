using AutoMapper;
using MemoryBox.Application.Services;
using MemoryBox.Application.ViewModels.Request.Accounts;
using MemoryBox.Application.ViewModels.Response.Accounts;
using MemoryBox.Domain.CustomException;
using MemoryBox.Domain.Entities;
using MemoryBox.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
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


        public AccountService(IFirebaseConfig firebaseConfig, IUnitOfWork unitOfWork, IMapper mapper, UserManager<Account> userManager, IAuthentication authentication)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
            _firebaseConfig = firebaseConfig;
            _authentication = authentication;
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
        public async Task<LoginResponse> Login(LoginRequest loginDTO)
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
    }
}
