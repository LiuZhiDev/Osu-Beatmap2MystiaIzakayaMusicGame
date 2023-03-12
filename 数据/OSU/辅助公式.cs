using System;
using System.Collections.Generic;

namespace 谱面转换器.数据.OSU
{
    public static class 辅助公式
    {
        /// <summary>
        /// 根据官方文档
        /// 乘100即可得到滑条每拍滑条经过多少像素
        /// 1. 首先60/BPM，得到每拍时长
        /// 2. 滑条基础速倍率值乘100 得到每拍经过像素数
        /// 3. 得到像素总数
        /// 滑条长度400 => 1秒每拍 每拍经过100像素，共持续4秒
        /// 滑条长度900 => 0.5秒每拍即BPM为120，每拍经过300像素，共持续900/300=共要进行3拍*0.5秒的每拍长度=1.5秒执行完毕。
        /// 得出公式 像素总数/滑条基础速速倍率*（60/BPM）
        /// </summary>
        public static int 计算滑条结束时间(int 开始的毫秒数, float BPM数, float 滑条基础速度倍率, float 滑条像素总数)
        {
            var 每拍毫秒数 = 60f / (float)BPM数 * 1000;
            var 滑条持续时间 = (float)滑条像素总数 / ((float)滑条基础速度倍率 * 100f) * (每拍毫秒数);
            var 最终时间 = 开始的毫秒数 + 滑条持续时间;
            return (int)最终时间;
        }
        /// <summary>
        /// 根据官方文档
        /// 乘100即可得到滑条每拍滑条经过多少像素
        /// 1. 首先60/BPM，得到每拍时长
        /// 2. 滑条基础速倍率值乘100 得到每拍经过像素数
        /// 3. 得到像素总数
        /// 滑条长度400 => 1秒每拍 每拍经过100像素，共持续4秒
        /// 滑条长度900 => 0.5秒每拍即BPM为120，每拍经过300像素，共持续900/300=共要进行3拍*0.5秒的每拍长度=1.5秒执行完毕。
        /// 得出公式 像素总数/滑条基础速速倍率*（60/BPM）
        /// </summary>
        public static int 计算滑条结束时间2(int 开始的毫秒数, float 每拍时长, float 滑条基础速度倍率, float 滑条像素总数)
        {
            var 每拍毫秒数 = 每拍时长;
            var 滑条持续时间 = (float)滑条像素总数 / ((float)滑条基础速度倍率 * 100f) * (每拍毫秒数);
            var 最终时间 = 开始的毫秒数 + 滑条持续时间;
            return (int)最终时间;
        }

        public static float 滑条速度倍率转换至毫秒(float 滑条速度倍率, float 每拍时长)
        {
            return 每拍时长 * 100 / 滑条速度倍率;
        }

        /// <summary>
        /// 判断这个时间点是否为非继承时间点，也就是是否继承之前的速度进行倍率变化
        /// </summary>
        /// <param name="数据组"></param>
        /// <returns></returns>
        /// <exception cref="Exception">如果数据组的格式不正确将抛出异常</exception>
        public static bool 是否为继承时间点(string[] 数据组)
        {
            if (数据组.Length < 8)
            {
                throw new Exception("无法判断是否为非继承时间点，数组量不足");
            }
            var 值 = 数据组[6].Trim();
            if (值 == "0") { return true; }
            else { return false; }
        }

        public static 数据.OSU.谱面数据.时间线? 寻找最近的上一个时间线(TimeSpan 目标时间, List<数据.OSU.谱面数据.时间线> 时间线集)
        {
            int 最短时间 = 0;
            数据.OSU.谱面数据.时间线 最近的时间线 = new();
            for (int i = 0; i < 时间线集.Count; i++)
            {
                var 时间差值 = 目标时间.Subtract(时间线集[i].开始时间).TotalMilliseconds;
                if (时间差值 < 0) { return 最近的时间线; }//已经超过了则返回最后一个
                if (时间差值 >= 0)
                {
                    最短时间 = (int)时间差值;
                    最近的时间线 = 时间线集[i];
                    return 最近的时间线;
                }
                if (i > 时间线集.Count - 1) 
                { return null; }
            }
            
            return null;
        }

        /// <summary>
        /// 官方的公式
        /// </summary>
        /// <param name="长度"></param>
        /// <param name="基础滑条速度"></param>
        /// <param name="变化速率"></param>
        /// <param name="每拍长度"></param>
        /// <returns></returns>
        public static float 计算滑条的结束时间(float 长度, float 基础滑条速度, float 变化速率, float 每拍长度)
        {

            var 结果 = (float)(长度 / (基础滑条速度*1000f * (变化速率)) * 每拍长度) * 1000f;
            if (结果 > 100000) 
            {
                throw new Exception("似乎计算成了不正确的值");
            }
            return 结果;
          
        }
    }
}
