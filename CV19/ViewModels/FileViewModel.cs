using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CV19.ViewModels
{
    class FileViewModel
    {
        private FileInfo fileInfo;

        public string Name => fileInfo.Name;

        public FileViewModel(string path)
        {
            fileInfo = new FileInfo(path);
        }
    }
}
