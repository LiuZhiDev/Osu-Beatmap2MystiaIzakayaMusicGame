using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MystiaIzakayaMusicGameConvert.辅助类
{
    public class 网络
    {
        public static bool 通过浏览器打开链接(String url)
        {
            if (string.IsNullOrEmpty(url)) { return false; }

#pragma warning disable CS8600 // 将 null 字面量或可能为 null 的值转换为非 null 类型。
            RegistryKey key = Registry.ClassesRoot.OpenSubKey(@"http\shell\open\command\");
            if (key == null) { return false; }
#pragma warning restore CS8600 // 将 null 字面量或可能为 null 的值转换为非 null 类型。
            String s = key.GetValue("")!.ToString()!;
            
            String? browserpath = null;
            if (s.StartsWith("\""))
            {
                browserpath = s.Substring(1, s.IndexOf('\"', 1) - 1);
            }
            else
            {
                browserpath = s.Substring(0, s.IndexOf(" "));
            }
            return System.Diagnostics.Process.Start(browserpath, url) != null;
        }
    }
}
