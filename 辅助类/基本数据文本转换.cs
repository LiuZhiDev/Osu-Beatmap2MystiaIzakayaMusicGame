using System;
using System.Diagnostics;

namespace 谱面转换器.辅助类
{
    /// <summary>
    /// 定义了一些基本的数据到文本转换
    /// </summary>
    internal static class 基本数据文本转换
    {
        /// <summary>
        /// 从一段文本转换成Double类型，如果有错误将在输出信息中显示
        /// </summary>
        /// <param name="文本"></param>
        /// <returns></returns>
        public static double 从文本到双精度数(string 文本)
        {
            double 数据;
            if (!double.TryParse(文本, out 数据))
            {
                转换报错(文本, "double");
            }
            return 数据;
        }

        /// <summary>
        /// 从一段文本转换成float类型，如果有错误将在输出信息中显示
        /// </summary>
        /// <param name="文本"></param>
        /// <returns></returns>
        public static float 从文本到单精度数(string 文本)
        {
            float 数据;
            if (!float.TryParse(文本, out 数据))
            {
                转换报错(文本, "float");
            }
            return 数据;
        }

        /// <summary>
        /// 从一段文本转换成int类型，如果有错误将在输出信息中显示
        /// </summary>
        /// <param name="文本"></param>
        /// <returns></returns>
        public static int 从文本到整数(string 文本)
        {
            int 数据;
            if (!int.TryParse(文本, out 数据))
            {
                转换报错(文本, "int");
            }
            return 数据;
        }

        /// <summary>
        /// 转换出错的逻辑
        /// </summary>
        /// <param name="文本"></param>
        /// <returns></returns>
        private static void 转换报错(string 原文本, string 转换类型)
        {
            Debug.WriteLine($"将 {原文本} 转换到 {转换类型} 转换失败");
        }
    }

    /// <summary>
    /// 定义了字节转换的逻辑
    /// </summary>
    public static class 字节转换
    {
        /// <summary>
        /// 从一个Int数值转换为string表示的Bit
        /// 结果从高位到低位
        /// </summary>
        /// <param name="intvalue"></param>
        /// <returns></returns>
        public static string 从Int转Byte(int intvalue)
        {

            return Convert.ToString(intvalue, 2).PadLeft(8, '0');
        }

    }
}
