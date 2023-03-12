using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace MystiaIzakayaMusicGameConvert.辅助类
{
    /// <summary>
    /// 类的深复制
    /// </summary>
    public static class 深复制
    {
        /// <summary>
        /// 创建一个实例化类的相同实例，要实例化的类要标记为[System.Runtime.Serialization]
        /// 通过二进制化来实例，效率更高
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="实体类"></param>
        /// <returns></returns>
        public static T 拷贝类<T>(T 实体类)
        {
            using (Stream objectStream = new MemoryStream())
            {
                //利用 System.Runtime.Serialization序列化与反序列化完成引用对象的复制
                IFormatter formatter = new BinaryFormatter();
#pragma warning disable SYSLIB0011 // 类型或成员已过时
#pragma warning disable CS8604 // 引用类型参数可能为 null。
                formatter.Serialize(objectStream, 实体类);
#pragma warning restore CS8604 // 引用类型参数可能为 null。
#pragma warning restore SYSLIB0011 // 类型或成员已过时
                objectStream.Seek(0, SeekOrigin.Begin);
#pragma warning disable SYSLIB0011 // 类型或成员已过时
                return (T)formatter.Deserialize(objectStream);
#pragma warning restore SYSLIB0011 // 类型或成员已过时
            }
        }

        /// <summary>
        /// 利用反射创建实例化类的相同实例，利用反射方法，效率更低
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T 拷贝类_反射<T>(T obj)
        {
#pragma warning disable CS8602 // 解引用可能出现空引用。
            if (obj is string || obj.GetType().IsValueType)
                return obj;
#pragma warning restore CS8602 // 解引用可能出现空引用。

#pragma warning disable CS8600 // 将 null 字面量或可能为 null 的值转换为非 null 类型。
            object retval = Activator.CreateInstance(obj.GetType());
#pragma warning restore CS8600 // 将 null 字面量或可能为 null 的值转换为非 null 类型。
            FieldInfo[] fields = obj.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
            foreach (var field in fields)
            {
                try
                {
                    field.SetValue(retval, 拷贝类(field.GetValue(obj)));
                }
                catch { }
            }

#pragma warning disable CS8600 // 将 null 字面量或可能为 null 的值转换为非 null 类型。
#pragma warning disable CS8603 // 可能返回 null 引用。
            return (T)retval;
#pragma warning restore CS8603 // 可能返回 null 引用。
#pragma warning restore CS8600 // 将 null 字面量或可能为 null 的值转换为非 null 类型。
        }
    }
}
