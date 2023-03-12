using MystiaIzakayaMusicGameConvert;
using System;
using System.Collections.Generic;
using System.Linq;
using 谱面转换器.接口.清洗器;
using 谱面转换器.数据.夜雀食堂;
using static 谱面转换器.接口.转换器.雀食堂转谱接口;

namespace 谱面转换器.洗谱器
{
    internal class 最终格式化 : 雀食堂洗谱接口
    {
        public 谱面数据.谱面信息 清洗谱面(ref 谱面数据.谱面信息 谱面)
        {
            var 元素集 = 谱面.元素集;
            清理被删除的元素(元素集);
            谱面延迟调整(元素集);
            谱面元素重排序(元素集);
            //谱面规格化;
            foreach (var 元素 in 元素集)
            {

                if (元素.元素类型 == 元素类型.Single.ToString())
                {
                    元素.结束时间 = 0;

                    continue;
                }
                if (元素.元素类型 == 元素类型.HoldSingle.ToString())
                {
                    元素.结束时间 = 0;

                    continue;
                }
            }
            return 谱面;
        }

        private void 谱面元素重排序(List<谱面数据.元素> 元素集)
        {
            元素集.OrderBy(x => x.出现时间);
        }

        private void 谱面延迟调整(List<谱面数据.元素> 元素集)
        {
            元素集.ForEach(x => { 

                x.出现时间 += 转换器主界面.ui信息.谱面总延迟; 
                x.结束时间 += 转换器主界面.ui信息.谱面总延迟;
            });

        }


        /// <summary>
        /// 清理所有类型被定义为Null的元素
        /// </summary>
        /// <param name="元素集"></param>
        private static void 清理被删除的元素(List<谱面数据.元素> 元素集)
        {
            元素集.RemoveAll(x => x.元素类型 == null);
        }


        public void 重置清洗()
        {
            //不需要重置
      
        }
    }
}
