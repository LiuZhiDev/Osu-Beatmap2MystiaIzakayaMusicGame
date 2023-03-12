using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 谱面转换器.接口.转换器;

namespace MystiaIzakayaMusicGameConvert.数据
{
    public static class 运行时
    {
        public enum 导出为 
        {
            工程,
            谱面
        }
        public static 雀食堂转谱接口? 转谱结果 { get; set; }
        public static 导出为 导出方式 { get; set; }
    }
}
