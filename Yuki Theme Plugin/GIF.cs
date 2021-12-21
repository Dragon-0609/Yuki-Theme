public static class GifExtension
    {
        public static TimeSpan? GetGifDuration(this Image image, int fps = 60)
        {
            var minimumFrameDelay = (1000.0/fps);
            if (!image.RawFormat.Equals(ImageFormat.Gif)) return null;
            if (!ImageAnimator.CanAnimate(image)) return null;

            var frameDimension = new FrameDimension(image.FrameDimensionsList[0]);

            var frameCount = image.GetFrameCount(frameDimension);
            var totalDuration = 0;

            for (var f = 0; f < frameCount; f++)
            {
                var delayPropertyBytes = image.GetPropertyItem(20736).Value;
                var frameDelay = BitConverter.ToInt32(delayPropertyBytes, f * 4) * 10;
                // Minimum delay is 16 ms. It's 1/60 sec i.e. 60 fps
                totalDuration += (frameDelay < minimumFrameDelay ? (int) minimumFrameDelay : frameDelay);
            }

            return TimeSpan.FromMilliseconds(totalDuration);
        }
    }