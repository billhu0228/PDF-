using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatchDrawing
{
    public class PdfPatchRecord
    {
        public int start_page { get; set; }
        public int end_page { get; set; }
        public double rotation { get; set; }
        public string image1 { get; set; }
        public string image2 { get; set; }
        public string image3 { get; set; }
        public string image4 { get; set; }
        public string image5 { get; set; }
        public string image6 { get; set; }
    }
}
