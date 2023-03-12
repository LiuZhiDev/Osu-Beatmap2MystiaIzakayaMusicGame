using MystiaIzakayaMusicGameConvert.洗谱器;
using System.Windows.Media;
using 谱面转换器.数据.夜雀食堂;
using 谱面转换器.洗谱器;
using 谱面转换器.资源;
using static 谱面转换器.资源.转换器;

namespace MystiaIzakayaMusicGameConvert.程序设定
{
    /// <summary>
    /// 配置应用程序的显示、运行逻辑
    /// </summary>
    public static class 程序配置
    {
        /// <summary>
        /// 设置谱面预览窗口的上方元素离顶部的距离
        /// </summary>
        public static int 上方元素Y偏移 { get; set; } = 25;
        /// <summary>
        /// 设置谱面预览窗口的下方元素离顶部的距离
        /// </summary>
        public static int 下方元素Y偏移 { get; set; } = 85;
        /// <summary>
        /// 设置点触的颜色定义
        /// </summary>
        public static Color 点触元素 { get; set; } = Color.FromArgb(180, 74, 156, 214);
        /// <summary>
        /// 设置长按的颜色定义
        /// </summary>
        public static Color 长按元素 { get; set; } = Color.FromArgb(180, 149, 105, 255);
        /// <summary>
        /// 设置黄点的颜色定义
        /// </summary>
        public static Color 黄点元素 { get; set; } = Color.FromArgb(180, 58, 149, 131);

        /// <summary>
        /// 加载程序定义好的洗谱器与转换器
        /// 如果你定义了这些内容，请最好在此处统一实例化
        /// </summary>
        public static void 加载程序配置()
        {
            转换器.雀食转换器集.Add(转换器名.Osu谱面转换, new 从OSU转换到夜雀食堂());
            清洗器.雀食洗谱器集.Add(清洗器.清洗器名.基本清洗器, new 基本清洗());
            清洗器.雀食洗谱器集.Add(清洗器.清洗器名.无脑转为单点, new 全部转换为单点());
            清洗器.雀食洗谱器集.Add(清洗器.清洗器名.最终格式化, new 最终格式化());
            清洗器.雀食洗谱器集.Add(清洗器.清洗器名.清理所有过近元素, new 清理所有距离过近元素());
            清洗器.雀食洗谱器集.Add(清洗器.清洗器名.还原所有更改, new 撤销所有编辑());
        }

    }
}
