using System.Collections.Generic;
using 谱面转换器.接口.清洗器;
using 谱面转换器.接口.转换器;

namespace 谱面转换器.资源
{
    /// <summary>
    /// 配置程序所需要使用的文件路径
    /// </summary>
    internal class 路径
    {

        public static string OSU音频文件名 { get; set; } = "未定义音频文件名";
        public static string OSU背景图像 { get; set; } = "未定义背景图像名";
        public static string 雀食堂图像资源文件夹名 { get; set; } = "SpriteAssets";
        public static string 雀食堂音频资源文件夹名 { get; set; } = "AudioAssets";
        public static string 雀食堂谱面背景图像 { get; set; } = "SpriteAssets\\_MapCoverImage.png";
        public static string 雀食堂预览背景图像 { get; set; } = "SpriteAssets\\ChapterCoverImage.png";
        public static string 雀食堂谱面音频 { get; set; } = "AudioAssets\\_Audio.mp3";
        public static string 雀食堂预览音频 { get; set; } = "AudioAssets\\_PreviewAudio.mp3";
        public static string 雀食堂Json配置路径 { get; set; } = "ChapterData.json";
        public static string 雀食堂工程文件_谱面集_图片文件夹名 { get; set; } = "SpriteAssets";
        public static string 雀食堂工程文件_谱面集_图片路径 { get; set; } = "ChapterCoverImage.png";
        public static string 雀食堂工程文件_谱面集_谱面文件夹名 { get; set; } = "CustomMapData";
        public static string 雀食堂工程文件_谱面集_Json配置路径 { get; set; } = "ChapterData.json";
        public static string 雀食堂工程文件_子谱面_图片资源文件夹名 { get; set; } = "SpriteAssets";
        public static string 雀食堂工程文件_子谱面_音乐资源文件夹名 { get; set; } = "AudioAssets";
        public static string 雀食堂工程文件_子谱面_Json配置路径 { get; set; } = "MapData.json";
        public static string 雀食堂工程文件_子谱面_音乐CG文件名 { get; set; } = "CG#0.png";
        public static string 雀食堂工程文件_子谱面_专辑CG文件名 { get; set; } = "MapCoverImage.png";
        public static string 雀食堂工程文件_子谱面_谱面音频 { get; set; } = "Audio.wav";
        public static string 雀食堂工程文件_子谱面_预览音频 { get; set; } = "PreviewAudio.wav";
        public static string OSU基本信息配置节名 { get; set; } = "General";
        public static string OSU元数据配置节名 { get; set; } = "Metadata";
        public static string OSU难度配置节名 { get; set; } = "Difficulty";
        public static string OSU时间线配置节名 { get; set; } = "TimingPoints";
        public static string OSU元素配置节名 { get; set; } = "HitObjects";
        public static string 谱师名称 = System.Environment.UserName;
    }

    /// <summary>
    /// 配置在转换过程中配置文件的文本段
    /// </summary>
    public class 键值
    {
        public class OSU
        {
            public static string 音频文件路径键 = "AudioFilename";
            public static string 预览起始时间键 = "PreviewTime";
            public static string 模式键 = "Mode";
            public static string 音乐名称键 = "TitleUnicode";
            public static string 专辑键 = "Source";
            public static string 艺术家键 = "Artist";
            public static string 谱面ID键 = "BeatmapID";
            /// <remarks>
            /// 乘100即可得到滑条每拍滑条经过多少像素
            /// 1. 首先60/BPM，得到每拍时长
            /// 2. 该值乘100 得到每拍经过像素数
            /// 3. 得到像素总数
            /// 滑条长度400 => 1秒每拍 每拍经过100像素，共持续4秒
            /// 滑条长度900 => 0.5秒每拍即BPM为120，每拍经过300像素，共持续900/300=共要进行3拍*0.5秒的每拍长度=1.5秒执行完毕。
            /// 得出公式 像素总数/滑条基础速速倍率*（60/BPM）
            /// </remarks>
            public static string 滑条基础速度倍率 = "SliderMultiplier";
        }
    }

    /// <summary>
    /// 定义了可以对任意谱面进行转换的转换器
    /// </summary>
    /// <remarks>
    /// 如果你要定义一个谱面转换器，请到“转换器”命名空间中继承“雀食堂谱面转换接口”
    /// </remarks>
    public static class 转换器
    {
        /// <summary>
        /// 已经存在于程序中的转换器枚举
        /// </summary>
        /// <remarks>
        /// 如果你定义了一个谱面转换器，你需要在这里为它定义一个文本名称，然后加入“雀食转换器集”
        /// 程序将会读取这些名称，并在用户使用时调用。
        /// </remarks>
        public enum 转换器名
        {
            /// <summary>
            /// 转换Osu谱面
            /// </summary>
            Osu谱面转换
        }
        /// <summary>
        /// 已经定义好的转换器实例集合
        /// 调用此字典中的转换器可以转换谱面
        /// </summary>
        /// <remarks>
        /// 如果你定义了一个谱面转换器，你需要在“转换器名”枚举中为它定义一个文本名称，然后加入此处
        /// 程序将会读取这些名称，并在用户使用时调用。
        /// </remarks>
        public static Dictionary<转换器名, 雀食堂转谱接口> 雀食转换器集 = new();
    }

    /// <summary>
    /// 定义了谱面清洗器，因为并非所有的谱面转换后都能符合游玩条件，需要经过一系列清洗步骤
    /// 此处定义了一些清洗步骤
    /// </summary>
    /// <remarks>
    /// 如果你要定义一个谱面清洗器，再到“洗谱器”命名空间中继承“雀食堂铺面清洗接口”
    /// </remarks>
    public static class 清洗器
    {
        /// <summary>
        /// 已经存在于程序中的清洗器枚举
        /// </summary>
        /// <remarks>
        /// 如果你定义了一个谱面清洗器，你需要在这里为它定义一个文本名称，然后加入“雀食洗谱器集”
        /// 程序将会读取这些名称，并在用户使用时调用。
        /// </remarks>
        public enum 清洗器名
        {
            /// <summary>
            /// 一个基本的清洗器
            /// </summary>
            基本清洗器,
            /// <summary>
            /// 在每次执行导出时必然使用的清洗器
            /// </summary>
            最终格式化,
            /// <summary>
            /// 将所有的元素无论如何都转成单点
            /// </summary>
            无脑转为单点,
            /// <summary>
            /// 清理所有距离过近的元素
            /// </summary>
            清理所有过近元素,
            /// <summary>
            /// 还原所有更改
            /// </summary>
            还原所有更改
        }
        /// <summary>
        /// 已经定义好的洗谱器实例集合
        /// 调用此字典中的洗谱器可以清洗谱面
        /// </summary>
        /// <remarks>
        /// 如果你定义了一个谱面清洗器，你需要在“清洗器名”枚举中为它定义一个文本名称，然后加入此处
        /// 程序将会读取这些名称，并在用户使用时调用。
        /// </remarks>
        public static Dictionary<清洗器名, 雀食堂洗谱接口> 雀食洗谱器集 = new();
    }



}
