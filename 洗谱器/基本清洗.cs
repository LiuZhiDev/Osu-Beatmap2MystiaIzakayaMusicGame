using System;
using 谱面转换器.接口.清洗器;
using 谱面转换器.数据.夜雀食堂;
using static 谱面转换器.接口.转换器.雀食堂转谱接口;

namespace 谱面转换器.洗谱器
{

    /// <remarks>
    /// 通过预判玩家当前手部的忙碌状态，来进行清洗
    /// </remarks>
    public class 基本清洗 : 雀食堂洗谱接口
    {
        #region 玩家手部状态定义
        /// <summary>
        /// 指示玩家的左手处于忙碌状态
        /// </summary>
        bool 左手忙碌 = false;
        /// <summary>
        /// 指示玩家左手的忙碌状态将于多少毫秒时结束
        /// </summary>
        int 左手释放时间 = -1;
        /// <summary>
        /// 获取玩家左手正在按下的元素，若为NULL则未按下任何元素
        /// </summary>
        谱面数据.元素? 左手正长按元素 { get; set; }
        /// <summary>
        /// 指示玩家的右手处于忙碌状态
        /// </summary>
        bool 右手忙碌 = false;
        /// <summary>
        /// 指示玩家右手的忙碌状态将于多少毫秒时结束
        /// </summary>
        int 右手释放时间 = -1;
        /// <summary>
        /// 获取玩家右手正在按下的元素，若为NULL则未按下任何元素
        /// </summary>
        谱面数据.元素? 右手正长按元素 { get; set; }
        #endregion


        public 谱面数据.谱面信息 清洗谱面(ref 谱面数据.谱面信息 谱面)
        {
            ///获取谱面中的所有元素
            var 元素集 = 谱面.元素集;
            foreach (var 元素 in 元素集)
            {
                刷新玩家长按元素状态(元素);

                var 不能被按到 = !按键有效(元素);
                if (不能被按到)
                {
                    if (元素.元素类型 == 元素类型.Single.ToString())
                    {
                        点触清洗(元素);

                        continue;
                    }
                    if (元素.元素类型 == 元素类型.Hold.ToString())
                    {
                        长按清洗(元素);

                        continue;
                    }

                }
                检测玩家手部状态(元素);

            }

            return 谱面;
        }

        public void 重置清洗()
        {
            左手忙碌 = false;
            左手释放时间 = -1;
            左手正长按元素 = null;
            右手忙碌 = false;
            右手释放时间 = -1;
            右手正长按元素 = null;
        }

        /// <summary>
        /// 刷新定义的玩家手部长按元素状态
        /// </summary>
        /// <param name="元素"></param>
        private void 刷新玩家长按元素状态(谱面数据.元素 元素)
        {
            if (!(元素.出现时间 > 元素.结束时间)) { return; }
            if (元素.元素位置 == 元素位置.Left.ToString())
            {
                左手正长按元素 = null;
            }
            if (元素.元素位置 == 元素位置.Right.ToString())
            {
                右手正长按元素 = null;
            }
        }
        /// <summary>
        /// 刷新定义的玩家手部状态
        /// </summary>
        /// <param name="元素"></param>
        private void 检测玩家手部状态(谱面数据.元素 元素)
        {
            if (元素.元素类型 == 元素类型.Hold.ToString())
            {
                if (元素.元素位置 == 元素位置.Left.ToString())
                {
                    左手忙碌 = true;
                    左手正长按元素 = 元素;
                    左手释放时间 = 元素.结束时间;
                    return;
                }
                if (元素.元素位置 == 元素位置.Right.ToString())
                {
                    右手忙碌 = true;
                    右手正长按元素 = 元素;
                    右手释放时间 = 元素.结束时间;
                    return;
                }
            }

        }
        /// <summary>
        /// 检查该元素是否能正常被玩家按下（元素位置的手是空闲状态）
        /// </summary>
        /// <param name="元素"></param>
        /// <returns></returns>
        private bool 按键有效(谱面数据.元素 元素)
        {
            if (元素.元素位置 == 元素位置.Left.ToString() && 左手忙碌)
            {
                if (元素.出现时间 > 左手释放时间) { 左手忙碌 = false; return true; }
                return false;

            }
            if (元素.元素位置 == 元素位置.Right.ToString() && 右手忙碌)
            {
                if (元素.出现时间 > 右手释放时间) { 右手忙碌 = false; return true; }
                return false;
            }
            return true;
        }
        /// <summary>
        /// 如果玩家某个手部处于忙碌状态时，还出现了单点元素，则执行的清洗逻辑
        /// </summary>
        /// <param name="元素"></param>
        private void 点触清洗(谱面数据.元素 元素)
        {
            if (元素.元素位置 == 元素位置.Left.ToString())
            {
                //尝试换边
                if (!右手忙碌)
                {
                    元素.元素位置 = 元素位置.Right.ToString();
                }
                else
                {
                    //转换成删除
                    元素.元素类型 = null;
                }

                return;
            }
            if (元素.元素位置 == 元素位置.Right.ToString())
            {
                //尝试换边
                if (!左手忙碌)
                {
                    元素.元素位置 = 元素位置.Left.ToString();
                }
                else
                {
                    //转换成删除
                    元素.元素类型 = null;
                }

                return;
            }
        }
        /// <summary>
        /// 如果玩家某个手部处于忙碌状态时，还出现了长按元素，则执行的清洗逻辑
        /// </summary>
        /// <param name="元素"></param>
        private void 长按清洗(谱面数据.元素 元素)
        {
            if (元素.元素位置 == 元素位置.Left.ToString())
            {
                合并滑条并转换(元素);
                return;
            }
            if (元素.元素位置 == 元素位置.Right.ToString())
            {
                //转换成删除
                元素.元素类型 = null;
                return;
            }
        }

        /// <summary>
        /// 合并在同一时间段内的滑条
        /// </summary>
        /// <param name="元素"></param>
        private void 合并滑条并转换(谱面数据.元素 元素)
        {
            if (元素.元素位置 == 元素位置.Left.ToString())
            {
                if (右手正长按元素 == null) { return; }
                //如果都在上个滑条内，就尝试转换成一个点触
                if (元素.结束时间 <= 左手正长按元素!.结束时间)
                {
                    //看看换边是否可以转换成点触，不能的话就删除掉
                    if (右手忙碌)
                    {
                        元素.元素类型 = null;
                    }
                    else
                    {
                        元素.元素位置 = 元素位置.Right.ToString();
                        元素.元素类型 = 元素类型.Single.ToString();
                    }

                }
                //如果已经超出了这个滑条，就合并左手的滑条，并尝试转换成一个点触
                if (元素.结束时间 > 左手正长按元素.结束时间)
                {
                    //看看换边是否可以转换成点触，不能的话就删除掉
                    if (右手忙碌)
                    {
                        元素.元素类型 = null;
                    }
                    else
                    {
                        左手正长按元素.结束时间 = 元素.结束时间;
                        左手释放时间 = 元素.结束时间; //合并释放时间
                        元素.元素位置 = 元素位置.Right.ToString();
                        元素.元素类型 = 元素类型.Single.ToString();
                    }
                }
            }

            if (元素.元素位置 == 元素位置.Right.ToString())
            {
                if (右手正长按元素 == null) { return; }
                //如果都在上个滑条内，就把开始转换成一个点触
                if (元素.结束时间 < 右手正长按元素.结束时间)
                {
                    //看看换边是否可以转换成点触，不能的话就删除掉
                    if (左手忙碌)
                    {
                        元素.元素类型 = null;
                    }
                    else
                    {
                        元素.元素位置 = 元素位置.Left.ToString();
                        元素.元素类型 = 元素类型.Single.ToString();
                    }

                }
                //如果已经超出了这个滑条，就合并右手的滑条，并尝试转换成一个点触
                if (元素.结束时间 > 右手正长按元素.结束时间)
                {
                    //看看换边是否可以转换成点触，不能的话就删除掉
                    if (左手忙碌)
                    {
                        元素.元素类型 = null;
                    }
                    else
                    {
                        右手正长按元素.结束时间 = 元素.结束时间;
                        右手释放时间 = 元素.结束时间; //合并释放时间
                        元素.元素位置 = 元素位置.Right.ToString();
                        元素.元素类型 = 元素类型.Single.ToString();
                    }
                }
            }
        }



    }
}
