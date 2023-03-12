using MystiaIzakayaMusicGameConvert.数据;
using MystiaIzakayaMusicGameConvert.辅助类;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using 谱面转换器.接口.清洗器;
using 谱面转换器.数据.夜雀食堂;

namespace MystiaIzakayaMusicGameConvert.洗谱器
{
    internal class 撤销所有编辑 : 雀食堂洗谱接口
    {
        public 谱面数据.谱面信息 清洗谱面(ref 谱面数据.谱面信息  谱面)
        {
            运行时.转谱结果.还原原始信息组();
            return 谱面;
        }

        public void 重置清洗()
        {
        
        }
    }
}
