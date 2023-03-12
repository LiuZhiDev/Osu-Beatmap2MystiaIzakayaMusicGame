using MystiaIzakayaMusicGameConvert.数据;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security.Policy;

namespace 谱面转换器.数据.夜雀食堂
{
    [Serializable]
    public class 谱面数据
    {
        [JsonProperty("ChapterName")]
        public string 音乐名称 { get; set; } = "未定义名称";
        [JsonProperty("Maps")]
        public List<谱面信息> 谱面集 { get; set; } = new();
        [Serializable]
        public class 谱面信息
        {
            [JsonProperty("MapName")]
            public string 谱面名称 { get; set; } = "未定义名称";
            [JsonProperty("Original")]
            public string 艺术家 { get; set; } = "未定义名称";
            [JsonProperty("Mapper")]
            public string 谱师 { get; set; } = "未定义名称";
            [JsonProperty("Level")]
            public int 难度 { get; set; }
            [JsonProperty("Notes")]
            public List<元素> 元素集 { get; set; } = new();
            [JsonProperty("Clicks")]
            public List<节拍段落> 节拍段落 { get; set; } = new();
            [JsonProperty("CoverPicOffset")]
            public 专辑图片偏移 专辑图片偏移 { get; set; } = new();
            [JsonProperty("CoverPicBorderColor")]
            public 专辑边框色彩 专辑图片边框 { get; set; } = new();

        }
        [Serializable]
        public class 元素
        {
            [JsonProperty("startTime")]
            public int 出现时间 { get; set; }
            [JsonProperty("endTime")]
            public int 结束时间 { get; set; }
            [JsonProperty("noteType")]
            public string? 元素类型 { get; set; }
            [JsonProperty("positionType")]
            public string? 元素位置 { get; set; }

        }
        [Serializable]
        public class 专辑图片偏移
        {
            /// <summary>
            /// 横向偏移
            /// </summary>
            public int x { get; set; } = 0;
            /// <summary>
            /// 纵向偏移
            /// </summary>
            public int y { get; set; } = 0;
        }
        [Serializable]
        public class 专辑边框色彩
        {
            /// <summary>
            /// 红色通道亮度
            /// </summary>
            public float r { get; set; } = 0.5f;
            /// <summary>
            /// 绿色通道亮度
            /// </summary>
            public float g { get; set; } = 0.000000023f;
            /// <summary>
            /// 蓝色通道亮度
            /// </summary>
            public float b { get; set; } = 0f;
            /// <summary>
            /// 透明度
            /// </summary>
            public float a { get; set; } = 1f;
        }
        [Serializable]
        public class 节拍段落
        {
            public enum 节拍段落类型
            {
                /// <summary>
                /// 系统默认的节拍段落
                /// </summary>
                Custom,
                /// <summary>
                /// 玩家定义的节拍段落
                /// </summary>
                Normal
            }
            [JsonProperty("StartTime")]
            /// <summary>
            /// 节拍段落的开始时间
            /// </summary>
            public int 开始时间 { get; set; }
            [JsonProperty("Bpm")]
            /// <summary>
            /// BPM
            /// </summary>
            public float 节拍 { get; set; }
            [JsonProperty("Division")]
            /// <summary>
            /// 节拍的分量（这里仅作编辑时使用的网格分割数）
            /// </summary>
            public int 节拍分量 { get; set; }
            [JsonProperty("SpecificMidiClick")]
            /// <summary>
            /// 程序定义的未知变量
            /// </summary>
            public string[]? 特殊Midi节拍点 { get; set; } = null;
            [JsonProperty("Type")]
            /// <summary>
            /// 段落的类型
            /// </summary>
            public string 类型 { get; set; } = "Custom";

        }
    }
    public class 谱面集文件夹
    {
        [JsonProperty("chapterName")]
        public string 谱面集名称 { get; set; } = "未定义谱面集名称";
        [JsonProperty("musicMapFolderName")]
        public List<string> 谱面名称 { get; set; } = new();
    }
}
