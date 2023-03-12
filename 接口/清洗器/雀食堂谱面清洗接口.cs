using static 谱面转换器.数据.夜雀食堂.谱面数据;

namespace 谱面转换器.接口.清洗器
{
    /// <summary>
    /// 继承此接口将定义一个洗谱器，程序将调用清洗谱面方法来清洗谱面
    /// </summary>
    public interface 雀食堂洗谱接口
    {
        /// <summary>
        /// 清洗一张谱面
        /// </summary>
        /// <param name="谱面"></param>
        /// <returns></returns>
        public 谱面信息 清洗谱面(ref 谱面信息 谱面);
        /// <summary>
        /// （若需要）重置这个清洗器实例
        /// </summary>
        public void 重置清洗();
    }
}
