using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace PandaClass
{
    /// <summary>
    /// 图片操作
    /// </summary>
    public class ImageLibrary
    {
        /// <summary>
        /// 打水印
        /// </summary>
        /// <param name="FilePath">源文件</param>
        /// <param name="SavePath">保存地址</param>
        /// <param name="WaterPath">水印地址</param>
        /// <returns></returns>
        public static Boolean Water(String FilePath, String SavePath, String watermarkImage)
        {
            Bitmap newImage = new Bitmap(FilePath);
            //透明图片水印
            if (watermarkImage != "")
            {
                if (System.IO.File.Exists(watermarkImage))
                {
                    //获取水印图片
                    using (System.Drawing.Image wrImage = System.Drawing.Image.FromFile(watermarkImage))
                    {
                        //水印绘制条件：原始图片宽高均大于或等于水印图片
                        if (newImage.Width >= wrImage.Width && newImage.Height >= wrImage.Height)
                        {
                            Graphics gWater = Graphics.FromImage(newImage);

                            //透明属性
                            ImageAttributes imgAttributes = new ImageAttributes();
                            ColorMap colorMap = new ColorMap();
                            colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
                            colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);
                            ColorMap[] remapTable = { colorMap };
                            imgAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);

                            float[][] colorMatrixElements = {
                                   new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f},
                                   new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f},
                                   new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f},
                                   new float[] {0.0f,  0.0f,  0.0f,  0.5f, 0.0f},//透明度:0.5
                                   new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}
                                };

                            ColorMatrix wmColorMatrix = new ColorMatrix(colorMatrixElements);
                            imgAttributes.SetColorMatrix(wmColorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                            gWater.DrawImage(wrImage, new Rectangle(newImage.Width - wrImage.Width, newImage.Height - wrImage.Height, wrImage.Width, wrImage.Height), 0, 0, wrImage.Width, wrImage.Height, GraphicsUnit.Pixel, imgAttributes);
                            gWater.Dispose();
                        }
                        newImage.Save(SavePath);
                        wrImage.Dispose();
                    }
                }
            }
            return true;
        }
        #region 固定模版裁剪并缩放
        /// <summary>
        /// 指定长宽裁剪
        /// 按模版比例最大范围的裁剪图片并缩放至模版尺寸
        /// </summary>
        /// <remarks>吴剑 2010-11-15</remarks>
        /// <param name="postedFile">原图HttpPostedFile对象</param>
        /// <param name="fileSaveUrl">保存路径</param>
        /// <param name="maxWidth">最大宽(单位:px)</param>
        /// <param name="maxHeight">最大高(单位:px)</param>
        /// <param name="quality">质量（范围0-100）</param>
        public static void CutForCustom(string fromfilepath, string fileSaveUrl, int maxWidth, int maxHeight, int quality)
        {
            if (System.IO.File.Exists(fromfilepath))
            {

                string dir = fileSaveUrl.Substring(0, fileSaveUrl.LastIndexOf('\\'));
                if (!System.IO.Directory.Exists(dir))
                {
                    System.IO.Directory.CreateDirectory(dir);
                }
                //从文件获取原始图片，并使用流中嵌入的颜色管理信息
                System.Drawing.Image initImage = System.Drawing.Image.FromFile(fromfilepath, true);

                //原图宽高均小于模版，不作处理，直接保存
                if (initImage.Width <= maxWidth && initImage.Height <= maxHeight)
                {
                    initImage.Save(fileSaveUrl, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
                else
                {
                    //模版的宽高比例
                    double templateRate = (double)maxWidth / maxHeight;
                    //原图片的宽高比例
                    double initRate = (double)initImage.Width / initImage.Height;

                    //原图与模版比例相等，直接缩放
                    if (templateRate == initRate)
                    {
                        //按模版大小生成最终图片
                        System.Drawing.Image templateImage = new System.Drawing.Bitmap(maxWidth, maxHeight);
                        System.Drawing.Graphics templateG = System.Drawing.Graphics.FromImage(templateImage);
                        templateG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                        templateG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        templateG.Clear(Color.White);
                        templateG.DrawImage(initImage, new System.Drawing.Rectangle(0, 0, maxWidth, maxHeight), new System.Drawing.Rectangle(0, 0, initImage.Width, initImage.Height), System.Drawing.GraphicsUnit.Pixel);
                        templateImage.Save(fileSaveUrl, System.Drawing.Imaging.ImageFormat.Jpeg);
                    }
                    //原图与模版比例不等，裁剪后缩放
                    else
                    {
                        //裁剪对象
                        System.Drawing.Image pickedImage = null;
                        System.Drawing.Graphics pickedG = null;

                        //定位
                        Rectangle fromR = new Rectangle(0, 0, 0, 0);//原图裁剪定位
                        Rectangle toR = new Rectangle(0, 0, 0, 0);//目标定位

                        //宽为标准进行裁剪
                        if (templateRate > initRate)
                        {
                            //裁剪对象实例化
                            pickedImage = new System.Drawing.Bitmap(initImage.Width, (int)Math.Floor(initImage.Width / templateRate));
                            pickedG = System.Drawing.Graphics.FromImage(pickedImage);

                            //裁剪源定位
                            fromR.X = 0;
                            fromR.Y = (int)Math.Floor((initImage.Height - initImage.Width / templateRate) / 2);
                            fromR.Width = initImage.Width;
                            fromR.Height = (int)Math.Floor(initImage.Width / templateRate);

                            //裁剪目标定位
                            toR.X = 0;
                            toR.Y = 0;
                            toR.Width = initImage.Width;
                            toR.Height = (int)Math.Floor(initImage.Width / templateRate);
                        }
                        //高为标准进行裁剪
                        else
                        {
                            pickedImage = new System.Drawing.Bitmap((int)Math.Floor(initImage.Height * templateRate), initImage.Height);
                            pickedG = System.Drawing.Graphics.FromImage(pickedImage);

                            fromR.X = (int)Math.Floor((initImage.Width - initImage.Height * templateRate) / 2);
                            fromR.Y = 0;
                            fromR.Width = (int)Math.Floor(initImage.Height * templateRate);
                            fromR.Height = initImage.Height;

                            toR.X = 0;
                            toR.Y = 0;
                            toR.Width = (int)Math.Floor(initImage.Height * templateRate);
                            toR.Height = initImage.Height;
                        }

                        //设置质量
                        pickedG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        pickedG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                        //裁剪
                        pickedG.DrawImage(initImage, toR, fromR, System.Drawing.GraphicsUnit.Pixel);

                        //按模版大小生成最终图片
                        System.Drawing.Image templateImage = new System.Drawing.Bitmap(maxWidth, maxHeight);
                        System.Drawing.Graphics templateG = System.Drawing.Graphics.FromImage(templateImage);
                        templateG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                        templateG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        templateG.Clear(Color.White);
                        templateG.DrawImage(pickedImage, new System.Drawing.Rectangle(0, 0, maxWidth, maxHeight), new System.Drawing.Rectangle(0, 0, pickedImage.Width, pickedImage.Height), System.Drawing.GraphicsUnit.Pixel);

                        //关键质量控制
                        //获取系统编码类型数组,包含了jpeg,bmp,png,gif,tiff
                        ImageCodecInfo[] icis = ImageCodecInfo.GetImageEncoders();
                        ImageCodecInfo ici = null;
                        foreach (ImageCodecInfo i in icis)
                        {
                            if (i.MimeType == "image/jpeg" || i.MimeType == "image/bmp" || i.MimeType == "image/png" || i.MimeType == "image/gif")
                            {
                                ici = i;
                            }
                        }
                        EncoderParameters ep = new EncoderParameters(1);
                        ep.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)quality);

                        //保存缩略图
                        templateImage.Save(fileSaveUrl, ici, ep);
                        //templateImage.Save(fileSaveUrl, System.Drawing.Imaging.ImageFormat.Jpeg);

                        //释放资源
                        templateG.Dispose();
                        templateImage.Dispose();

                        pickedG.Dispose();
                        pickedImage.Dispose();
                    }
                }

                //释放资源
                initImage.Dispose();
            }
        }
        #endregion

        /// <summary>
        /// 缩略图生成
        /// </summary>
        /// <param name="FilePath">来源文件</param>
        /// <param name="W">缩略图宽度</param>
        /// <param name="H">缩略图高度</param>
        /// <returns></returns>
        public static Boolean Thumbnail(String FilePath, String SavePath, double targetWidth, double targetHeight)
        {
            if (System.IO.File.Exists(FilePath))
            {

                string dir = SavePath.Substring(0, SavePath.LastIndexOf('\\'));
                if (!System.IO.Directory.Exists(dir))
                {
                    System.IO.Directory.CreateDirectory(dir);
                }
                System.Drawing.Image initImage = System.Drawing.Image.FromFile(FilePath);
                System.IO.Directory.CreateDirectory(SavePath.Substring(0, SavePath.LastIndexOf('\\')));
                //缩略图宽、高计算
                double newWidth = initImage.Width;
                double newHeight = initImage.Height;

                //宽大于高或宽等于高（横图或正方）
                if (initImage.Width > initImage.Height || initImage.Width == initImage.Height)
                {
                    //如果宽大于模版
                    if (initImage.Width > targetWidth)
                    {
                        //宽按模版，高按比例缩放
                        newWidth = targetWidth;
                        newHeight = initImage.Height * (targetWidth / initImage.Width);
                    }
                }
                //高大于宽（竖图）
                else
                {
                    //如果高大于模版
                    if (initImage.Height > targetHeight)
                    {
                        //高按模版，宽按比例缩放
                        newHeight = targetHeight;
                        newWidth = initImage.Width * (targetHeight / initImage.Height);
                    }
                }

                //生成新图
                //新建一个bmp图片
                System.Drawing.Image newImage = new System.Drawing.Bitmap((int)newWidth, (int)newHeight);
                //新建一个画板
                System.Drawing.Graphics newG = System.Drawing.Graphics.FromImage(newImage);

                //设置质量
                newG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                newG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                //置背景色
                newG.Clear(Color.White);
                //画图
                newG.DrawImage(initImage, new System.Drawing.Rectangle(0, 0, newImage.Width, newImage.Height), new System.Drawing.Rectangle(0, 0, initImage.Width, initImage.Height), System.Drawing.GraphicsUnit.Pixel);

                //保存缩略图
                newImage.Save(SavePath, System.Drawing.Imaging.ImageFormat.Jpeg);

                //释放资源
                newG.Dispose();
                newImage.Dispose();
                initImage.Dispose();
                return true;
            }
            return false;
            //try
            //{

            //    _Graphics.DrawImage(SourceImg, 0, 0, W, H);
            //    ImageFormat IFM = null;
            //    String FileType = FilePath.Substring(FilePath.LastIndexOf('.') + 1, FilePath.Length - FilePath.LastIndexOf('.') - 1);
            //    switch (FileType.ToUpper())
            //    {
            //        case "GIF":
            //            {
            //                IFM = System.Drawing.Imaging.ImageFormat.Gif;
            //            }
            //            break;
            //        case "JPG":
            //            {
            //                IFM = System.Drawing.Imaging.ImageFormat.Jpeg;
            //            }
            //            break;
            //        case "PNG":
            //            {
            //                IFM = System.Drawing.Imaging.ImageFormat.Png;
            //            }
            //            break;
            //        case "JPEG":
            //            {
            //                IFM = System.Drawing.Imaging.ImageFormat.Jpeg;
            //            }
            //            break;
            //    }
            //    Bmp.Save(SavePath, IFM);
            //    SourceImg.Dispose();
            //    Bmp.Dispose();
            //    _Graphics.Dispose();
            //    return true;
            //}
            //catch
            //{
            //    return false;
            //}
        }

        /// <summary>
        /// 图片剪裁
        /// </summary>
        /// <param name="Path">源图片</param>
        /// <param name="SavePath">保存地址</param>
        /// <param name="x">起始X坐标</param>
        /// <param name="y">起始Y坐标</param>
        /// <param name="w">宽度</param>
        /// <param name="h">高度</param>
        public static void CutImg(String Path, String SavePath, int x, int y, int w, int h)
        {
            if (System.IO.File.Exists(Path))
            {
                string dir = SavePath.Substring(0, SavePath.LastIndexOf('\\'));
                if (!System.IO.Directory.Exists(dir))
                {
                    System.IO.Directory.CreateDirectory(dir);
                }
                Bitmap Bmp = new Bitmap(w, h, PixelFormat.Format24bppRgb);
                Bitmap SourceBmp = new Bitmap(Path);
                Graphics _G = Graphics.FromImage(Bmp);
                _G.DrawImage(SourceBmp, new Rectangle(0, 0, w, h), new Rectangle(x, y, w, h), GraphicsUnit.Pixel);
                _G.Dispose();
                SourceBmp.Dispose();
                Bmp.Save(SavePath, ImageFormat.Jpeg);
                Bmp.Dispose();
            }
        }
        /// <summary>
        /// 获取指定mimeType的ImageCodecInfo
        /// </summary>
        private static ImageCodecInfo GetImageCodecInfo(string mimeType)
        {
            ImageCodecInfo[] CodecInfo = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo ici in CodecInfo)
            {
                if (ici.MimeType == mimeType)
                    return ici;
            }
            return null;
        }
    }


}
