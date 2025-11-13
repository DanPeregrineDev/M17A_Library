using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M17A_Library
{
    internal class Utilities
    {
        static public string ProgrammFolder(string Name)
        {
            string startFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            startFolder += @"\" + Name;

            if (System.IO.Directory.Exists(startFolder) == false)
            {
                System.IO.Directory.CreateDirectory(startFolder);
            }

            return startFolder;
        }
    }
}
