using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Service.Common
{
    public interface IStorageService
    {
        public string GetFileUrl(string fileName);
        Task SaveFileAsync(Stream mediaBinaryStream,string fileName);
        Task DeleteFileAsync(string fileName);
    }
}
