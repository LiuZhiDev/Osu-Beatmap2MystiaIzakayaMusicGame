using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Linq;
using 谱面转换器.接口.转换器;
using static 谱面转换器.接口.转换器.雀食堂转谱接口;
using static 谱面转换器.数据.夜雀食堂.谱面数据;

namespace MystiaIzakayaMusicGameConvert.UI
{
    public class 音符预览
    {

        public static List<Border> 被删除的元素集 { get; set; } = new();

        public static void 加载所有音符至预览窗(List<元素> 元素集, Canvas 元素框)
        {
            被删除的元素集.Clear();
            元素框.Children.Clear();
            int 最大偏移 = 0;
            foreach (var 元素 in 元素集)
            {
                var 起始位置 = 元素.出现时间 / 10f;
                var 宽度 = 元素.结束时间 / 10f - 元素.出现时间 / 10f;
                var 元素类型 = 元素.元素类型;
                if (宽度 == 0) { 宽度 = 10; }
                if (元素类型 == 雀食堂转谱接口.元素类型.Single.ToString() || 元素类型 == 雀食堂转谱接口.元素类型.HoldSingle.ToString())
                {
                    宽度 = 10;
                }
                var 元素位置 = 元素.元素位置;
                var Y偏移 = 0;
                var 颜色 = Color.FromArgb(128, 255, 0, 0);
                if (元素位置 == 雀食堂转谱接口.元素位置.Left.ToString()) { Y偏移 = 程序设定.程序配置.下方元素Y偏移; }
                if (元素位置 == 雀食堂转谱接口.元素位置.Right.ToString()) { Y偏移 = 程序设定.程序配置.上方元素Y偏移; }
                if (元素类型 == 雀食堂转谱接口.元素类型.Single.ToString()) { 颜色 = 程序设定.程序配置.点触元素; }
                if (元素类型 == 雀食堂转谱接口.元素类型.Hold.ToString()) { 颜色 = 程序设定.程序配置.长按元素; }
                if (元素类型 == 雀食堂转谱接口.元素类型.HoldSingle.ToString()) { 颜色 = 程序设定.程序配置.黄点元素; }

                var 边框 = 生成边框(元素, 起始位置, Y偏移, 宽度, 颜色);
                if (元素.出现时间 > 最大偏移 || 元素.结束时间 > 最大偏移)
                {
                    最大偏移 = 元素.结束时间 / 10 + (int)宽度;
                }
                
                if (元素类型 == null)
                {
                    被删除的元素集.Add(边框); 边框.Visibility = Visibility.Collapsed;
                }
                元素框.Children.Add(边框);
            }
            元素框.Width = 最大偏移;
        }
        public static Border 生成边框(object 元素信息, float X起始位置, float Y偏移, float 宽度, Color 颜色, int 高度 = 10, int 圆角 = 5)
        {

            Border 边框 = new Border()
            {

                Width = 宽度,
                Height = 高度,
                Background = new SolidColorBrush(颜色),
                CornerRadius = new CornerRadius(圆角),
                

            };
            边框.Tag =  new List<object> { 元素信息, 边框 };
            边框.MouseEnter += 进入边框;
            边框.MouseLeave += 退出边框; ;
            Canvas.SetLeft(边框, X起始位置);
            Canvas.SetTop(边框, Y偏移);
            return 边框;
        }

        private static void 退出边框(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var 边框 = (Border)sender;
            var 元素 = (元素)((List<object>)边框.Tag)[0];
            if(元素.元素类型 == 元素类型.Hold.ToString()) 
            {
                边框.Background = new SolidColorBrush( MystiaIzakayaMusicGameConvert.程序设定.程序配置.长按元素);
            }
            if (元素.元素类型 == 元素类型.HoldSingle.ToString())
            {
                边框.Background = new SolidColorBrush(MystiaIzakayaMusicGameConvert.程序设定.程序配置.黄点元素);
            }
            if (元素.元素类型 == 元素类型.Single.ToString())
            {
                边框.Background = new SolidColorBrush(MystiaIzakayaMusicGameConvert.程序设定.程序配置.点触元素);
            }

        }

        private static void 进入边框(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var 边框 = (Border)sender;
            var 元素 = (元素)((List<object>)边框.Tag)[0];
            转换器主界面.主界面.数据表单.SelectedItem = 元素;
            转换器主界面.主界面.数据表单.ScrollIntoView(元素);
            边框.Background = new SolidColorBrush(Colors.Brown);
            
        }

        public static void 更换单个音符至预览窗(Border 原元素, 元素 元素, Canvas 元素框)
        {
            元素框.Children.Remove(原元素);

            var 起始位置 = 元素.出现时间 / 10f;
            var 宽度 = 元素.结束时间 / 10f - 元素.出现时间 / 10f;
            var 元素类型 = 元素.元素类型;
            if (宽度 == 0) { 宽度 = 10; }
            if (元素类型 == 雀食堂转谱接口.元素类型.Single.ToString() || 元素类型 == 雀食堂转谱接口.元素类型.HoldSingle.ToString())
            {
                宽度 = 10;
            }
            var 元素位置 = 元素.元素位置;
            var Y偏移 = 0;
            var 颜色 = Color.FromArgb(128, 255, 0, 0);
            if (元素位置 == 雀食堂转谱接口.元素位置.Left.ToString()) { Y偏移 = 程序设定.程序配置.上方元素Y偏移; }
            if (元素位置 == 雀食堂转谱接口.元素位置.Right.ToString()) { Y偏移 = 程序设定.程序配置.下方元素Y偏移; }
            if (元素类型 == 雀食堂转谱接口.元素类型.Single.ToString()) { 颜色 = 程序设定.程序配置.点触元素; }
            if (元素类型 == 雀食堂转谱接口.元素类型.Hold.ToString()) { 颜色 = 程序设定.程序配置.长按元素; }
            if (元素类型 == 雀食堂转谱接口.元素类型.HoldSingle.ToString()) { 颜色 = 程序设定.程序配置.黄点元素; }
            var 边框 = 生成边框(元素, 起始位置, Y偏移, 宽度, 颜色);
            if (元素类型 == null)
            {
                被删除的元素集.Add(边框); 边框.Visibility = Visibility.Collapsed;
            }
            元素框.Children.Add(边框);

        }

    }
}
