using ConsoleApp25.辅助类;
using MystiaIzakayaMusicGameConvert.UI;
using MystiaIzakayaMusicGameConvert.洗谱器;
using MystiaIzakayaMusicGameConvert.程序设定;
using Panuon.WPF;
using Panuon.WPF.UI;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using 谱面转换器.接口.转换器;
using 谱面转换器.数据.OSU;
using 谱面转换器.资源;
using 谱面转换器.辅助类;

namespace MystiaIzakayaMusicGameConvert
{
 
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class 转换器主界面 : Window
    {

        public static 转换器主界面 主界面 { get; set; } 
        public static UI信息 ui信息 { get; set; } = new();
        public UI命令 ui命令 { get; set; } = new();
        public 转换器主界面()
        {
            主界面 = this;
            InitializeComponent();
            DataContext = this;
            Loaded += 已加载窗口;
        }

        private void 已加载窗口(object sender, RoutedEventArgs e)
        {
      
            程序配置.加载程序配置();
            ui信息.自动清理工具 = 清洗器.雀食洗谱器集.Keys.ToList();

       
        }
#warning 临时加入，可能需重写
        private void 意图打开菜单(object sender, ContextMenuEventArgs e)
        {
            // 获取鼠标右键点击的元素
            var element = e.OriginalSource as FrameworkElement;
            // 如果元素是Border类型，则为其生成菜单
            if (element is Border border)
            {
                // 调用定义菜单方法，传入元素信息
                var menu = 自定义洗谱.定义菜单(border.Tag);
                // 设置Border的ContextMenu属性为生成的菜单
                border.ContextMenu = menu;
                // 显示菜单
                menu.IsOpen = true;
                // 阻止其他元素显示默认菜单
                e.Handled = true;
            }
        }

    }
}
