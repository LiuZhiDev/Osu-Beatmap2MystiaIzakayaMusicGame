using System.Collections.Generic;
using System.ComponentModel;

namespace ConsoleApp25.辅助类
{
    /// <summary>
    /// 定义了一个嵌套字典，使用这个字典，将会在带上MVVM通知，通知UI自动刷新更改
    /// </summary>
    public class 嵌套字典 : Dictionary<string, Dictionary<string, string>>, INotifyPropertyChanged
    {
#pragma warning disable CS8612 // 类型中引用类型的为 Null 性与隐式实现的成员不匹配。
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
#pragma warning restore CS8612 // 类型中引用类型的为 Null 性与隐式实现的成员不匹配。


        // 重写索引器，以便在设置值时触发事件
        public new Dictionary<string, string> this[string 键]
        {
            get => base[键];
            set
            {
                base[键] = value;
                触发属性变更事件(键);
            }
        }

        // 定义一个辅助方法来触发事件
        private void 触发属性变更事件(string 属性名称)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(属性名称));
        }
    }
}
