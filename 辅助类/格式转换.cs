using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 谱面转换器.辅助类
{
    public static class 音频转换
    {
        /// <summary>
        /// 将一段MP3音频文件转换到WAV
        /// </summary>
        /// <param name="待转换的MP3文件"></param>
        /// <param name="要输出的WAV文件"></param>
        public static void 从Mp3转换到Wav(string 待转换的MP3文件, string 要输出的WAV文件)
        {
            using (Mp3FileReader MP3读取器 = new Mp3FileReader(待转换的MP3文件))
            {
                using (WaveStream WAV流 = WaveFormatConversionStream.CreatePcmStream(MP3读取器))
                {
                    WaveFileWriter.CreateWaveFile(要输出的WAV文件, WAV流);
                }
            }
        }

        public static void 雀食专用_修改WAV二进制(string 待转换的Wav文件)
        {
            //除此之外雀食堂的谱面音乐似乎定义了奇怪的WAV文件头，需要读取二进制文档，从“00001144”位置开始复制数据
            //复制到00001180
            //以下为需要复制的二进制数据段
            /* 00 01 02 03 04 05 06 07 08 09 0A 0B 0C 0D 0E 0F
             *             FD FF FF FF FD FF FF FF FE FF FF FF
             * FC FF FE FF FD FF FE FF FD FF FE FF FD FF FF FF 
             * FD FF FF FD FF FE FF FE FF FF FF FD FD FF FF FF 
             * FD FF FF FF FD FF FE FF FD FF FE FF FE FF FF FF 
             *
             */
            using (BinaryWriter 二进制写入器 = new BinaryWriter(File.OpenWrite(待转换的Wav文件)))
            {
                // 设置起始的位置
                二进制写入器.Seek(0x1144, SeekOrigin.Begin);
                // Write FF until the end position
                for (int i = 0; i < 60; i++)
                {
                    二进制写入器.Write((byte)0xFF);
                }
            }
        }
    }
}
