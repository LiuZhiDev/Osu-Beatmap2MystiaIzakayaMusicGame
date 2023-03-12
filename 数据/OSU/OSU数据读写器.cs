using MystiaIzakayaMusicGameConvert;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using 谱面转换器.资源;
using 谱面转换器.辅助类;

namespace 谱面转换器.数据.OSU
{
    public class OSU数据读写器
    {
        /// <summary>
        /// 读取一个OSZ文件内的数据
        /// 读写后可访问 谱面转换器.数据.OSU.文件组数据
        /// </summary>
        /// <param name="osz文件路径"></param>
        public OSU数据读写器(string osz文件路径)
        {
            清空数据集();
            var 临时目录 = 谱面转换器.辅助类.文件.解压Zip文件到临时目录(osz文件路径);
            文件组数据.临时文件路径 = 临时目录;
            文件组数据.包文件组 = Directory.GetFiles(临时目录).ToList();
            文件组数据.谱面文件组 = Directory.GetFiles(临时目录, "*.osu").ToList();
            foreach (var 谱面文件 in 文件组数据.谱面文件组)
            {
                var 谱面文件内容 = File.ReadAllText(谱面文件);

                //读取配置节后实例化相关的类 - 基本数据
                var 基本信息配置内容 = 配置文件.读内存配置项节文本(谱面文件内容, 路径.OSU基本信息配置节名);
                var 基本数据 = 读取谱面基本数据(基本信息配置内容, 谱面文件);

                //读取配置节后实例化相关的类 - 元数据
                //此处因为Metadata用户可能输入意料以外的字符，所以无法使用配置节文本读取方式
                var 元数据配置内容 = 正则.正则子匹配文本(谱面文件内容, "\\[Metadata][\\s\\S]([\\s\\S]+?)\\[D",0,0);
                var 元数据 = 读取谱面元数据(元数据配置内容, 临时目录);

                //读取配置节后实例化相关的类 - 难度数据
                var 难度数据配置内容 = 配置文件.读内存配置项节文本(谱面文件内容, 路径.OSU难度配置节名);
                var 难度数据 = 读取谱面难度数据(难度数据配置内容);

                //读取配置节后实例化相关的类 - 时间线数据
                var 时间线数据配置内容 = 配置文件.读内存配置项节文本(谱面文件内容, 路径.OSU时间线配置节名);
                var 时间线数据 = 读取谱面时间线数据(时间线数据配置内容, (float)难度数据.基础滑条速度倍率);

                //读取配置节后实例化相关的类 - 元素数据
                var 元素数据配置内容 = 配置文件.读内存配置项节文本(谱面文件内容, 路径.OSU元素配置节名);
                var 元素数据 = 读取谱面所有元素(元素数据配置内容, 基本数据.模式, 难度数据, 时间线数据);
                var Osu谱面数据 = new OSU.谱面数据()
                {
                    元素集数据 = 元素数据,
                    基本数据 = 基本数据,
                    时间线数据 = 时间线数据,
                    曲目元数据 = 元数据
                };
                文件组数据.谱面组.Add(Osu谱面数据);
            }

        }

        /// <summary>
        /// 由于OSU数据是定义的Static，所以每次使用都必须重新清理。
        /// </summary>
        private void 清空数据集()
        {
            文件组数据.临时文件路径 = "";
            文件组数据.包文件组 = new();
            文件组数据.谱面文件组 = new();
            文件组数据.谱面组 = new();
        }

        public 谱面数据.基本 读取谱面基本数据(string 谱面基本数据部分, string 配置文件名)
        {
            var 文件名 = Path.GetFileNameWithoutExtension(配置文件名);
            var 难度名 = 正则.正则子匹配文本(文件名, "\\[(.*)\\]", 0, 0);
            var 基本数据字典 = 冒号配置节转换为字典(谱面基本数据部分);
            var 曲目预览开始时间 = TimeSpan.FromMilliseconds(基本数据文本转换.从文本到双精度数(基本数据字典[键值.OSU.预览起始时间键]));
            var 模式 = 谱面数据.基本.模式转换为实际模式(基本数据文本转换.从文本到整数(基本数据字典[键值.OSU.模式键]));
            var 音频文件子路径 = 基本数据字典[键值.OSU.音频文件路径键];
            谱面数据.基本 基本数据 = new 谱面数据.基本()
            {
                曲目预览开始时间 = 曲目预览开始时间,
                模式 = 模式,
                难度名称 = 难度名,
                配置名称 = 文件名,
                音乐文件子路径 = 音频文件子路径
            };
            return 基本数据;
        }
        public 谱面数据.曲目元 读取谱面元数据(string 谱面元数据部分, string 目录路径)
        {
            var 图像路径集 = Directory.GetFiles(目录路径, "*.jpg");
            var 图像路径 = 图像路径集.FirstOrDefault();
            if (string.IsNullOrEmpty(图像路径)) 
            {
                MessageBox.Show("这个谱面没有专辑封面，将使用默认的封面");
                图像路径 = Directory.GetCurrentDirectory() + "\\icon\\Untitled.png";
            }
            var 元数据字典 = 冒号配置节转换为字典(谱面元数据部分);
            var 曲名 = 元数据字典[键值.OSU.音乐名称键];
            var 专辑或来源 = 元数据字典[键值.OSU.专辑键];
            var 艺术家 = 元数据字典[键值.OSU.艺术家键];
            var 谱面ID = 元数据字典[键值.OSU.谱面ID键];
            var 曲目元 = new 谱面数据.曲目元()
            {
                图像路径 = 图像路径,
                专辑或来源 = 专辑或来源,
                曲名 = 曲名,
                艺术家 = 艺术家,
                谱面ID = 谱面ID
            };
            return 曲目元;
        }
        public 谱面数据.难度 读取谱面难度数据(string 谱面难度数据部分)
        {
            var 难度数据字典 = 冒号配置节转换为字典(谱面难度数据部分);
            var 基础滑条速度倍率 = 基本数据文本转换.从文本到双精度数(难度数据字典[键值.OSU.滑条基础速度倍率]);

            return new 谱面数据.难度() { 基础滑条速度倍率 = 基础滑条速度倍率, 每秒经过像素数 = 基础滑条速度倍率 * 100 };
        }
        public List<谱面数据.时间线> 读取谱面时间线数据(string 谱面时间线部分, float 基础滑条速度)
        {
            var 分行时间线组 = 谱面时间线部分.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            float 上一次的每拍时长 = 0;
            List<谱面数据.时间线> 时间线组 = new();
            bool 已设定继承时间点 = false;
            foreach (var 元素 in 分行时间线组)
            {
                var 信息集 = 元素.Split(',');
                if (元素.Length < 8) { continue; }
                TimeSpan 时间线起始位置 = TimeSpan.FromMilliseconds(基本数据文本转换.从文本到整数(信息集[0]));
                var 继承时间点 = 辅助公式.是否为继承时间点(信息集);
                float SV_非继承时间点变化率 = 100;
                float 每拍时长 = 0;
                if (继承时间点)//如果是继承时间点，则信息集[1]的数字将修改SV
                {
                    SV_非继承时间点变化率 = Math.Abs(基本数据文本转换.从文本到单精度数(信息集[1]));
                    每拍时长 = 上一次的每拍时长;
    
                }
                else //如果不是继承时间点，则信息集[1]的数字将修改每拍时长，并重置SV
                {
                    //如果是第一次非继承时间点，那么将谱面总体延迟设置为该值
                    if (!已设定继承时间点) 
                    { 
                        转换器主界面.ui信息.谱面总延迟 = (int)时间线起始位置.TotalMilliseconds;
                        已设定继承时间点 = true;
                    }
                    每拍时长 = 基本数据文本转换.从文本到单精度数(信息集[1]);
                    SV_非继承时间点变化率 = 100;
                    
                    上一次的每拍时长 = 每拍时长;
                }
                int 节拍分量 = 基本数据文本转换.从文本到整数(信息集[2]);
                var BPM = 1f / 每拍时长 * 1000f * 60f;
                if (SV_非继承时间点变化率 == 0) { throw new Exception("变化率不能为0"); }
                var 时间线 = new 谱面数据.时间线()
                {
                    BPM = BPM,
                    开始时间 = 时间线起始位置,
                    每拍长度_毫秒 = 每拍时长,
                    节拍分量 = 节拍分量,
                    SV_非继承时间线变化率 = SV_非继承时间点变化率,
                };
                时间线组.Add(时间线);
            }
            return 时间线组;
        }
        public List<谱面数据.元素> 读取谱面所有元素(string 谱面元素数据部分, 谱面数据.基本.Osu游戏模式 游戏模式, 谱面数据.难度 难度数据, List<谱面数据.时间线> TimeLine集)
        {
            var 分行元素组 = 谱面元素数据部分.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            List<谱面数据.元素> 元素集 = new();
            int 元素计数 = 0;
            foreach (var 元素 in 分行元素组)
            {
                元素计数++;
                var 信息集 = 元素.Split(',');
                if (元素.Length < 6) { continue; }
                int 横向坐标 = (int)基本数据文本转换.从文本到双精度数(信息集[0]);
                int 纵向坐标 = (int)基本数据文本转换.从文本到双精度数(信息集[1]);
                TimeSpan 出现时间 = TimeSpan.FromMilliseconds(基本数据文本转换.从文本到整数(信息集[2]));
                TimeSpan 结束时间 = TimeSpan.FromMilliseconds(0);
                var 元素类型 = 谱面数据.元素.从数字转换为元素类型(基本数据文本转换.从文本到整数(信息集[3]));
                var 元素声效 = 谱面数据.元素.从数字转换为声效类型(基本数据文本转换.从文本到整数(信息集[4]));
                var 最近的时间线 = 辅助公式.寻找最近的上一个时间线(出现时间, TimeLine集);

                if (元素类型 == 谱面数据.元素.元素类型.滑条)
                {
                    var 滑条长度 = (int)基本数据文本转换.从文本到单精度数(信息集[7]);
                    try
                    {
                        结束时间 = TimeSpan.FromMilliseconds(辅助公式.计算滑条的结束时间(滑条长度, (float)难度数据.基础滑条速度倍率, 最近的时间线.SV_非继承时间线变化率, 最近的时间线.每拍长度_毫秒));
                    }
                    catch(Exception e)
                    {
                        MessageBox.Show($"在读取{游戏模式}谱面中，第 {元素计数} 个元素（滑条）的结束时间时发生错误，这可能是由于本谱面设计不规范导致，该元素将不被读取\r\n{e.Message}" );
                        continue;
                    }
                }
                if (元素类型 == 谱面数据.元素.元素类型.马娘长键)
                {
                    结束时间 = TimeSpan.FromMilliseconds(基本数据文本转换.从文本到单精度数(信息集[5].Split(':')[0]));
                }
                元素集.Add(new 谱面数据.元素()
                {
                    横向坐标 = 横向坐标,
                    纵向坐标 = 纵向坐标,
                    出现时间 = 出现时间,
                    原元素类型 = 元素类型,
                    原元素声效 = 元素声效,
                    结束时间 = 出现时间.Add(结束时间)

                });

            }

            return 元素集;

        }
        /// <summary>
        /// 读取由冒号分隔的键值项
        /// </summary>
        /// <param name="冒号配置节文本"></param>
        /// <returns></returns>
        public Dictionary<string, string> 冒号配置节转换为字典(string 冒号配置节文本)
        {
            var 分行配置项组 = 冒号配置节文本.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            Dictionary<string, string> 配置节 = new();
            foreach (var 分行配置项 in 分行配置项组)
            {
                if (string.IsNullOrWhiteSpace(分行配置项)) { continue; }
                var 配置项 = 分行配置项.Split(':');
                if (配置项.Length < 2) { continue; }
                var 配置项名 = 配置项[0].Trim();
                var 配置项值 = 配置项[1].Trim();
                配置节.Add(配置项名, 配置项值);
            }
            return 配置节;
        }
        /// <summary>
        /// 在某个字典中继续加入数据
        /// </summary>
        /// <param name="冒号配置节文本"></param>
        /// <param name="配置节"></param>
        /// <returns></returns>
        public Dictionary<string, string> 字典追加配置节信息(string 冒号配置节文本, Dictionary<string, string> 配置节)
        {
            var 分行配置项组 = 冒号配置节文本.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            foreach (var 分行配置项 in 分行配置项组)
            {
                if (string.IsNullOrWhiteSpace(分行配置项)) { continue; }
                var 配置项 = 分行配置项.Split(':');
                if (配置项.Length < 2) { continue; }
                var 配置项名 = 配置项[0].Trim();
                var 配置项值 = 配置项[1].Trim();
                配置节.Add(配置项名, 配置项值);
            }
            return 配置节;
        }

    }
}
