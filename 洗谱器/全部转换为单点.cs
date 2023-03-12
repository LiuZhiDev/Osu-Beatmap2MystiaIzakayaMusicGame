using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 谱面转换器.接口.清洗器;
using 谱面转换器.数据.夜雀食堂;
using static 谱面转换器.接口.转换器.雀食堂转谱接口;

namespace 谱面转换器.洗谱器
{
    internal class 全部转换为单点:雀食堂洗谱接口
    {
        public 谱面数据.谱面信息 清洗谱面(ref 谱面数据.谱面信息 谱面)
        {
            var 元素集 = 谱面.元素集;
            元素集.ForEach((x) => { 
            if (x.元素类型 == 元素类型.Hold.ToString())
                {
                    x.元素类型 = 元素类型.Single.ToString();
                }
            
            });
            return 谱面;
        }

        public void 重置清洗()
        {

        }

    }
}
