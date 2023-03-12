using System;
using System.Collections.Generic;

namespace 谱面转换器.数据.OSU
{
    /// <summary>
    /// Osz文件的数据，此类是一切数据的入口
    /// </summary>
    public class 文件组数据
    {
        /// <summary>
        /// Osz文件内存放的文件组
        /// </summary>
        public static List<string> 包文件组 { get; set; } = new();
        /// <summary>
        /// Osz文件内存放的谱面文件组（*.osz）
        /// </summary>
        public static List<string> 谱面文件组 { get; set; } = new();
        /// <summary>
        /// 解压osz文件存放的临时路径
        /// </summary>
        public static string 临时文件路径 { get; set; } = "";
        /// <summary>
        /// 读取到内存的OSU谱面信息
        /// </summary>
        public static List<谱面数据> 谱面组 { get; set; } = new();

    }
    /// <summary>
    /// 存放谱面的数据
    /// </summary>
    public class 谱面数据
    {
        public 基本 基本数据 { get; set; } = new();
        public 曲目元 曲目元数据 { get; set; } = new();
        public List<时间线> 时间线数据 { get; set; } = new();
        public List<元素> 元素集数据 { get; set; } = new();

        public class 基本
        {
            /// <summary>
            /// OSU的游戏模式
            /// </summary>
            public enum Osu游戏模式
            {
                /// <summary>
                /// 配置文件中为0
                /// </summary>
                所有,
                /// <summary>
                /// 配置文件中为1
                /// </summary>
                太鼓,
                /// <summary>
                /// 配置文件中为2
                /// </summary>
                接水果,
                /// <summary>
                /// 配置文件中为3
                /// </summary>
                马娘,
            }
            public static Dictionary<int, Osu游戏模式> 模式枚举字典 = new() { { 0, Osu游戏模式.所有 }, { 1, Osu游戏模式.太鼓 }, { 2, Osu游戏模式.接水果 }, { 3, Osu游戏模式.马娘 } };
            public static Osu游戏模式 模式转换为实际模式(int 模式枚举值)
            {
                return 模式枚举字典[模式枚举值];
            }
            public string 配置名称 { get; set; } = "读取失败";
            public string 音乐文件子路径 { get; set; } = "读取失败";
            public string 难度名称 { get; set; } = "读取失败";
            public Osu游戏模式 模式 { get; set; }
            public TimeSpan 曲目预览开始时间 { get; set; }

        }
        /// <summary>
        /// 曲目的元数据类
        /// </summary>
        public class 曲目元
        {
            public string 曲名 { get; set; } = "读取失败";
            public string 艺术家 { get; set; } = "读取失败";
            public string 专辑或来源 { get; set; } = "读取失败";
            public string 谱面ID { get; set; } = "读取失败";
            public string 图像路径 { get; set; } = "读取失败";
        }
        public class 难度
        {
            public double 基础滑条速度倍率 { get; set; } = -1;
            public double 每秒经过像素数 { get; set; }
        }

        /// <summary>
        /// OSU中的TimeLine
        /// </summary>
        public class 时间线
        {
            public TimeSpan 开始时间 { get; set; }
            public float BPM { get; set; }
            public float 每拍长度_毫秒 { get; set; }
            public int 节拍分量 { get; set; }
            public float SV_非继承时间线变化率 { get; set; }
            public int 滑条每秒经过像素数 { get; set; }
        }

        public class 元素
        {
            static Dictionary<int, 元素类型> 元素转换字典 = new Dictionary<int, 元素类型>()
            {
                    { 1, 元素类型.点触},
                    { 2, 元素类型.滑条},
                    { 8, 元素类型.转盘},
                    { 128, 元素类型.马娘长键},
            };
            static Dictionary<int, 元素声效> 声效转换字典 = new()
            {
                    { 0, 元素声效.普通},
                    { 2, 元素声效.哨声},
                    { 4, 元素声效.击䥽},
                    { 8, 元素声效.拍手}
            };
            public static 元素类型 从数字转换为元素类型(int 数字)
            {
                var 字节 = 辅助类.字节转换.从Int转Byte(数字);
                int 类型 = 99;
                if (字节[7 - 0] == '1') { 类型 = 1; }
                if (字节[7 - 1] == '1') { 类型 = 2; }
                if (字节[7 - 3] == '1') { 类型 = 8; }
                if (字节[7 - 7] == '1') { 类型 = 128; }

                return 元素转换字典[类型];
            }
            public static 元素声效 从数字转换为声效类型(int 数字)
            {
                if (!声效转换字典.ContainsKey(数字))
                {
                    return 声效转换字典[0];
                }
                return 声效转换字典[数字];

            }
            public enum 元素类型
            {
                /// <summary>
                /// 配置文件中为 1
                /// </summary>
                点触,
                /// <summary>
                /// 配置文件中为 2
                /// </summary>
                滑条,
                /// <summary>
                /// 配置文件中为 8
                /// </summary>
                转盘,
                /// <summary>
                /// 配置文件中为 128
                /// </summary>
                马娘长键
            }
            public enum 元素声效
            {
                /// <summary>
                /// 配置文件中为 0
                /// </summary>
                普通,
                /// <summary>
                /// 配置文件中为 2
                /// </summary>
                哨声,
                /// <summary>
                /// 配置文件中为 4
                /// </summary>
                击䥽,
                /// <summary>
                /// 配置文件中为 8
                /// </summary>
                拍手
            }

            public TimeSpan 出现时间 { get; set; }
            public TimeSpan 结束时间 { get; set; }
            public 元素类型 原元素类型 { get; set; }
            public 元素声效 原元素声效 { get; set; }
            public int 横向坐标 { get; set; }
            public int 纵向坐标 { get; set; }

        }

    }
}
