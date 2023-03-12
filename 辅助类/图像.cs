using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using 谱面转换器.辅助类;

namespace ConsoleApp25.辅助类
{

    /// <summary>
    /// 读取Image或转换为ImageSource
    /// </summary>
    public static class 图像
    {


        /// <summary>
        /// 把DrawingImage转换为System.Windows.Media.ImageSource WPF中使用的类中的Iamge类
        /// </summary>
        /// <param name="gdiImg"></param>
        /// <returns></returns>
        public static System.Windows.Media.ImageSource 从Image转换至ImageSource(System.Drawing.Image gdiImg)
        {


            System.Windows.Controls.Image img = new System.Windows.Controls.Image();
            //convert System.Drawing.Image to WPF image
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(gdiImg);
            IntPtr hBitmap = bmp.GetHbitmap();
            return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        }

        /// <summary>
        /// 从文件中读取图像到ImageSource
        /// </summary>
        /// <param name="imagePath">图像路径</param>
        /// <returns></returns>
        public static System.Windows.Controls.Image 从文件到ImageSource(string imagePath)
        {
            BitmapImage bitmapImage = new BitmapImage(new Uri("PathToImageFile.png", UriKind.Relative));
            System.Windows.Controls.Image image = new System.Windows.Controls.Image();
            return image;
        }

        public static BitmapImage 从文件到BitmapImage(string 文件路径)
        {
            if (string.IsNullOrWhiteSpace(文件路径))
            {
                MessageBox.Show("这个谱面没有专辑封面，将使用默认的封面");
                文件路径 = Directory.GetCurrentDirectory()+"\\icon\\Untitled.png";
            }
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(文件路径);
            bitmap.EndInit();
            return bitmap;
        }


        /// <summary>
        /// 取得某个图片
        /// </summary>
        /// <param name="FilePath">文件位置</param>
        /// <returns></returns>
        public static Image 取得图片(string FilePath)
        {
            FileStream fileStream = new FileStream(FilePath, FileMode.Open, FileAccess.Read);
            var res = Image.FromStream(fileStream);
            fileStream.Close();
            fileStream.Dispose();

            return res;
        }
        /// <summary>
        /// 压缩调整图片分辨率
        /// </summary>
        /// <param name="OldFilePath">旧文件地址</param>
        /// <param name="NewFilePath">新文件地址</param>
        /// <param name="maxWidth">图片宽度最大限制</param>
        /// <param name="maxHeight">图片高度最大限制</param>
        /// <returns></returns>
        public static bool 压缩图片(string OldFilePath, string NewFilePath, int maxWidth = 800, int maxHeight = 5000)
        {
            bool result = false;//是否有执行压缩

            System.Drawing.Image imgPhoto = System.Drawing.Image.FromFile(OldFilePath);
            int imgWidth = imgPhoto.Width;
            int imgHeight = imgPhoto.Height;
            if (imgWidth > maxWidth)  //如果宽度超过高度以宽度为准来压缩
            {
                if (imgWidth > maxWidth)  //如果图片宽度超过限制
                {
                    int toImgWidth = maxWidth;   //图片压缩后的宽度
                    int toImgHeight = (int)((float)imgHeight / ((float)imgWidth / (float)toImgWidth)); //图片压缩后的高度
                    System.Drawing.Bitmap img =
                    new System.Drawing.Bitmap(imgPhoto, toImgWidth, toImgHeight);
                    imgPhoto.Dispose();
                    img.Save(NewFilePath, System.Drawing.Imaging.ImageFormat.Jpeg);  //保存压缩后的图片
                    result = true;
                }
            }
            else
            {
                if (imgHeight > maxHeight)
                {
                    int toImgHeight1 = maxHeight;
                    int toImgWidth1 = (int)((float)imgWidth / ((float)imgHeight / (float)toImgHeight1));
                    System.Drawing.Bitmap img =
                    new System.Drawing.Bitmap(imgPhoto, toImgWidth1, toImgHeight1);
                    imgPhoto.Dispose();
                    img.Save(NewFilePath, System.Drawing.Imaging.ImageFormat.Jpeg);  //保存压缩后的图片
                    result = true;
                }
            }
            if (result == false)
            {
                System.Drawing.Bitmap img = new System.Drawing.Bitmap(imgPhoto);
                imgPhoto.Dispose();
                img.Save(OldFilePath, System.Drawing.Imaging.ImageFormat.Jpeg);  //保存压缩后的图片
            }
            imgPhoto.Dispose();
            return result;
        }


        /// <summary>
        /// 通过FileStream 来打开文件，这样就可以实现不锁定Image文件，到时可以让多用户同时访问Image文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Image 读取图片(string path)
        {
            FileStream fs = File.OpenRead(path); //OpenRead
            int filelength = 0;
            filelength = (int)fs.Length; //获得文件长度 
            Byte[] image = new Byte[filelength]; //建立一个字节数组 
            fs.Read(image, 0, filelength); //按字节流读取 
            System.Drawing.Image result = System.Drawing.Image.FromStream(fs);
            fs.Close();
            return result;
        }


        /// <summary>
        ///获取图片格式
        /// </summary>
        /// <param name="img">图片</param>
        /// <returns>默认返回JPEG</returns>
        public static ImageFormat 取得图片格式(Image img)
        {
            if (img.RawFormat.Equals(ImageFormat.Jpeg))
            {
                return ImageFormat.Jpeg;
            }
            if (img.RawFormat.Equals(ImageFormat.Gif))
            {
                return ImageFormat.Gif;
            }
            if (img.RawFormat.Equals(ImageFormat.Png))
            {
                return ImageFormat.Png;
            }
            if (img.RawFormat.Equals(ImageFormat.Bmp))
            {
                return ImageFormat.Bmp;
            }
            return ImageFormat.Jpeg;//根据实际情况选择返回指定格式还是null
        }

    }


}

