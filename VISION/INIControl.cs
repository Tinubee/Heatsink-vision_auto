using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace VISION
{
    public class INIControl
    {
        private string Path;

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        public INIControl(string Newpath)
        {
            if (Newpath.EndsWith("ini") == true)
            {
                Path = Newpath;
            }
        }

        public string ReadData(string Section, string Key, bool isnumbberic = false)
        {
            System.Text.StringBuilder Strings = new StringBuilder();
            int Errors;

            Errors = GetPrivateProfileString(Section, Key, "", Strings, 255, Path);

            if (Strings.ToString() == "")
            {
                if (isnumbberic == true)
                {
                    return "0";
                }
            }
            return Strings.ToString();
        }

        public bool ReadData_bool(string Section, string Key)
        {
            System.Text.StringBuilder Strings = new StringBuilder();
            int Errors;

            Errors = GetPrivateProfileString(Section, Key, "", Strings, 255, Path);

            if (Strings.ToString() == "" || Strings.ToString() == "0")
            {
                return false;
            }
            return true;
        }

        public void WriteData(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, Path);
        }

        public void DeleteKey(string Section, string Key)
        {
            WritePrivateProfileString(Section, Key, null, Path);
        }
    }
}
