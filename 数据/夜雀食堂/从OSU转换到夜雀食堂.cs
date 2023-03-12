using MystiaIzakayaMusicGameConvert.辅助类;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using 谱面转换器.接口.转换器;
using 谱面转换器.数据.OSU;
using static 谱面转换器.数据.夜雀食堂.谱面数据;

namespace 谱面转换器.数据.夜雀食堂
{
    /// <summary>
    /// 从OSU的谱面文件转换到雀食堂
    /// </summary>
    internal class 从OSU转换到夜雀食堂 : 雀食堂转谱接口
    {
        /// <summary>
        /// 存放一个雀食堂谱面数据的实例
        /// </summary>
        public 夜雀食堂.谱面数据 数据 { get; set; } = new();
        public (int 总进度, string 状态解释) 运行状态 { get; set; } = (0, "");

        夜雀食堂.谱面数据 原始数据 = new 谱面数据();



        #region 接口必须实现的方法
        public void 转换(string 源文件路径)
        {
            OSU.OSU数据读写器 数据读写 = new OSU数据读写器(源文件路径);
            数据 = new();
            读取谱面集基础数据();
            加载每个谱面的数据();
            原始数据 = 深复制.拷贝类(数据);
        }
        public 谱面数据 取得谱面信息组()
        {
            return 数据;
        }
        public 谱面数据 取得原始信息组()
        {
            return 原始数据;
        }
        public 谱面数据 还原原始信息组()
        {
            数据 = 深复制.拷贝类(原始数据);
            return 数据;
        }
        public void 导出谱面(string 文件位置)
        {
            var 主临时文件目录 = Path.GetTempPath();
            主临时文件目录 += new Random().Next(11085878, 90166665);
            Directory.CreateDirectory(主临时文件目录);
            主临时文件目录 += "\\";
            var 临时文件目录 = $"{主临时文件目录}{DateTime.Now.ToString("MM.dd.HH.mm.ss")}\\";
            Directory.CreateDirectory(临时文件目录 + 资源.路径.雀食堂图像资源文件夹名);
            Directory.CreateDirectory(临时文件目录 + 资源.路径.雀食堂音频资源文件夹名);
            if (文件组数据.谱面组.FirstOrDefault() == null) { return; }
            var osu谱面组 = 文件组数据.谱面组.FirstOrDefault();
            File.Copy(osu谱面组!.曲目元数据.图像路径, 临时文件目录 + 资源.路径.雀食堂谱面背景图像);
            File.Copy(osu谱面组.曲目元数据.图像路径, 临时文件目录 + 资源.路径.雀食堂预览背景图像);
            File.Copy(文件组数据.临时文件路径 + "\\" + osu谱面组.基本数据.音乐文件子路径, 临时文件目录 + 资源.路径.雀食堂谱面音频);
            File.Copy(文件组数据.临时文件路径 + "\\" + osu谱面组.基本数据.音乐文件子路径, 临时文件目录 + 资源.路径.雀食堂预览音频);
            string JSON = JsonConvert.SerializeObject(数据);
            JObject jObject = JObject.Parse(JSON);
            JArray maps = (JArray)jObject.SelectToken("Maps");
            foreach (var map in maps.Children())
            {
                JObject mapObject = (JObject)map;
                JArray clicks = (JArray)mapObject.SelectToken("Clicks");
                clicks.Clear();
            }
            JSON = jObject.ToString();
            JSON = Json关键字替换(JSON, 替换类型.导出谱面);
            File.WriteAllText(临时文件目录 + "ChapterData.json", JSON);
            辅助类.文件.压缩文件夹(主临时文件目录, 文件位置);
        }
        public 雀食堂转谱接口.元素位置 夜雀食堂元素位置转换器(object[] 信息集)
        {
            int osu元素横坐标 = (int)信息集[0]; int osu元素纵坐标 = (int)信息集[1];
            if (osu元素横坐标 > 300)
            {
                return 雀食堂转谱接口.元素位置.Right;
            }
            else
            {
                return 雀食堂转谱接口.元素位置.Left;
            }

        }
        public 雀食堂转谱接口.元素类型 夜雀食堂元素类型转换器(object[] 信息集)
        {
            OSU.谱面数据.元素.元素类型 osu元素类型 = (OSU.谱面数据.元素.元素类型)信息集[0];
            OSU.谱面数据.元素.元素声效 osu元素声效 = (OSU.谱面数据.元素.元素声效)信息集[1];

            if (osu元素类型 == OSU.谱面数据.元素.元素类型.点触)
            {
                return 雀食堂转谱接口.元素类型.Single;
            }

            if (osu元素类型 == OSU.谱面数据.元素.元素类型.滑条 || osu元素类型 == OSU.谱面数据.元素.元素类型.马娘长键)
            {
                return 雀食堂转谱接口.元素类型.Hold;
            }
            if (osu元素类型 == OSU.谱面数据.元素.元素类型.转盘)
            {
                return 雀食堂转谱接口.元素类型.Hold;
            }
            return 雀食堂转谱接口.元素类型.Single;
        }

        #endregion
        public enum 替换类型
        {
            导出工程,
            导出谱面
        }

        /// <summary>
        /// 将Json中的敏感、不能识别的字词进行替换，目前未使用
        /// </summary>
        /// <param name="jSON"></param>
        /// <returns></returns>
        private string Json关键字替换(string jSON, 替换类型 替换类型)
        {
            if (替换类型 == 替换类型.导出工程)
            {
                jSON = jSON.Replace("MapName", "MusicDisplayName");
                jSON = jSON.Replace("Clicks", "NoteClick");
            }
            return jSON;
        }

        /// <summary>
        /// 读取每一个OSU谱面并转换为雀食堂的格式
        /// </summary>
        private void 加载每个谱面的数据()
        {
            foreach (var Osu谱面 in OSU.文件组数据.谱面组)
            {
                谱面信息 雀食堂谱面 = new 谱面信息();
                雀食堂谱面.谱面名称 = Osu谱面.基本数据.难度名称;
                雀食堂谱面.谱师 = 资源.路径.谱师名称;
                雀食堂谱面.难度 = 6;//此处需要计算具体的难度
                雀食堂谱面.艺术家 = Osu谱面.曲目元数据.艺术家;
                加载所有时间线(雀食堂谱面.节拍段落, Osu谱面);
                加载所有游戏元素(雀食堂谱面.元素集, Osu谱面.元素集数据);
                数据.谱面集.Add(雀食堂谱面);
            }
        }

        private void 加载所有时间线(List<节拍段落> 雀食堂谱面, OSU.谱面数据 osu谱面)
        {
            foreach (var osu时间线 in osu谱面.时间线数据)
            {
                //忽略掉所有非继承时间点
                if (osu时间线.SV_非继承时间线变化率 != 100)
                {
                    continue;
                }
                var 时间线开始位置 = osu时间线.开始时间;
                var 节拍分量 = osu时间线.节拍分量;
                var BPM信息 = osu时间线.BPM;
                雀食堂谱面.Add(new 节拍段落()
                {
                    开始时间 = (int)时间线开始位置.TotalMilliseconds,
                    节拍 = BPM信息,
                    节拍分量 = 节拍分量,
                     特殊Midi节拍点= null,
                      类型 = 节拍段落.节拍段落类型.Normal.ToString()
                }) ;
            }
        }

        /// <summary>
        /// 转换所有的游戏内元素
        /// </summary>
        /// <param name="雀食堂谱面"></param>
        /// <param name="osu谱面"></param>
        private void 加载所有游戏元素(List<元素> 雀食堂谱面, List<OSU.谱面数据.元素> osu谱面)
        {

            foreach (var osu元素 in osu谱面)
            {
                
                var osu谱面位置信息 = new object[] { osu元素.横向坐标, osu元素.纵向坐标 };
                var osu谱面类型信息 = new object[] { osu元素.原元素类型, osu元素.原元素声效 };
                
                var 元素位置 = 夜雀食堂元素位置转换器(osu谱面位置信息);
                var 元素类型 = 夜雀食堂元素类型转换器(osu谱面类型信息);
                var 起始时间 = osu元素.出现时间;
                var 结束时间 = osu元素.结束时间;

                雀食堂谱面.Add(new 元素()
                {
                    出现时间 = (int)起始时间.TotalMilliseconds,
                    结束时间 = (int)结束时间.TotalMilliseconds,
                    元素位置 = 元素位置.ToString(),
                    元素类型 = 元素类型.ToString()
                });

            }
            雀食堂谱面.OrderBy(x => x.出现时间);
        }
        /// <summary>
        /// 转换谱面的基础数据集
        /// </summary>
        private void 读取谱面集基础数据()
        {
            if (OSU.文件组数据.谱面组 == null) { return; }
            数据.音乐名称 = OSU.文件组数据.谱面组.FirstOrDefault()!.曲目元数据.曲名;
        }

        public void 导出工程(string 文件位置)
        {
            运行状态 = (1, "正在创建临时文件");
            var 父临时文件夹目录 = Path.GetTempPath();
            父临时文件夹目录 += new Random().Next(11085878, 90166665);
            Directory.CreateDirectory(父临时文件夹目录);
            父临时文件夹目录 += "\\";
            var 子临时文件夹 = $"{父临时文件夹目录}{Path.GetFileNameWithoutExtension(文件位置)}\\";
            var osu谱面组 = 文件组数据.谱面组.FirstOrDefault();
            #region 建立谱面总信息文件夹
            运行状态 = (3, "建立谱面总信息文件夹");
            string 总信息_图片文件夹 = 子临时文件夹 + 资源.路径.雀食堂工程文件_谱面集_图片文件夹名 + "\\";
            string 总信息_谱面文件夹 = 子临时文件夹 + 资源.路径.雀食堂工程文件_谱面集_谱面文件夹名 + "\\";
            string 总信息_Json文件 = 子临时文件夹 + 资源.路径.雀食堂工程文件_谱面集_Json配置路径;
            Directory.CreateDirectory(子临时文件夹);
            Directory.CreateDirectory(总信息_图片文件夹);
            Directory.CreateDirectory(总信息_谱面文件夹);
            运行状态 = (8, "正在拷贝总信息文件夹图像数据");
            File.Copy(osu谱面组!.曲目元数据.图像路径, 总信息_图片文件夹 + 资源.路径.雀食堂工程文件_谱面集_图片路径);
            #endregion
            #region 遍历建立子信息文件夹
            运行状态 = (14, "正在建立谱面总信息文件夹");
            //遍历所有谱面，建立主要JSON信息
            谱面集文件夹 谱面集信息 = new 谱面集文件夹();
            var 总谱面名称 = 数据.音乐名称;
            谱面集信息.谱面集名称 = 总谱面名称;
            foreach (var 谱面 in 数据.谱面集)
            {
                谱面集信息.谱面名称.Add(谱面.谱面名称.Trim('.'));
            }
            //将Json信息写入位置
            运行状态 = (18, "正在建立谱面总信息的Json配置文件");
            string 谱面集JSON = JsonConvert.SerializeObject(谱面集信息);
            Json关键字替换(谱面集JSON, 替换类型.导出谱面);
            File.WriteAllText(总信息_Json文件, 谱面集JSON);
            #endregion
            #region 遍历建立子谱面文件夹
            运行状态 = (18, "正在处理谱面子信息"); int 当前数 = 0;
            //遍历所有谱面，建立音乐子信息
            foreach (var 谱面 in 数据.谱面集)
            {
                #region 建立子文件夹
                当前数++;
                运行状态 = (18 + 60*(当前数/数据.谱面集.Count), "正在创建子信息文件夹");
                var 子谱面名称 = 谱面.谱面名称.Trim('.');
                var 子文件夹 = $"{总信息_谱面文件夹}{子谱面名称}\\";
                var 子信息_图片文件夹 = $"{子文件夹}{资源.路径.雀食堂工程文件_子谱面_图片资源文件夹名}\\";
                var 子信息_音乐文件夹 = $"{子文件夹}{资源.路径.雀食堂工程文件_子谱面_音乐资源文件夹名}\\";
                var 子信息_Json文件 = $"{子文件夹}{资源.路径.雀食堂工程文件_子谱面_Json配置路径}";
                Directory.CreateDirectory(子文件夹);
                Directory.CreateDirectory(子信息_图片文件夹);
                Directory.CreateDirectory(子信息_音乐文件夹);
                #endregion
                #region 建立子Json文件
                运行状态 = (18 + 60 * (当前数 / 数据.谱面集.Count), "正在创建谱面的Json配置文件");
                //加入其他数据
                谱面.节拍段落.Add(new 节拍段落()
                {
                    开始时间 = 0,
                    特殊Midi节拍点 = new string[0],
                    类型 = 节拍段落.节拍段落类型.Custom.ToString(),
                    节拍 = -1,
                    节拍分量 = -1
                });
                谱面.节拍段落.Add(new 节拍段落()
                {
                    开始时间 = 100,
                    特殊Midi节拍点 = null,
                    类型 = 节拍段落.节拍段落类型.Normal.ToString(),
                    节拍 = 120,
                    节拍分量 = 2
                });
                string 子谱面Json = JsonConvert.SerializeObject(谱面);
                子谱面Json = Json关键字替换(子谱面Json, 替换类型.导出工程);
                File.WriteAllText(子信息_Json文件, 子谱面Json);
                #endregion
                #region 复制文件数据
                运行状态 = (18 + 60 * (当前数 / 数据.谱面集.Count), "正在拷贝文档");
                File.Copy(osu谱面组!.曲目元数据.图像路径, 子信息_图片文件夹 + 资源.路径.雀食堂工程文件_子谱面_专辑CG文件名);
                File.Copy(osu谱面组.曲目元数据.图像路径, 子信息_图片文件夹 + 资源.路径.雀食堂工程文件_子谱面_音乐CG文件名);
                #region 处理音频
                //以下部分是废话，实际上并不是二进制文件被修改了只是把文件名给搞错了。
                ///与雀食堂的谱面播放器不同，雀食堂的编辑器只支持Wav格式（汗...）所以这里需要做判断，进行额外的格式转换
                ///除此之外雀食堂的谱面音乐似乎定义了奇怪的WAV文件头，需要读取二进制文档，从“00001144”位置开始复制数据
                ///复制到00001180
                ///以下为需要复制的二进制数据段
                /* 00 01 02 03 04 05 06 07 08 09 0A 0B 0C 0D 0E 0F
                 *             FD FF FF FF FD FF FF FF FE FF FF FF
                 * FC FF FE FF FD FF FE FF FD FF FE FF FD FF FF FF 
                 * FD FF FF FD FF FE FF FE FF FF FF FD FD FF FF FF 
                 * FD FF FF FF FD FF FE FF FD FF FE FF FE FF FF FF 
                 *
                 */

                string 音乐文件路径 = 文件组数据.临时文件路径 + "\\" + osu谱面组.基本数据.音乐文件子路径;
                var 扩展名 = Path.GetExtension(音乐文件路径);
                if (扩展名 == ".mp3")
                {
                    运行状态 = (18 + 60 * (当前数 / 数据.谱面集.Count), "正在转换音频文件...");
                    string 音乐WAV文件新路径 = 音乐文件路径.Replace(".mp3", ".wav");
                    if (!File.Exists(音乐WAV文件新路径))
                    {
                        辅助类.音频转换.从Mp3转换到Wav(音乐文件路径, 音乐WAV文件新路径);
                    }
                    File.Copy(音乐WAV文件新路径, 子信息_音乐文件夹 + 资源.路径.雀食堂工程文件_子谱面_谱面音频);
                    File.Copy(音乐WAV文件新路径, 子信息_音乐文件夹 + 资源.路径.雀食堂工程文件_子谱面_预览音频);
                    continue;
                }
                else
                {
                    File.Copy(文件组数据.临时文件路径 + "\\" + osu谱面组.基本数据.音乐文件子路径, 子信息_音乐文件夹 + 资源.路径.雀食堂工程文件_子谱面_谱面音频);
                    File.Copy(文件组数据.临时文件路径 + "\\" + osu谱面组.基本数据.音乐文件子路径, 子信息_音乐文件夹 + 资源.路径.雀食堂工程文件_子谱面_预览音频);
                }

                #endregion

                #endregion
            }
            运行状态 = (85 * (当前数 / 数据.谱面集.Count), "正在打包...");
            辅助类.文件.压缩文件夹(父临时文件夹目录, 文件位置);
            #endregion 
            运行状态 = (99 * (当前数 / 数据.谱面集.Count), "即将完成");
        }
    }
}
