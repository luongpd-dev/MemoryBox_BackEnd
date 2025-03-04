using MemoryBox.Domain.Entities;
using MemoryBox.Domain.Interfaces;
using MemoryBox.Domain.Utils.VNPay;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace MemoryBox.Application.Services
{
    public class VNPayService : IVNPayService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IUnitOfWork _unitOfWork;

        public VNPayService(IConfiguration configuration, IHttpContextAccessor contextAccessor, IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _contextAccessor = contextAccessor;
            _unitOfWork = unitOfWork;
        }

        public string CreatePaymentUrl(Guid accountId, Guid planId, decimal amount)
        {
            var vnpay = new VnPayLibrary();
            string vnp_TmnCode = _configuration["VnPay:TmnCode"];
            string vnp_HashSecret = _configuration["VnPay:HashSecret"];
            string vnp_Url = _configuration["VnPay:BaseUrl"];
            string vnp_ReturnUrl = _configuration["VnPay:CallbackUrl"];

            vnpay.AddRequestData("vnp_Version", "2.1.0");
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            vnpay.AddRequestData("vnp_Amount", (amount * 100).ToString());
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_TxnRef", Guid.NewGuid().ToString());
            vnpay.AddRequestData("vnp_OrderInfo", $"Thanh toan goi {planId} cho tai khoan {accountId}");
            vnpay.AddRequestData("vnp_Locale", "vn");
            vnpay.AddRequestData("vnp_ReturnUrl", vnp_ReturnUrl);
            vnpay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));

            return vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
        }

        public async Task<bool> ProcessPaymentCallback(IQueryCollection queryParameters)
        {
            var vnpay = new VnPayLibrary();
            string vnp_HashSecret = _configuration["VnPay:HashSecret"];

            foreach (var key in queryParameters.Keys)
            {
                vnpay.AddResponseData(key, queryParameters[key]);
            }

            string vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
            string vnp_TransactionNo = vnpay.GetResponseData("vnp_TransactionNo");
            string vnp_TxnRef = vnpay.GetResponseData("vnp_TxnRef");
            string vnp_SecureHash = vnpay.GetResponseData("vnp_SecureHash");

            if (vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret) && vnp_ResponseCode == "00")
            {
                var transaction = new PaymentTransaction
                {
                    PaymentTransactionId = Guid.NewGuid(),
                    AccountId = Guid.Parse(vnp_TxnRef.Split('-')[0]),
                    SubscriptionPlanId = Guid.Parse(vnp_TxnRef.Split('-')[1]),
                    Amount = decimal.Parse(vnpay.GetResponseData("vnp_Amount")) / 100,
                    Status = "Success",
                    CreatedAt = DateTime.UtcNow
                };

                await _unitOfWork.PaymentTransactionRepository.InsertAsync(transaction);
                await _unitOfWork.SaveAsync();

                return true;
            }
            return false;
        }
    }
}
