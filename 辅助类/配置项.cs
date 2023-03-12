using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace 谱面转换器.辅助类
{
    /// <summary>
    /// 管理关于配置文件（.ini）的读写
    /// </summary>
    public static class 配置文件
    {
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, int nSize, string lpFileName);


        [DllImport("kernel32")]
        private static extern int WritePrivateProfileString(string lpApplicationName, string lpKeyName, string lpString, string lpFileName);

        /// <summary>
        ///读取一个配置项，如果这个配置项中的值不存在返回一个默认值
        /// </summary>
        /// <param name="配置节名">配置节名</param>
        /// <param name="配置项名">配置项名</param>
        /// <param name="默认返回">如果这个配置项中的值不存在返回一个默认值</param>
        /// <param name="配置项文件">指定一个配置项的文件路径</param>
        /// <returns></returns>
        public static string 读配置项(string 配置项文件, string 配置节名, string 配置项名, string 默认返回)
        {
            if (File.Exists(配置项文件))
            {
                var pat = System.IO.Path.GetDirectoryName(配置项文件);
                if (string.IsNullOrWhiteSpace(pat)) { pat = System.IO.Directory.GetCurrentDirectory(); 配置项文件 = pat + $"\\{配置项文件}"; }
                StringBuilder temp = new StringBuilder(1024);
                GetPrivateProfileString(配置节名, 配置项名, 默认返回, temp, 1024, 配置项文件);
                Console.WriteLine("temp" + temp.Length + " " + temp.ToString());
                return temp.ToString();
            }
            else
            {
                return 默认返回;
            }
        }

        /// <summary>
        /// 写配置项，返回True或者Flase
        /// </summary>
        /// <param name="配置项路径">输入目标文件路径</param>
        /// <param name="配置节名">配置节名 [ ]内的文字</param>
        /// <param name="配置项名">配置项名 = 前的文字</param>
        /// <param name="值">配置值 = 后的文字</param>
        /// <returns></returns>
        public static bool 写配置项(string 配置项路径, string 配置节名, string 配置项名, string 值)
        {
            var pat = System.IO.Path.GetDirectoryName(配置项路径);
            if (string.IsNullOrWhiteSpace(pat)) { pat = System.IO.Directory.GetCurrentDirectory(); 配置项路径 = pat + $"\\{配置项路径}"; }
            if (Directory.Exists(pat) == false)
            {
                Directory.CreateDirectory(pat);
            }
            if (File.Exists(配置项路径) == false)
            {
                File.Create(配置项路径).Close();
            }
            long OpStation = WritePrivateProfileString(配置节名, 配置项名, 值, 配置项路径);
            if (OpStation == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        /// <summary>
        /// 写配置项，返回True或者Flase
        /// </summary>
        /// <param name="配置项路径">输入目标文件路径</param>
        /// <param name="配置节名">配置节名 [ ]内的文字</param>
        /// <param name="配置项名">配置项名 = 前的文字</param>
        /// <param name="值">配置值 = 后的数字</param>
        /// <returns></returns>
        public static bool 写配置项(string 配置项路径, string 配置节名, string 配置项名, int 值)
        {
            var pat = System.IO.Path.GetDirectoryName(配置项路径);
            if (Directory.Exists(pat) == false)
            {
                Directory.CreateDirectory(pat!);
            }
            if (File.Exists(配置项路径) == false)
            {
                File.Create(配置项路径).Close();
            }
            long OpStation = WritePrivateProfileString(配置节名, 配置项名, Convert.ToString(值), 配置项路径);
            if (OpStation == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 抽取一个配置项中的配置节，获取其中所有的配置项与其的值
        /// </summary>
        /// <param name="配置文件路径"></param>
        /// <param name="配置节名"></param>
        /// <returns></returns>
        public static string 读配置项节文本(string 配置文件路径, string 配置节名)
        {
            string 一段文本 = File.ReadAllText(配置文件路径);
            string 正则表达式 = String.Format(@"\[{0}\]\s([\s\S]+?)\[", 配置节名);
            return 正则子匹配文本(一段文本, 正则表达式, 0, 0);

        }

        /// <summary>
        /// 抽取一个配置项中的配置节，获取其中所有的配置项与其的值
        /// </summary>
        /// <param name="配置文件路径"></param>
        /// <param name="配置节名"></param>
        /// <returns></returns>
        public static string 读内存配置项节文本(string 配置文本, string 配置节名)
        {
            string 正则表达式 = String.Format(@"\[{0}\]\s([\s\S]+?)\[", 配置节名);

            var 结果 = 正则子匹配文本(配置文本, 正则表达式, 0, 0);
            if (结果 == "Error")
            {
                正则表达式 = String.Format(@"{0}\][\s\S]([\s\S]+)", 配置节名);
                结果 = 正则子匹配文本(配置文本, 正则表达式, 0, 0);
            }
            return 结果;

        }

        /// <summary>
        /// 返回正则符合正则表达式，并且匹配后的子文本项
        /// </summary>
        /// <param name="一段文本">原文本</param>
        /// <param name="正则表达式">正则表达式</param>
        /// <param name="匹配">匹配索引</param>
        /// <param name="子匹配">子匹配索引</param>
        /// <returns>返回0为匹配空</returns>
        public static string 正则子匹配文本(string 一段文本, string 正则表达式, int 匹配, int 子匹配)
        {

            Regex 匹配器 = new Regex(正则表达式);
            var 匹配集 = 匹配器.Matches(一段文本);
            if (匹配集.Count == 0)
            { return "Error"; }
            return 匹配集[匹配].Groups[子匹配 + 1].Value;

        }


    }
    public static class 正则
    {

        /// <summary>
        /// 返回正则符合正则表达式，并且匹配后的子文本项
        /// </summary>
        /// <param name="一段文本">原文本</param>
        /// <param name="正则表达式">正则表达式</param>
        /// <param name="匹配">匹配索引</param>
        /// <param name="子匹配">子匹配索引</param>
        /// <returns>返回0为匹配空</returns>
        public static string 正则子匹配文本(string 一段文本, string 正则表达式, int 匹配, int 子匹配)

        {

            Regex 匹配器 = new Regex(正则表达式);
            var 匹配集 = 匹配器.Matches(一段文本);
            if (匹配集.Count == 0)
            { return "Error"; }
            return 匹配集[匹配].Groups[子匹配 + 1].Value;

        }

        /// <summary>
        /// 返回正则表达式的文本子匹配数量
        /// </summary>
        /// <param name="一段文本">输入一段文本</param>
        /// <param name="正则表达式">输入一个正则表达式</param>
        /// <returns></returns>
        public static int 正则子匹配数量(string 一段文本, string 正则表达式, int 匹配)

        {

            Regex 匹配器 = new Regex(正则表达式);
            var 匹配集 = 匹配器.Matches(一段文本);
            return 匹配集[匹配].Groups.Count;


        }


        /// <summary>
        /// 返回正则通过正则匹配的文本
        /// </summary>
        /// <param name="一段文本">输入一段文本</param>
        /// <param name="正则表达式">输入一个正则表达式</param>
        /// <param name="匹配索引">正则匹配索引</param>
        /// <returns></returns>
        public static string 正则匹配文本(string 一段文本, string 正则表达式, int 匹配索引)

        {
            //正则创建
            var a = Regex.Matches(一段文本, 正则表达式);
            //表示获取第一个正则匹配索引
            return a[匹配索引].ToString();
        }


        /// <summary>
        /// 返回正则匹配的数量（次数）
        /// </summary>
        /// <param name="一段文本">输入一段文本</param>
        /// <param name="正则表达式">输入一个正则表达式</param>
        /// <returns></returns>
        public static int 正则匹配数量(string 一段文本, string 正则表达式)

        {
            //正则创建
            var a = Regex.Matches(一段文本, 正则表达式);
            //正则匹配数量
            return a.Count;

        }

    }
}
