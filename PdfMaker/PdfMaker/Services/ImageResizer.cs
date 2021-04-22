﻿using System;
using System.IO;
using System.Threading.Tasks;
using PdfMaker.Services;
//#if __IOS__
//using System.Drawing;
//using UIKit;
//using CoreGraphics;
//#endif

//#if __ANDROID__
using Android.Graphics;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
//#endif

//#if WINDOWS_UWP
//using System.Threading.Tasks;
//using Windows.Storage.Streams;
//using Windows.Graphics.Imaging;
//using System.Runtime.InteropServices.WindowsRuntime;
//#endif

namespace XamFormsImageResize
{
    public static class ImageResizer
    {
        static ImageResizer()
        {
        }

        public static async Task<byte[]> ResizeImage(byte[] imageData)
        {
            //#if __IOS__
            //            return ResizeImageIOS(imageData, width, height);
            //#endif
            //#if __ANDROID__
            return await ResizeImageAndroid(imageData);
            //#endif
            //#if WINDOWS_UWP
            //            return await ResizeImageWindows(imageData, width, height);
            //#endif
        }


        //#if __IOS__
        //        public static byte[] ResizeImageIOS(byte[] imageData, float width, float height)
        //        {
        //            UIImage originalImage = ImageFromByteArray(imageData);
        //            UIImageOrientation orientation = originalImage.Orientation;

        //            //create a 24bit RGB image
        //            using (CGBitmapContext context = new CGBitmapContext(IntPtr.Zero,
        //                                                 (int)width, (int)height, 8,
        //                                                 4 * (int)width, CGColorSpace.CreateDeviceRGB(),
        //                                                 CGImageAlphaInfo.PremultipliedFirst))
        //            {

        //                RectangleF imageRect = new RectangleF(0, 0, width, height);

        //                // draw the image
        //                context.DrawImage(imageRect, originalImage.CGImage);

        //                UIKit.UIImage resizedImage = UIKit.UIImage.FromImage(context.ToImage(), 0, orientation);

        //                // save the image as a jpeg
        //                return resizedImage.AsJPEG().ToArray();
        //            }
        //        }

        //        public static UIKit.UIImage ImageFromByteArray(byte[] data)
        //        {
        //            if (data == null)
        //            {
        //                return null;
        //            }

        //            UIKit.UIImage image;
        //            try
        //            {
        //                image = new UIKit.UIImage(Foundation.NSData.FromArray(data));
        //            }
        //            catch (Exception e)
        //            {
        //                Console.WriteLine("Image load failed: " + e.Message);
        //                return null;
        //            }
        //            return image;
        //        }
        //#endif

        //#if __ANDROID__

        public async static Task<byte[]> ResizeImageAndroid(byte[] imageData)
        {
            // Load the bitmap
            Bitmap originalImage = BitmapFactory.DecodeByteArray(imageData, 0, imageData.Length);
            double ratio = (double)Settings.ImageResizeHeight / originalImage.Height;
            int newWidth = (int)(originalImage.Width * ratio);
            int newHeight = (int)(originalImage.Height * ratio);
            Bitmap resizedImage = Bitmap.CreateScaledBitmap(originalImage, newWidth, newHeight, false);

            using (MemoryStream ms = new())
            {
                await ResizeAndRotate(resizedImage, newWidth, newHeight).CompressAsync(Bitmap.CompressFormat.Jpeg, Settings.ImageCompressionQuality, ms);
                return ms.ToArray();
            }
        }
        public static Bitmap ResizeAndRotate(Bitmap image, int width, int height)
        {
            var matrix = new Matrix();
            var scaleWidth = ((float)width) / image.Width;
            var scaleHeight = ((float)height) / image.Height;
            matrix.PreRotate(Settings.ImagePreRotate);
            matrix.PreScale(scaleWidth, scaleHeight);
            return Bitmap.CreateBitmap(image, 0, 0, width, height, matrix, false);
        }
        //#endif

        //#if WINDOWS_UWP

        //        public static async Task<byte[]> ResizeImageWindows(byte[] imageData, float width, float height)
        //        {
        //            byte[] resizedData;

        //            using (var streamIn = new MemoryStream(imageData))
        //            {
        //                using (var imageStream = streamIn.AsRandomAccessStream())
        //                {
        //                    var decoder = await BitmapDecoder.CreateAsync(imageStream);
        //                    var resizedStream = new InMemoryRandomAccessStream();
        //                    var encoder = await BitmapEncoder.CreateForTranscodingAsync(resizedStream, decoder);
        //                    encoder.BitmapTransform.InterpolationMode = BitmapInterpolationMode.Linear;
        //                    encoder.BitmapTransform.ScaledHeight = (uint)height;
        //                    encoder.BitmapTransform.ScaledWidth = (uint)width;
        //                    await encoder.FlushAsync();
        //                    resizedStream.Seek(0);
        //                    resizedData = new byte[resizedStream.Size];
        //                    await resizedStream.ReadAsync(resizedData.AsBuffer(), (uint)resizedStream.Size, InputStreamOptions.None);                  
        //                }                
        //            }

        //            return resizedData;
        //        }

        //#endif
    }
}