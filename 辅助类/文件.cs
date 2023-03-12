using System;
using System.IO;
using System.IO.Compression;
using System.Runtime.InteropServices;
using System.Windows;

namespace 谱面转换器.辅助类
{
    /// <summary>
    /// 提供了关于文件、选择文件、处理文件的相关操作
    /// </summary>
    public static class 文件
    {
        /// <summary>
        /// 解压一个ZIP文件，并获取这个目录
        /// </summary>
        /// <param name="ZIP文件路径">ZIP文件的路径</param>
        /// <returns>临时文件的目录</returns>
        public static string 解压Zip文件到临时目录(string ZIP文件路径)
        {
            var 临时文件目录 = Path.GetTempPath();
            临时文件目录 += new Random().Next(111085878, 190166665);
            Directory.CreateDirectory(临时文件目录);

            ZipFile.ExtractToDirectory(ZIP文件路径, 临时文件目录);
            return 临时文件目录;
        }

        /// <summary>
        /// 将指定目录压缩为Zip文件
        /// </summary>
        /// <param name="文件夹路径">文件夹路径</param>
        /// <param name="ZIP文件路径">目标路径</param>
        public static void 压缩文件夹(string 文件夹路径, string ZIP文件路径)
        {
            DirectoryInfo 新压缩文件信息 = new DirectoryInfo(ZIP文件路径);

            if (新压缩文件信息.Parent != null)
            {
                新压缩文件信息 = 新压缩文件信息.Parent;
            }

            if (!新压缩文件信息.Exists)
            {
                新压缩文件信息.Create();
            }
            if (File.Exists(ZIP文件路径))
            {
                var 真实文件名 = Path.GetFileNameWithoutExtension(ZIP文件路径);
                var 后缀名 = Path.GetExtension(ZIP文件路径);
                真实文件名 += DateTime.Now.ToString("dd.hh.mm.ss");
                var 新文件夹路径 = Path.GetDirectoryName(ZIP文件路径);
                ZIP文件路径 = 新文件夹路径 + 真实文件名 + 后缀名;
            }
            ZipFile.CreateFromDirectory(文件夹路径, ZIP文件路径, CompressionLevel.Optimal, false);
        }


        /// <summary>
        /// 完成文件浏览器与系统的交互操作
        /// </summary>
        public static class 文件定位器
        {
            [DllImport("shell32.dll")]
            static extern IntPtr ShellExecute(
            IntPtr hwnd,
            string lpOperation,
            string lpFile,
            string lpParameters,
            string lpDirectory,
            ShowCommands nShowCmd);
            private enum ShowCommands : int
            {
                SW_HIDE = 0,
                SW_SHOWNORMAL = 1,
                SW_NORMAL = 1,
                SW_SHOWMINIMIZED = 2,
                SW_SHOWMAXIMIZED = 3,
                SW_MAXIMIZE = 3,
                SW_SHOWNOACTIVATE = 4,
                SW_SHOW = 5,
                SW_MINIMIZE = 6,
                SW_SHOWMINNOACTIVE = 7,
                SW_SHOWNA = 8,
                SW_RESTORE = 9,
                SW_SHOWDEFAULT = 10,
                SW_FORCEMINIMIZE = 11,
                SW_MAX = 11
            }
            /// <summary>
            /// 打开系统的文件资源管理器，然后定位到某个特定的文件或文件夹
            /// </summary>
            /// <param name="完整路径"></param>
            public static void 定位文件(string 完整路径)
            {
                if (Directory.Exists(完整路径))
                {
                    ShellExecute(IntPtr.Zero, "open", "explorer.exe", 完整路径, "", ShowCommands.SW_NORMAL);
                    return;
                }

                ShellExecute(IntPtr.Zero, "open", "explorer.exe", @"/e,/select," + 完整路径, "", ShowCommands.SW_NORMAL);

            }
            /// <summary>
            /// 打开选择器，让用户选择某个特定的文件或文件夹
            /// </summary>
            /// <param name="初始目录"></param>
            /// <param name="选择文件类型"></param>
            /// <returns></returns>
            public static string 选择文件(string 初始目录, string 选择文件类型 = "图像文件(*.jpg)|*.jpg")
            {

                if (!Directory.Exists(初始目录)) { MessageBox.Show("找不到初始目录"); return ""; }
                Microsoft.Win32.OpenFileDialog 文件打开位置框 = new Microsoft.Win32.OpenFileDialog();
                文件打开位置框.InitialDirectory = 初始目录;
                文件打开位置框.Filter = 选择文件类型;
                文件打开位置框.AddExtension = true;
                文件打开位置框.DefaultExt = "jpg";
                文件打开位置框.ShowDialog();
                if (文件打开位置框.FileName == "" || 文件打开位置框.FileName == null) { return ""; }
                return 文件打开位置框.FileName;
            }
            /// <summary>
            /// 打开选择器，让用户选择文件保存位置
            /// </summary>
            /// <param name="初始目录"></param>
            /// <param name="可预览类型"></param>
            /// <param name="保存文件类型"></param>
            /// <returns></returns>
            public static string 保存文件(string 初始目录, string 可预览类型 = "图像文件(*.xlsx)|*.xlsx", string 保存文件类型 = "xlsx")
            {

                if (!Directory.Exists(初始目录)) { MessageBox.Show("找不到初始目录"); return ""; }
                Microsoft.Win32.SaveFileDialog 文件打开位置框 = new Microsoft.Win32.SaveFileDialog();
                文件打开位置框.InitialDirectory = 初始目录;
                文件打开位置框.Filter = 可预览类型;
                文件打开位置框.AddExtension = true;
                文件打开位置框.DefaultExt = 保存文件类型;
                文件打开位置框.ShowDialog();
                if (文件打开位置框.FileName == "" || 文件打开位置框.FileName == null) { return ""; }
                return 文件打开位置框.FileName;
            }

            /// <summary>
            /// 创建一个快捷方式
            /// </summary>
            /// <param name="lnkFilePath">快捷方式的完全限定路径。</param>
            /// <param name="workDir"></param>
            /// <param name="args">快捷方式启动程序时需要使用的参数。</param>
            /// <param name="targetPath"></param>
            public static void 创建新的快捷方式(string lnkFilePath, string targetPath, string workDir, string args = "")
            {
                var shellType = Type.GetTypeFromProgID("WScript.Shell");
                dynamic shell = Activator.CreateInstance(shellType!)!;
                var shortcut = shell!.CreateShortcut(lnkFilePath);
                shortcut.TargetPath = targetPath;
                shortcut.Arguments = args;
                shortcut.WorkingDirectory = workDir;
                shortcut.Save();
            }

            // 获取快捷方式目标路径
            public static readonly Guid CLSID_WshShell = new Guid("72C24DD5-D70A-438B-8A42-98424B88AFB8");
            /// <summary>
            /// 获得一个lnk快捷方式文件指向的具体路径
            /// </summary>
            /// <param name="lnk"></param>
            /// <returns></returns>
            public static string? 获得快捷方式路径(string lnk)
            {
                if (System.IO.File.Exists(lnk))
                {
                    dynamic objWshShell = null!, objShortcut = null!;
                    try
                    {
                        objWshShell = Activator.CreateInstance(Type.GetTypeFromCLSID(CLSID_WshShell)!)!;
                        objShortcut = objWshShell.CreateShortcut(lnk);
                        return objShortcut.TargetPath;
                    }
                    finally
                    {
                        Marshal.ReleaseComObject(objShortcut);
                        Marshal.ReleaseComObject(objWshShell);
                    }
                }
                return null;
            }

            /// <summary>
            /// 拼接程序目录下的文件Url
            /// </summary>
            /// <param name="相对路径字符串">以/开头的路径字符串</param>
            /// <returns></returns>
            public static string 拼接程序目录下的文件Url(string 相对路径字符串)
            {
                return Directory.GetCurrentDirectory() + 相对路径字符串;
            }

        }


    }

}
