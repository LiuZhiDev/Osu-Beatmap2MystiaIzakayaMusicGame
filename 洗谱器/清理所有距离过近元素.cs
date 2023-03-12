using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 谱面转换器.接口.清洗器;
using 谱面转换器.数据.夜雀食堂;
using static 谱面转换器.接口.转换器.雀食堂转谱接口;

namespace MystiaIzakayaMusicGameConvert.洗谱器
{
    internal class 清理所有距离过近元素 : 雀食堂洗谱接口
    {
        /// <summary>
        /// 清理相邻时间小于110毫秒的单点
        /// </summary>
        /// <param name="元素集"></param>
        private static void 清理被相邻过近的单点(List<谱面数据.元素> 元素集)
        {
            #region 经过NewBing优化过的代码
            // 找出集合中所有类型为Single的元素，并存储在一个新的集合中
            var 单点集 = 元素集.FindAll(x => x.元素类型 == 元素类型.Single.ToString());
            // 初始化一个空的临时集合来存储需要保留的元素
            var 临时集 = new List<谱面数据.元素>();
            // 初始化最近一次保留的单点出现时间为-1000，表示没有保留过
            int 最近保留时间 = -1000;
            // 遍历单点集中的每个单点
            foreach (var 单点 in 单点集)
            {
                // 计算当前单点和最近保留时间的差值，单位为毫秒
                var 差值 = 单点.出现时间 - 最近保留时间;
                // 如果差值大于或等于110毫秒，表示当前单点距离上一个保留单点足够远，需要保留当前单点，并添加到临时集合中
                if (差值 >= 110)
                {
                    临时集.Add(单点);
                    // 更新最近保留时间为当前单点的出现时间
                    最近保留时间 = 单点.出现时间;
                }
            }
            // 将临时集合赋值给原来的单点集
            单点集 = 临时集;
            #endregion
            #region 原先的代码
            /*
            var 单点集 = 元素集.FindAll(x => x.元素类型 == 元素类型.Single.ToString());
            int 上一次点击时间 = -1000;
            for (int i = 0; i < 单点集.Count; i++)
            {
                var 差值 = 单点集[i].出现时间 - 上一次点击时间;
                if (差值 < 110) { 单点集.Remove(单点集[i]); }
                if (单点集.Count == i)
                {
                    continue;
                }
                上一次点击时间 = 单点集[i].出现时间;
            }
            */
            #endregion
        }

        private static void 清理开始与结束时间过近的滑条(List<谱面数据.元素> 元素集)
        {
            List<谱面数据.元素> 临时建新元素集 = new List<谱面数据.元素>();
            元素集.ForEach((x) => { 
                 if (x.结束时间 - x.出现时间 < 200)
                {
                    //将滑条转换为单点
                    x.元素类型 = 元素类型.Single.ToString();
                    x.结束时间 = 0;
                }


            });
    
        }

        public 谱面数据.谱面信息 清洗谱面(ref 谱面数据.谱面信息 谱面)
        {
            var 元素集 = 谱面.元素集;
            清理被相邻过近的单点(元素集);
            清理开始与结束时间过近的滑条(元素集);
            return 谱面;
        }

        public void 重置清洗()
        {
           
        }
    }
}
