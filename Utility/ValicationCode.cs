using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hack.Fast.Extensions.Utility
{
    public class ValicationCode
    {
        public static Bitmap GetCodeMap(string checkCode)
        {
            Bitmap myImage = new Bitmap(80, 30); //生成一个位图  
            using (Graphics graphic = Graphics.FromImage(myImage)) //从一个位图生成一个画布  
            {
                graphic.Clear(Color.White); //清除图片背景色  
                Random random = new Random(); //生成随机生成器  
                //画图片的前景噪音点  
                for (int i = 0; i < 100; i++)
                {
                    int x = random.Next(myImage.Width);
                    int y = random.Next(myImage.Height);
                    myImage.SetPixel(x, y, Color.FromArgb(random.Next()));
                }
                //画图片的背景噪音线  
                for (int i = 0; i < 2; i++)
                {
                    int x1 = random.Next(myImage.Width);
                    int x2 = random.Next(myImage.Width);
                    int y1 = random.Next(myImage.Height);
                    int y2 = random.Next(myImage.Height);
                    graphic.DrawLine(new Pen(Color.Black), x1, y1, x2, y2);
                }
                Font font = new Font("Arial", 15, FontStyle.Bold);
                using (
                    System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.
                        LinearGradientBrush(
                        new Rectangle(0, 0, myImage.Width, myImage.Height), Color.Blue, Color.DarkRed, 1.2f, true))
                {
                    graphic.DrawString(checkCode, font, brush, 2, 2);
                }
                //画图片的边框线  
                graphic.DrawRectangle(new Pen(Color.Silver), 0, 0, myImage.Width - 1, myImage.Height - 1);
                return myImage;
            }
        }
        /// <summary>  
        /// 动态生成4个随机数或字母  
        /// </summary>  
        /// <returns>返回验证码字符串</returns>  
        public static string GenerateCheckCode()
        {
            //动态生成4个随机数或字母  
            int number; //定义变量  
            char code;
            string checkCode = String.Empty; //空字符串，只读  
            Random random = new Random(); //定义随机变量实例  

            for (int i = 1; i < 5; i++)
            {
                //利用for循环生成四个随机数或字母  
                number = random.Next(); //返回一个小于指定的最大值的非负的随机数 next有三个构造函数   
                if (number % 2 == 0)
                {
                    //产生1个1位数  
                    code = (char)('0' + (char)(number % 10));
                }
                else
                {
                    //产生1个大写字母  
                    code = (char)('A' + (char)(number % 26));
                }
                checkCode += code.ToString();
            }
            return checkCode.Replace("0","6").Replace("O","P").Replace("1","2").Replace("I","E");
        }

    }
}
