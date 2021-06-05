using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VideoCloudApp.Models
{
    public class VideoInfo
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public VideoInfo(int Width, int Height)
        {
            this.Width = Width;
            this.Height = Height;
        }
    }
}
