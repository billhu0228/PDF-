using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DrawingDataChange
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
            {
                string resourceName = "DrawingDataChange.DLL" + new AssemblyName(args.Name).Name + ".dll";

                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
                {
                    Byte[] assemblyData = new Byte[stream.Length];
                    stream.Read(assemblyData, 0, assemblyData.Length);
                    return Assembly.Load(assemblyData);
                }
            };

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            //AddTextField();



        }

        

        public static void GeneratePDF(string filename, string imageLoc,string savename)
        {
            PdfDocument document = PdfReader.Open(filename);
            int numbers = document.PageCount;
            //numbers = 3;
            
            for(int i = 0; i < numbers; i++)
            {
                PdfPage page = document.Pages[i];
                XGraphics gfx = XGraphics.FromPdfPage(page);
                DrawImage(gfx, imageLoc, 1106, 786);
            }           
            document.Save(savename);
            document.Close();
            document.Dispose();
        }

        static void DrawImage(XGraphics gfx, string jpegSamplePath, int x, int y, int width, int height)
        {
            XImage image = XImage.FromFile(jpegSamplePath);
            gfx.DrawImage(image, x, y, width, height);            
        }
        static void DrawImage(XGraphics gfx, string jpegSamplePath, int x, int y)
        {
            XImage image = XImage.FromFile(jpegSamplePath);
            gfx.DrawImage(image, x, y, image.PointWidth, image.PointHeight);
        }





    }
}
