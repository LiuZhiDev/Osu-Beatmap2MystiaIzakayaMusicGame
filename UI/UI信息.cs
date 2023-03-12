using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Media;
using 谱面转换器.数据.夜雀食堂;
using static 谱面转换器.资源.清洗器;

namespace MystiaIzakayaMusicGameConvert
{


    public class UI信息 : INotifyPropertyChanged
    {

        private int _谱面总延迟 = 0;
        public int 谱面总延迟
        {
            get => _谱面总延迟;
            set
            {
                _谱面总延迟 = value;
                OnPropertyChanged(nameof(谱面总延迟));
            }
        }

        private string _运行状态 = "闲置中";
        public string 运行状态
        {
            get => _运行状态;
            set
            {
                _运行状态 = value;
                OnPropertyChanged(nameof(运行状态));
            }
        }

        private string _曲目名称 = "请选择文件";
        public string 曲目名称
        {
            get => _曲目名称;
            set
            {
                _曲目名称 = value;
                OnPropertyChanged(nameof(曲目名称));
            }
        }
        private string _艺术家 = "请选择文件";
        public string 艺术家
        {
            get => _艺术家;
            set
            {
                _艺术家 = value;
                OnPropertyChanged(nameof(艺术家));
            }
        }

        private string _谱面信息 = "请选择文件";
        public string 谱面信息
        {
            get => _谱面信息;
            set
            {
                _谱面信息 = value;
                OnPropertyChanged(nameof(谱面信息));
            }
        }

        private ImageSource? _图像 = null;
        public ImageSource 图像
        {
            get => _图像!;
            set
            {
                _图像 = value;
                OnPropertyChanged(nameof(图像));
            }
        }

        private bool _显示已删除元素 = false;
        public bool 显示已删除元素
        {
            get => _显示已删除元素;
            set
            {
                _显示已删除元素 = value;
                OnPropertyChanged(nameof(显示已删除元素));
                UI命令.显隐已删除元素(value);
            }
        }

        private List<string> _谱面列表 = new();
        public List<string> 谱面列表
        {
            get => _谱面列表;
            set
            {
                _谱面列表 = value;
                OnPropertyChanged(nameof(谱面列表));
            }
        }

        private int _谱面列表选定下标 = -1;
        public int 谱面列表选定下标
        {
            get => _谱面列表选定下标;
            set
            {
                _谱面列表选定下标 = value;
                if (value == -1) { return; }
                已修改谱面列表选定(nameof(谱面列表选定下标));
            }
        }


        private List<清洗器名> _自动清理工具 = new();
        public List<清洗器名> 自动清理工具
        {
            get => _自动清理工具;
            set
            {
                _自动清理工具 = value;
                OnPropertyChanged(nameof(自动清理工具));
            }
        }
        private int _自动清理工具选定下标 = -1;
        public int 自动清理工具选定下标
        {
            get => _自动清理工具选定下标;
            set
            {
                _自动清理工具选定下标 = value;
    
            }
        }

        private List<谱面数据.元素> _元素集 = new();
        public List<谱面数据.元素> 元素集
        {
            get => _元素集;
            set
            {
                _元素集 = value;
                OnPropertyChanged(nameof(元素集));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        protected void 已修改谱面列表选定(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
#warning 临时加入，可能需重写
            UI命令.谱面选择();
        }
        protected void 已修改谱面清理器选定(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
#warning 临时加入，可能需重写
            UI命令.谱面清理();
        }

        protected void 查看删除元素状态更新(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            
        }
    }
}
