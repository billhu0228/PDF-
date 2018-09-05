using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CsvHelper;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace PatchDrawing
{
    class PdfService
    {
        public static void PatchPdfFile(string source_file_path, double page_rotation)
        {
            FileInfo source_file = new FileInfo(source_file_path);
            FileInfo destination_file = new FileInfo(Path.ChangeExtension(source_file.FullName, ".patched.pdf"));

            // Copy PDF file
            File.Copy(source_file.FullName, destination_file.FullName, true);

            // Read CSV
            string csv_file_path = Path.ChangeExtension(source_file.FullName, ".csv");
            TextReader textReader = File.OpenText(csv_file_path);
            CsvReader csv = new CsvReader(textReader);
            while (csv.Read())
            {
                var record = csv.GetRecord<PdfPatchRecord>();

                int start_page = record.start_page - 1;
                int end_page = record.end_page - 1;

                if (record.image1 != null && record.image1 != "")
                {
                    FileInfo image1_file = new FileInfo(Path.Combine(source_file.DirectoryName, record.image1));
                    DrawImageOnPdfFile(destination_file, image1_file, destination_file, 596.2, 786, start_page, end_page, page_rotation);
                }

                if (record.image2 != null && record.image2 != "")
                {
                    FileInfo image2_file = new FileInfo(Path.Combine(source_file.DirectoryName, record.image2));
                    DrawImageOnPdfFile(destination_file, image2_file, destination_file, 681.2, 786, start_page, end_page, page_rotation);
                }

                if (record.image3 != null && record.image3 != "")
                {
                    FileInfo image3_file = new FileInfo(Path.Combine(source_file.DirectoryName, record.image3));
                    DrawImageOnPdfFile(destination_file, image3_file, destination_file, 766.5, 786, start_page, end_page, page_rotation);
                }

                if (record.image4 != null && record.image4 != "")
                {
                    FileInfo image4_file = new FileInfo(Path.Combine(source_file.DirectoryName, record.image4));
                    DrawImageOnPdfFile(destination_file, image4_file, destination_file, 851.5, 786, start_page, end_page, page_rotation);
                }

                if (record.image5 != null && record.image5 != "")
                {
                    FileInfo image5_file = new FileInfo(Path.Combine(source_file.DirectoryName, record.image5));
                    DrawImageOnPdfFile(destination_file, image5_file, destination_file, 936.2, 786, start_page, end_page, page_rotation);
                }

                if (record.image6 != null && record.image6 != "")
                {
                    FileInfo image6_file = new FileInfo(Path.Combine(source_file.DirectoryName, record.image6));
                    DrawImageOnPdfFile(destination_file, image6_file, destination_file, 1106.5, 786, start_page, end_page, page_rotation);
                }
            }
            csv.Dispose();
            textReader.Close();
            textReader.Dispose();
        }

        public static void DrawImageOnPdfFile(FileInfo pdf_file, FileInfo iamge_file, FileInfo output_file, double x, double y, int from_page = 0, int to_page = -1, double page_rotation = 0)
        {
            // Read PDF file
            PdfDocument document = PdfReader.Open(pdf_file.FullName);

            // Read image file
            XImage image = XImage.FromFile(iamge_file.FullName);

            DrawImageOnPdf(document, image, x, y, from_page, to_page, page_rotation);

            // Save output file
            document.Save(output_file.FullName);
            document.Close();
            document.Dispose();
        }

        public static void DrawImageOnPdf(PdfDocument document, XImage image, double x, double y, int from_page = 0, int to_page = -1, double page_rotation = 0)
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
                DrawImageOnPage(page, image, x, y, page_rotation);
            }
        }

        public static void DrawImageOnPage(PdfPage page, XImage image, double x, double y, double page_rotation)
        {
            XGraphics gfx = XGraphics.FromPdfPage(page);

            // there is a bug for landscape PDF files
            Console.WriteLine("orientation {0}", page.Orientation);
            if (page.Orientation == PdfSharp.PageOrientation.Landscape)
            {
                // Translate coordinate system. To solve a strange problem.
                gfx.RotateTransform(-90);
                gfx.TranslateTransform(-page.Height, 0);

            }

            if (page_rotation != 0.0)
            {
                Console.WriteLine("rotate {0} degree", page_rotation);

                XPoint centerPoint = new XPoint(page.Width / 2, page.Height / 2);
                gfx.RotateAtTransform(page_rotation, centerPoint);
            }
            gfx.DrawImage(image, x, y);
        }
    }
}
