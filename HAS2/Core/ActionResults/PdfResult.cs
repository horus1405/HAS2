using System.Web;
using System.Web.Mvc;

namespace HAS2.Core.ActionResults
{
    public class PdfResult : FileContentResult
    {
        public const string PDFMIME = "application/pdf";


        public PdfResult(byte[] fileBytes, string fileName):base(fileBytes, PDFMIME)
        {
            FileDownloadName = fileName;
        }

        protected override void WriteFile(HttpResponseBase response)
        {
            base.WriteFile(response);
            if (string.IsNullOrEmpty(FileDownloadName)) return;

            response.ClearHeaders();

            response.AddHeader("content-disposition", "attachment; filename=" + FileDownloadName);
            response.AddHeader("Content-type", PDFMIME);
        }
    }

    public class ZipResult : FileContentResult
    {
        public const string MIME = "application/octet-stream";


        public ZipResult(byte[] fileBytes, string fileName)
            : base(fileBytes, MIME)
        {
            FileDownloadName = fileName;
        }

        protected override void WriteFile(HttpResponseBase response)
        {
            base.WriteFile(response);
            if (string.IsNullOrEmpty(FileDownloadName)) return;

            response.ClearHeaders();

            response.AddHeader("content-disposition", "attachment; filename=" + FileDownloadName);
            response.AddHeader("Content-type", MIME);
        }
    }
}