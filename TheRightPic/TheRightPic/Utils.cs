using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace NowUSeeIt
{
    class Utils
    {
        public static async Task<bool> CheckFileExistAsync(StorageFolder folder, string filename)
        {
            try
            {
                StorageFile file = await folder.GetFileAsync(filename);
                return file != null;
            }
            catch
            {
                return false;
            }
        }
    }
}
