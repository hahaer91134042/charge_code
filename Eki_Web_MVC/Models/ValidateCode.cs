using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevLibs;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace Eki_Web_MVC
{
    [Serializable]
    public class ValidateCode:IBaseDAO,ISession
    {
        //設定數字最大長度
        public const int CodeMaxLength = 10;

        //設定數字最小長度
        public const int CodeMinLength = 1;

        public string code;
        public byte[] img { get => CreateValidateGraphic(code); }
        private ValidateCode() { }
        private ValidateCode(string c):this()
        {
            code = c;
        }

        public static ValidateCode creat(int length = 4,RandomString input=null)
        {
            return new ValidateCode(creatValidCode(length, input));
        }

        public static ValidateCode fromSession() => new ValidateCode().getSession<ValidateCode>();


        public static double imageWidth(int validateNumLength)  //設定圖片寬度
        {
            //return (double)(validateNumLength * 12.0);
            return validateNumLength * 10.0;
        }

        public const int imageHeight = 26;   //設定圖片高度


        public static string creatValidCode(int length,RandomString input = null)
        {
            var s = input;
            if (s == null)
                s = RandomString.NumberString + RandomString.LowerString;
            var code= RandomUtil.GetRandomString(length, s);
            return code;
        }

        //產生驗證碼
        //public string CreateValidateCode(int length)    
        //{
        //    int[] randMembers = new int[length];
        //    int[] validateNums = new int[length];
        //    string validateNumberStr = "";
        //    int seekSeek = unchecked((int)DateTime.Now.Ticks);
        //    Random seekRand = new Random(seekSeek);

        //    int beginSeek = (int)seekRand.Next(0, Int32.MaxValue - length * 10000);
        //    int[] seeks = new int[length];
        //    for (int i = 0; i < length; i++)
        //    {
        //        beginSeek += 10000;
        //        seeks[i] = beginSeek;
        //    }

        //    //產生隨機數字
        //    for (int i = 0; i < length; i++)
        //    {
        //        Random rand = new Random(seeks[i]);
        //        int pownum = 1 * (int)Math.Pow(10, length);
        //        randMembers[i] = rand.Next(pownum, Int32.MaxValue);
        //    }

        //    //讀取隨機數字
        //    for (int i = 0; i < length; i++)
        //    {
        //        string numStr = randMembers[i].ToString();
        //        int numLength = numStr.Length;
        //        Random rand = new Random();
        //        int numPosition = rand.Next(0, numLength - 1);
        //        validateNums[i] = Int32.Parse(numStr.Substring(numPosition, 1));
        //    }
        //    //產生驗證碼
        //    for (int i = 0; i < length; i++)
        //    {
        //        validateNumberStr += validateNums[i].ToString();
        //    }
        //    return validateNumberStr;
        //}

        public static byte[] CreateValidateGraphic(string raw,string gap=" ")    //建立驗證碼圖片
        {
            var code = "";

            for (int i = 0; i < raw.Length; i++)
            {
                code += $"{raw[i]}{gap}";
            }

            Bitmap image = new Bitmap((int)Math.Ceiling(imageWidth(code.Length)), imageHeight);
            Graphics g = Graphics.FromImage(image);
            try
            {
                //產生隨機數字
                Random random = new Random();
                //清空圖片背景色
                g.Clear(Color.White);
                //畫圖片干擾線

                for (int i = 0; i < 25; i++)
                {
                    int x1 = random.Next(image.Width);
                    int x2 = random.Next(image.Width);
                    int y1 = random.Next(image.Height);
                    int y2 = random.Next(image.Height);
                    g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);

                }

                //Font font = new Font("Arial", 12, (FontStyle.Bold | FontStyle.Italic));
                Font font = new Font("Arial", 14, (FontStyle.Bold | FontStyle.Italic));
                LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height),
                 Color.Blue, Color.DarkRed, 1.2f, true);
                g.DrawString(code, font, brush, 3, 2);

                //畫圖片前景干擾線
                for (int i = 0; i < 100; i++)
                {
                    int x = random.Next(image.Width);
                    int y = random.Next(image.Height);
                    image.SetPixel(x, y, Color.FromArgb(random.Next()));
                }

                //畫圖片邊框線
                g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);

                //儲存圖片數據
                MemoryStream stream = new MemoryStream();
                image.Save(stream, ImageFormat.Jpeg);
                //輸出圖片
                return stream.ToArray();
            }

            finally
            {
                g.Dispose();
                image.Dispose();
            }

        }

        public string sessionFlag() => AppFlag.ValidCode;
    }
}