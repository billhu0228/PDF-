using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

using CsvHelper;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace PatchDrawing
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        public static void PatchPdfFile(string source_file_path)
        {
            FileInfo source_file = new FileInfo(source_file_path);

            // Read CSV
            string csv_file_path = Path.ChangeExtension(source_file.FullName, ".csv");
            TextReader textReader = File.OpenText(csv_file_path);
            CsvReader csv = new CsvReader(textReader);
            while (csv.Read())
            {
                var record = csv.GetRecord<PdfPatchRecord>();

                int start_page = record.start_page - 1;
                int end_page = record.end_page - 1;

                FileInfo destination_file = new FileInfo(Path.ChangeExtension(source_file.FullName, ".patched.pdf"));

                // Copy PDF file
                File.Copy(source_file.FullName, destination_file.FullName, true);

                if (record.image1 != null && record.image1 != "") {
                    FileInfo image1_file = new FileInfo(Path.Combine(source_file.DirectoryName, record.image1));
                    DrawImageOnPdfFile(destination_file, image1_file, destination_file, 596.2, 786, start_page, end_page);
                }

                if (record.image2 != null && record.image2 != "") {
                    FileInfo image2_file = new FileInfo(Path.Combine(source_file.DirectoryName, record.image2));
                    DrawImageOnPdfFile(destination_file, image2_file, destination_file, 681.2, 786, start_page, end_page);
                }

                if (record.image3 != null && record.image3 != "") {
                    FileInfo image3_file = new FileInfo(Path.Combine(source_file.DirectoryName, record.image3));
                    DrawImageOnPdfFile(destination_file, image3_file, destination_file, 766.5, 786, start_page, end_page);
                }

                if (record.image4 != null && record.image4 != "") {
                    FileInfo image4_file = new FileInfo(Path.Combine(source_file.DirectoryName, record.image4));
                    DrawImageOnPdfFile(destination_file, image4_file, destination_file, 851.5, 786, start_page, end_page);
                }

                if (record.image5 != null && record.image5 != "") {
                    FileInfo image5_file = new FileInfo(Path.Combine(source_file.DirectoryName, record.image5));
                    DrawImageOnPdfFile(destination_file, image5_file, destination_file, 936.2, 786, start_page, end_page);
                }

                if (record.image6 != null && record.image6 != "") {
                    FileInfo image6_file = new FileInfo(Path.Combine(source_file.DirectoryName, record.image6));
                    DrawImageOnPdfFile(destination_file, image6_file, destination_file, 1106.5, 786, start_page, end_page);
                }
            }
            csv.Dispose();
            textReader.Close();
            textReader.Dispose();
        }

        public static void DrawImageOnPdfFile(FileInfo pdf_file, FileInfo iamge_file, FileInfo output_file, double x, double y, int from_page = 0, int to_page = -1)
        {
            // Read PDF file
            PdfDocument document = PdfReader.Open(pdf_file.FullName);

            // Read image file
            XImage image = XImage.FromFile(iamge_file.FullName);

            DrawImageOnPdf(document, image, x, y, from_page, to_page);

            // Save output file
            document.Save(output_file.FullName);
            document.Close();
            document.Dispose();
        }

        public static void DrawImageOnPdf(PdfDocument document, XImage image, double x, double y, int from_page = 0, int to_page = -1)
        {
            int page_count = document.PageCount;
            if (to_page < 0)
            {
                to_page += page_count;
            }

            // Draw image on page
            for (int i = from_page; i <= to_page; i++)
            {
                PdfPage page = document.Pages[i];
                DrawImageOnPage(page, image, x, y);
            }
        }

        public static void DrawImageOnPage(PdfPage page, XImage image, double x, double y)
        {
            XGraphics gfx = XGraphics.FromPdfPage(page);

            // Translate coordinate system. To solve a strange problem.
            gfx.RotateTransform(-90);
            XRect rect = gfx.Transformer.WorldToDefaultPage(new XRect(new XPoint(0, 0), new XSize(0, 0)));
            gfx.TranslateTransform(-rect.Y, rect.X);

            gfx.DrawImage(image, x, y);
        }
    }
}
