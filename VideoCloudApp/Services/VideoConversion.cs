using FFmpeg.NET;
using FFmpeg.NET.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VideoCloudApp.Models;

namespace VideoCloudApp.Services
{
    public class VideoConversion
    {
        private static readonly Engine ffmpeg = new Engine(@"C:\Users\ASUS\Desktop\ffmpeg-4.4-full_build\bin\ffmpeg.exe");
        private static readonly ConversionOptions conversionOptionsFor1080 = new ConversionOptions { VideoSize = VideoSize.Hd1080 };
        private static readonly ConversionOptions conversionOptionsFor720 = new ConversionOptions { VideoSize = VideoSize.Hd720 };
        private static readonly ConversionOptions conversionOptionsFor480 = new ConversionOptions { VideoSize = VideoSize.Hd480 };

        public async Task<VideoInfo> GetVideoResolution(string OriginalFilePath)
        {

            MediaFile OriginalFile = new(OriginalFilePath);
            var inputFileVideoData = await ffmpeg.GetMetaDataAsync(OriginalFile);
            int[] VideoFrameSize = Array.ConvertAll(inputFileVideoData.VideoData.FrameSize.Split('x'), s => int.Parse(s));
            VideoInfo videoResolution = new(VideoFrameSize[0], VideoFrameSize[1]);

            return videoResolution;
        }
        //c/nanana/projekt/nanana/uploads/guid/file.mp4 <- OrigianFilePath <- the file which is converted
        //c/nanana/projekt/nanana/uploads/guid/ <- The directory where all converted files will be
        public async Task<Dictionary<int, string>> DoAllConversions(string OriginalFileDirectory, string OriginalFilePath, string FileGuid)
        {
            string upload = "upload";
            #region paths 
            string x1080path = Path.Combine(OriginalFileDirectory, "1080");
            string x720path = Path.Combine(OriginalFileDirectory, "720");
            string x480path = Path.Combine(OriginalFileDirectory, "480/");
            string basic = Path.Combine(OriginalFileDirectory, "basic");
            #endregion

            #region mediafile directories
            MediaFile _1080path = new MediaFile(x1080path + "\\1080.mp4");
            MediaFile _720path = new MediaFile(x720path + "\\720.mp4");
            MediaFile _480path = new MediaFile(x480path + "\\480.mp4");
            MediaFile basicpath = new MediaFile(basic + "\\basic.mp4");
            #endregion

            MediaFile input = new(OriginalFilePath);
            Dictionary<int, string> ConvertedVideosPaths = new Dictionary<int, string>();
            ConvertedVideosPaths.Add(1080, "");
            ConvertedVideosPaths.Add(720, "");
            ConvertedVideosPaths.Add(480, "");
            ConvertedVideosPaths.Add(0, "");
            var videoResolution = await GetVideoResolution(OriginalFilePath);

            
            if (videoResolution.Width >= 1920 || videoResolution.Height >= 1920)
            {
                Directory.CreateDirectory(x1080path);
                await ffmpeg.ConvertAsync(input, _1080path, conversionOptionsFor1080);
                ConvertedVideosPaths[1080] = string.Concat("/uploads/", FileGuid.ToString(), "/1080", "/1080.mp4");

                Directory.CreateDirectory(x720path);
                await ffmpeg.ConvertAsync(input, _720path, conversionOptionsFor720);
                ConvertedVideosPaths[720] = string.Concat("/uploads/", FileGuid.ToString(), "/720", "/720.mp4");

                Directory.CreateDirectory(x480path);
                await ffmpeg.ConvertAsync(input, _480path, conversionOptionsFor480);
                ConvertedVideosPaths[480] = string.Concat("/uploads/", FileGuid.ToString(), "/480", "/480.mp4");

                Directory.CreateDirectory(basic);
                await ffmpeg.ConvertAsync(input, basicpath);
                ConvertedVideosPaths[0] = string.Concat("/uploads/", FileGuid.ToString(), "/basic", "/basic.mp4");
            }
            else if (videoResolution.Width >= 1080 || videoResolution.Height >= 1080)
            {
                Directory.CreateDirectory(x720path);
                await ffmpeg.ConvertAsync(input, _720path, conversionOptionsFor720);
                ConvertedVideosPaths[720] = string.Concat("/uploads/", FileGuid.ToString(), "/720", "/720.mp4");

                Directory.CreateDirectory(x480path);
                await ffmpeg.ConvertAsync(input, _480path, conversionOptionsFor480);
                ConvertedVideosPaths[480] = string.Concat("/uploads/", FileGuid.ToString(), "/480", "/480.mp4");

                Directory.CreateDirectory(basic);
                await ffmpeg.ConvertAsync(input, basicpath);
                ConvertedVideosPaths[0] = string.Concat("/uploads/", FileGuid.ToString(), "/basic", "/basic.mp4");
            }
            else if (videoResolution.Width > 852 || videoResolution.Height > 852)
            {
                Directory.CreateDirectory(x480path);
                await ffmpeg.ConvertAsync(input, _480path, conversionOptionsFor480);
                ConvertedVideosPaths[480] = string.Concat("/uploads/", FileGuid.ToString(), "/480", "/480.mp4");

                Directory.CreateDirectory(basic);
                await ffmpeg.ConvertAsync(input, basicpath);
                ConvertedVideosPaths[0] = string.Concat("/uploads/", FileGuid.ToString(), "/basic", "/basic.mp4");
            }
            else
            {
                Directory.CreateDirectory(basic);
                await ffmpeg.ConvertAsync(input, basicpath);
                ConvertedVideosPaths[0] = string.Concat("/uploads/", FileGuid.ToString(), "/basic", "/basic.mp4");
            }
            

            return ConvertedVideosPaths;
        }
    }
}
