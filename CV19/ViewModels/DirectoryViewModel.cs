using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace CV19.ViewModels
{
    class DirectoryViewModel
    {
        private DirectoryInfo directoryInfo;

        public IEnumerable<DirectoryViewModel> SubDirectories
        {
            get
            {
                try
                {
                    return directoryInfo
                        .EnumerateDirectories()
                        .Select(d => new DirectoryViewModel(d.FullName)).OrderBy(d => d.Name);
                }
                catch (UnauthorizedAccessException e)
                {
                    Debug.WriteLine(e.Message);
                }

                return Enumerable.Empty<DirectoryViewModel>();
            }
        }

        public IEnumerable<FileViewModel> Files
        {
            get
            {
                try
                {
                    return directoryInfo
                        .EnumerateFiles()
                        .Select(f => new FileViewModel(f.FullName)).OrderBy(f => f.Name);
                }
                catch (UnauthorizedAccessException e)
                {
                    Debug.WriteLine(e.Message);
                }

                return Enumerable.Empty<FileViewModel>();
            }
        }

        public string Name => directoryInfo.Name;

        public DirectoryViewModel(string path) => directoryInfo = new DirectoryInfo(path);
    }
}
