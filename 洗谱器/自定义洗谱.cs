using MystiaIzakayaMusicGameConvert.UI;
using MystiaIzakayaMusicGameConvert.数据;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using 谱面转换器.数据.夜雀食堂;
using static 谱面转换器.接口.转换器.雀食堂转谱接口;

namespace MystiaIzakayaMusicGameConvert.洗谱器
{
    /// <summary>
    /// 用户通过主动操作修改谱面的操作
    /// </summary>
    public static class 自定义洗谱
    {
        /// <summary>
        /// 生成用户可操作的菜单
        /// </summary>
        /// <param name="元素信息">传入要操作的元素</param>
        /// <returns></returns>
        public static ContextMenu 定义菜单(object 元素信息)
        {
            ContextMenu 菜单 = new ContextMenu();
            var 删除 = new MenuItem() { Header = "删除元素" };
            删除.Click += 自定义洗谱.删除该元素; 菜单.Items.Add(删除);
            删除.Tag = 元素信息;
            var 定义为点触 = new MenuItem() { Header = "转换为点触" };
            定义为点触.Click += 自定义洗谱.定义为点触; 菜单.Items.Add(定义为点触);
            定义为点触.Tag = 元素信息;
            var 定义为滑条 = new MenuItem() { Header = "转换为滑条" };
            定义为滑条.Click += 自定义洗谱.定义为滑条; 菜单.Items.Add(定义为滑条);
            定义为滑条.Tag = 元素信息;
            var 定义为黄点 = new MenuItem() { Header = "定义为黄点" };
            定义为黄点.Click += 自定义洗谱.定义为黄点; 菜单.Items.Add(定义为黄点);
            定义为黄点.Tag = 元素信息;
            var 转换手位 = new MenuItem() { Header = "转换手位" };
            转换手位.Click += 自定义洗谱.转换手位; 菜单.Items.Add(转换手位);
            转换手位.Tag = 元素信息;
            return 菜单;
        }

        #region 事件定义 
        public static void 删除该元素(object sender, RoutedEventArgs e)
        {
            var 控件 = 拆包(sender);
            删除该元素(控件.元素);
            更新元素状态(控件.元素, 控件.边框);
        }
        public static void 定义为点触(object sender, RoutedEventArgs e)
        {
            var 控件 = 拆包(sender);
            定义为点触(控件.元素);
            更新元素状态(控件.元素, 控件.边框);
        }
        public static void 定义为滑条(object sender, RoutedEventArgs e)
        {
            var 控件 = 拆包(sender);
            定义为滑条(控件.元素);
            更新元素状态(控件.元素, 控件.边框);
        }
        public static void 定义为黄点(object sender, RoutedEventArgs e)
        {
            var 控件 = 拆包(sender);
            定义为黄点(控件.元素);
            更新元素状态(控件.元素, 控件.边框);
        }
        public static void 转换手位(object sender, RoutedEventArgs e)
        {
            var 控件 = 拆包(sender);
            转换手位(控件.元素);
            更新元素状态(控件.元素, 控件.边框);
        }

        public static (谱面数据.元素 元素, Border 边框) 拆包(object sender)
        {
            var 控件 = (MenuItem)sender;
            var 包 = ((List<object>)控件.Tag);
            var 元素 = (谱面数据.元素)包[0];
            var 边框 = (Border)包[1];
            return (元素, 边框);
        }
        #endregion
        /// <summary>
        /// 将在导出时删除这个元素
        /// </summary>
        /// <param name="元素"></param>
        public static void 删除该元素(谱面数据.元素 元素)
        {
            元素.元素类型 = null;
        }
        /// <summary>
        /// 将该元素转换为单点
        /// </summary>
        /// <param name="元素"></param>
        public static void 定义为点触(谱面数据.元素 元素)
        {
            元素.元素类型 = 元素类型.Single.ToString();
        }
        /// <summary>
        /// 将该元素转换为滑条
        /// </summary>
        /// <param name="元素"></param>
        public static void 定义为滑条(谱面数据.元素 元素)
        {
            元素.元素类型 = 元素类型.Hold.ToString();
        }
        /// <summary>
        /// 将该元素转换为单点
        /// </summary>
        /// <param name="元素"></param>
        public static void 定义为黄点(谱面数据.元素 元素)
        {
            元素.元素类型 = 元素类型.HoldSingle.ToString();
        }
        /// <summary>
        /// 使该元素换手点按
        /// </summary>
        /// <param name="元素"></param>
        public static void 转换手位(谱面数据.元素 元素)
        {
            if (元素.元素位置 == 元素位置.Left.ToString()) { 元素.元素位置 = 元素位置.Right.ToString(); return; }
            if (元素.元素位置 == 元素位置.Right.ToString()) { 元素.元素位置 = 元素位置.Left.ToString(); return; }
        }

        public static void 更新元素状态(谱面数据.元素 元素, Border 边框)
        {
            音符预览.更换单个音符至预览窗(边框, 元素, 转换器主界面.主界面.显示格);
            转换器主界面.ui信息.元素集 = new();
            #warning 临时加的代码，为了刷新而刷新，可能导致页面卡顿
            if (数据.运行时.转谱结果 == null) { return; }
            转换器主界面.ui信息.元素集 = 数据.运行时.转谱结果.取得谱面信息组().谱面集[转换器主界面.ui信息.谱面列表选定下标].元素集;
        }
    }
}
