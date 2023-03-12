using ConsoleApp25.辅助类;
using Panuon.WPF;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using 谱面转换器.辅助类;
using 谱面转换器.资源;
using System.Windows.Controls;
using Panuon.WPF.UI;
using MystiaIzakayaMusicGameConvert.UI;
using System.Reflection;
using MystiaIzakayaMusicGameConvert.辅助类;
using MystiaIzakayaMusicGameConvert.数据;
using System.Threading.Tasks;
using System.Threading;

namespace MystiaIzakayaMusicGameConvert
{
    public class UI命令
    {
        public ICommand 关闭命令 { get; set; }
        public ICommand 最小化命令 { get; set; }
        public ICommand 最大化与还原命令 { get; set; }
        public ICommand 导入新谱面命令 { get; set; }
        public ICommand 输出谱面命令 { get; set; }
        public ICommand 输出工程命令 { get; set; }
        public ICommand 显示已删除元素命令 { get; set; }
        public ICommand 谱面清理命令 { get; set; }
        public ICommand 解决文件名过长命令 { get; set; }
        public UI命令()
        {
            关闭命令 = new RelayCommand(控件 => 关闭窗口());
            最小化命令 = new RelayCommand(控件 => 最小化窗口());
            最大化与还原命令 = new RelayCommand(控件 => 最大化或还原());
            导入新谱面命令 = new RelayCommand(控件 => 导入新谱面());
            输出谱面命令 = new RelayCommand(控件 => 输出谱面());
            谱面清理命令 = new RelayCommand(控件 => 谱面清理());
            输出工程命令 = new RelayCommand(控件 => 输出工程());
            解决文件名过长命令 = new RelayCommand(控件 => 解决文件名过长());
        }

        private void 解决文件名过长()
        {
            辅助类.网络.通过浏览器打开链接("https://www.jb51.net/os/win10/747737.html");
        }

        private void 输出工程()
        {
            运行时.导出方式 = 运行时.导出为.工程;
            Task 新线程 = new Task(() => 
            {
                //定位文件与判空逻辑
                if (数据.运行时.转谱结果 == null) { return; }
                var 文件路径 = 文件.文件定位器.保存文件(Directory.GetCurrentDirectory(), "压缩文档(*.zip)|*.zip");
                if (string.IsNullOrWhiteSpace(文件路径)) { return; }
                //循环所有谱面做基础清洗
                for (int i = 0; i < 数据.运行时.转谱结果.取得谱面信息组().谱面集.Count; i++)
                {
                    var 谱面 = 数据.运行时.转谱结果.取得谱面信息组().谱面集[i];
                    清洗器.雀食洗谱器集[清洗器.清洗器名.最终格式化].清洗谱面(ref 谱面);
                }
                //准备UI更新
                var 正在转换 = true;
                Task UI更新线程 = new Task(() => 
                {
                    数据.运行时.转谱结果.运行状态 = (0, "正在后台运行导出功能");
                    while (正在转换)
                    {
                        Application.Current.Dispatcher.Invoke(() => 
                        {
                            转换器主界面.ui信息.运行状态 = $"{数据.运行时.转谱结果.运行状态.状态解释} {数据.运行时.转谱结果.运行状态.总进度}%";
                        });
                        
                        Thread.Sleep(100);
                    }
                });
                //使用接口的导出功能
                UI更新线程.Start();
                数据.运行时.转谱结果.导出工程(文件路径);
                正在转换 = false;
                Application.Current.Dispatcher.Invoke(() =>
                {
                    数据.运行时.转谱结果.运行状态 = (100, "已完成导出");
                    转换器主界面.ui信息.运行状态 = "已导出";
                    MessageBox.Show($"谱面文件已经成功导出至\r\n{文件路径}\r\n请打开谱面编辑器，并将里面的文件夹复制到谱面编辑器的数据文件夹中", "导出成功");
                });
            });
            新线程.Start();
        
        }

        private void 输出谱面()
        {
            运行时.导出方式 = 运行时.导出为.谱面;
            if (数据.运行时.转谱结果 == null) { return; }
            var 文件路径 = 文件.文件定位器.保存文件(Directory.GetCurrentDirectory(), "夜雀食堂谱面文件(*.tmicpkg)|*.tmicpkg");
            if (string.IsNullOrWhiteSpace(文件路径)) { return; }
            for (int i = 0; i < 数据.运行时.转谱结果.取得谱面信息组().谱面集.Count; i++)
            {
                var 谱面 = 数据.运行时.转谱结果.取得谱面信息组().谱面集[i];
                清洗器.雀食洗谱器集[清洗器.清洗器名.最终格式化].清洗谱面(ref 谱面);
            }

            数据.运行时.转谱结果.导出谱面(文件路径);
            MessageBoxX.Show($"谱面文件已经成功导出至\r\n{文件路径}", "导出成功");
        }

        public static void 显隐已删除元素(bool 已勾选)
        {
            if (已勾选)
            {
                音符预览.被删除的元素集.ForEach(x => x.Visibility = Visibility.Visible);
            }
            else 
            {
                音符预览.被删除的元素集.ForEach(x => x.Visibility = Visibility.Collapsed);
            }
        }

        public static void 谱面清理()
        {
            if (数据.运行时.转谱结果 == null) { return; }
            if (转换器主界面.ui信息.自动清理工具选定下标 == -1) { return; }

            for (int i = 0; i < 数据.运行时.转谱结果.取得谱面信息组().谱面集.Count; i++)
            {
                var 谱面 = 数据.运行时.转谱结果.取得谱面信息组().谱面集[i];
                谱面 = 清洗器.雀食洗谱器集[转换器主界面.ui信息.自动清理工具[转换器主界面.ui信息.自动清理工具选定下标]].清洗谱面(ref 谱面);
                清洗器.雀食洗谱器集[转换器主界面.ui信息.自动清理工具[转换器主界面.ui信息.自动清理工具选定下标]].重置清洗();
            }

            音符预览.加载所有音符至预览窗(数据.运行时.转谱结果.取得谱面信息组().谱面集[转换器主界面.ui信息.谱面列表选定下标].元素集, 转换器主界面.主界面.显示格);
        }

        public static void 谱面选择()
        {
            if (数据.运行时.转谱结果 == null) { return; }
            音符预览.加载所有音符至预览窗(数据.运行时.转谱结果.取得谱面信息组().谱面集[转换器主界面.ui信息.谱面列表选定下标].元素集, 转换器主界面.主界面.显示格);
            转换器主界面.ui信息.元素集 = 数据.运行时.转谱结果.取得谱面信息组().谱面集[转换器主界面.ui信息.谱面列表选定下标].元素集;
        }

        private void 导入新谱面()
        {
            var 文件路径 = 文件.文件定位器.选择文件(Directory.GetCurrentDirectory(), "OSU谱面包(*.osz)|*.osz");
            if (string.IsNullOrWhiteSpace(文件路径)) { return; }
            
            数据.运行时.转谱结果 = API.执行转换(转换器.转换器名.Osu谱面转换, 文件路径);
            if (数据.运行时.转谱结果.取得谱面信息组().谱面集.FirstOrDefault() == null) { MessageBox.Show("转换失败"); }
            转换器主界面.ui信息.曲目名称 = 数据.运行时.转谱结果.取得谱面信息组().音乐名称;
            转换器主界面.ui信息.艺术家 = 数据.运行时.转谱结果.取得谱面信息组().谱面集.FirstOrDefault()!.艺术家;
            var 图像路径 = 谱面转换器.数据.OSU.文件组数据.谱面组.FirstOrDefault()!.曲目元数据.图像路径;
            转换器主界面.ui信息.图像 = 图像.从文件到BitmapImage(图像路径);
            转换器主界面.ui信息.谱面列表.RemoveAll(x => true);
            var a = 转换器主界面.主界面.谱面列表.Items.Count;
#warning 此处是临时修复的，一但不重新绑定源绑定会出现问题
            转换器主界面.主界面.谱面列表.ItemsSource = null;
            Debug.WriteLine(转换器主界面.ui信息.谱面列表.Count);
            数据.运行时.转谱结果.取得谱面信息组().谱面集.ForEach(x => 转换器主界面.ui信息.谱面列表.Add(x.谱面名称));
#warning 此处是临时修复的，一但不重新绑定源绑定会出现问题
            var dsa = 转换器主界面.主界面.谱面列表.Items.Count;
            转换器主界面.主界面.谱面列表.ItemsSource = 转换器主界面.ui信息.谱面列表;
            转换器主界面.ui信息.谱面信息 = $"已导入 {数据.运行时.转谱结果.取得谱面信息组().谱面集.Count} 个谱面";
            //需要还原调用

            转换器主界面.ui信息.谱面列表选定下标 = 0;
            //谱面列表.SelectedIndex = 0;
        }
        private void 最大化或还原()
        {
            var 主界面 = 转换器主界面.主界面;
            if (主界面.WindowState == WindowState.Maximized) { 主界面.WindowState = WindowState.Normal; return; }
            if (主界面.WindowState == WindowState.Normal) { 主界面.WindowState = WindowState.Maximized; return; }
        }
        private void 最小化窗口()
        {
            var 主界面 = 转换器主界面.主界面;
            主界面.WindowState = WindowState.Minimized;
        }
        private void 关闭窗口()
        {
            Application.Current.Shutdown();
        }
    }
}