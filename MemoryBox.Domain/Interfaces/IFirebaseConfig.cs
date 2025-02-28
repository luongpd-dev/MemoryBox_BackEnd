using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryBox.Domain.Interfaces
{
    public interface IFirebaseConfig
    {
        Task<string> UploadFiles(IFormFile file);

    }
}
