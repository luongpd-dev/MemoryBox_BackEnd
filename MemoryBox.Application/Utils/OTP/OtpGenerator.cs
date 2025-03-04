using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryBox.Application.Utils.OTP
{
    public static class OtpGenerator
    {
        // Phương thức tạo OTP 4 chữ số
        public static string GenerateOtp()
        {
            Random random = new Random();
            return random.Next(1000, 9999).ToString(); // Tạo mã OTP gồm 4 chữ số
        }
    }
}
