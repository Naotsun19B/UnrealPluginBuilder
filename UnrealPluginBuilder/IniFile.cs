using System;
using System.IO;
using System.Text;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace UnrealPluginBuilder
{
    class IniFile
    {
        [DllImport("kernel32.dll")]
        private static extern uint GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, uint nSize, string lpFileName);

        [DllImport("kernel32.dll")]
        private static extern uint GetPrivateProfileInt(string lpAppName, string lpKeyName, int nDefault, string lpFileName);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool WritePrivateProfileString(string lpAppName, string lpKeyName, string lpString, string lpFileName);

        public string FilePath { get; set; }

        public IniFile(string filePath)
        {
            FilePath = filePath;
        }

        public bool TryGetValueOrDefault<T>(string sectionName, string keyName, T defaultValue, out T outputValue)
        {
            outputValue = defaultValue;

            if (string.IsNullOrEmpty(FilePath) || !File.Exists(FilePath))
            {
                return false;
            }

            var sb = new StringBuilder(1024);
            var ret = GetPrivateProfileString(sectionName, keyName, string.Empty, sb, Convert.ToUInt32(sb.Capacity), FilePath);
            if (ret == 0 || string.IsNullOrEmpty(sb.ToString()))
            {
                return false;
            }

            var conv = TypeDescriptor.GetConverter(typeof(T));
            if (conv == null)
            {
                return false;
            }

            try
            {
                outputValue = (T)conv.ConvertFromString(sb.ToString());
            }
            catch (NotSupportedException)
            {
                return false;
            }
            catch (FormatException)
            {
                return false;
            }

            return true;
        }

        public T GetValueOrDefault<T>(string sectionName, string keyName, T defaultValue)
        {
            T ret = defaultValue;
            TryGetValueOrDefault(sectionName, keyName, defaultValue, out ret);
            return ret;
        }

        public void SetValue<T>(string sectionName, string keyName, T outputValue) =>
            WritePrivateProfileString(sectionName, keyName, outputValue.ToString(), FilePath);
    }
}
