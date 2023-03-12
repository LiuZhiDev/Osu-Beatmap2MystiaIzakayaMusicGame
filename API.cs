using 谱面转换器.接口.转换器;
using 谱面转换器.资源;

namespace MystiaIzakayaMusicGameConvert
{
    /// <summary>
    /// 这里定义了一些方便程序使用的API，避免代码混杂
    /// </summary>
    public static class API
    {
        /// <summary>
        /// 使用已经定义好的转换器将支持的谱面转换雀食堂的谱面
        /// </summary>
        /// <param name="要使用的转换器">选择要使用的转换器名称</param>
        /// <param name="源文件路径">选择要转换的源文件路径</param>
        /// <returns></returns>
        public static 雀食堂转谱接口 执行转换(转换器.转换器名 要使用的转换器, string 源文件路径)
        {
            转换器.雀食转换器集[要使用的转换器].转换(源文件路径);
            return 转换器.雀食转换器集[要使用的转换器];
        }
        /// <summary>
        /// 导出转换器中转换好的文件
        /// </summary>
        /// <param name="要使用的转换器">选择已经使用过的转换器名称</param>
        /// <param name="目标文件路径">选择导出文件的路径包括后缀名</param>
        public static void 导出(转换器.转换器名 要使用的转换器, string 目标文件路径)
        {
            转换器.雀食转换器集[要使用的转换器].导出谱面(目标文件路径);
        }
    }
}
