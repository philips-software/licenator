using System;
using System.IO;

namespace Licenator
{
    public class DirectoryTraveler
    {
        public void Begin(string rootPath, Action<string> onEachFile)
        {
            TravelFolder(rootPath, onEachFile);
        }

        private void TravelFolder(string path, Action<string> onEachFile)
        {
            var dir = new DirectoryInfo(path);

            foreach (var fi in dir.GetFiles())
            {
                onEachFile(fi.FullName);
            }

            foreach (var di in dir.GetDirectories())
            {
                TravelFolder(di.FullName, onEachFile);
            }
        }
    }
}

