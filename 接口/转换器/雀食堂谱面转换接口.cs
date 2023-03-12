using 谱面转换器.数据.夜雀食堂;

namespace 谱面转换器.接口.转换器
{
    /// <summary>
    /// 定义一个谱面转换器，用于转换谱面数据
    /// </summary>
    public interface 雀食堂转谱接口
    {
        /// <summary>
        /// 在长时间操作时，UI需要更新的模块状态
        /// </summary>
        /// <remarks>
        /// 需要注意的是，总进度是0-100之间的整数，若总进度到达100，则UI会停止更新模块状态
        /// 这意味着继承该接口的转换器的长时间操作应该运行结束
        /// </remarks>
        public (int 总进度, string 状态解释) 运行状态 { get; set; }
        /// <summary>
        /// 将某个谱面进行转换，程序将调用此方法转换谱面
        /// 需要将源文件进行操作后转换成雀食堂能读取的谱面类型
        /// </summary>
        /// <param name="源文件路径">程序传入的源文件路径</param>
        public void 转换(string 源文件路径);
        /// <summary>
        /// 取得转换好的谱面信息组
        /// </summary>
        /// <returns></returns>
        public 谱面数据 取得谱面信息组();
        /// <summary>
        /// 取得未进行任何处理的原始谱面信息组
        /// </summary>
        /// <returns></returns>
        public 谱面数据 取得原始信息组();
        /// <summary>
        /// 还原原始信息组
        /// </summary>
        /// <returns></returns>
        public 谱面数据 还原原始信息组();
        /// <summary>
        /// 将谱面导出为雀食堂能读取的谱面类型
        /// </summary>
        /// <param name="文件位置"></param>
        public void 导出谱面(string 文件位置);
        public void 导出工程(string 文件位置);
        /// <summary>
        /// 雀食堂的元素位置
        /// </summary>
        public enum 元素位置
        {
            /// <summary>
            /// 左手按键
            /// </summary>
            Right,
            /// <summary>
            /// 右手按键
            /// </summary>
            Left
        }
        /// <summary>
        /// 雀食堂的元素类型
        /// </summary>
        public enum 元素类型
        {
            /// <summary>
            /// 单点
            /// </summary>
            Single,
            /// <summary>
            /// 长按
            /// </summary>
            Hold,
            /// <summary>
            /// 黄点
            /// </summary>
            HoldSingle
        }

        /// <summary>
        /// 将其他谱面的位置信息转换为雀食堂的谱面位置信息
        /// </summary>
        /// <param name="信息集"></param>
        /// <returns></returns>
        public 元素位置 夜雀食堂元素位置转换器(object[] 信息集);

        /// <summary>
        /// 将其他谱面的类型信息转换为雀食堂的谱面类型信息
        /// </summary>
        /// <param name="信息集"></param>
        /// <returns></returns>
        public 元素类型 夜雀食堂元素类型转换器(object[] 信息集);

    }
}
