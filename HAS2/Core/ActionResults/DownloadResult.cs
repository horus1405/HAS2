using System.Linq;
using System.Web.Mvc;

namespace HAS2.Core.ActionResults
{
    public class DownloadResult : ActionResult
    {

        public string VirtualPath { get; set; }
        public string FileDownloadName { get; set; }
        public string MimeType { get; set; }


        public DownloadResult(string virtualPath, string mimeType, string downloadName = null)
        {
            VirtualPath = virtualPath;
            MimeType = mimeType;
            if (string.IsNullOrEmpty(downloadName))
            {
                var pos = virtualPath.LastIndexOf("/", System.StringComparison.InvariantCultureIgnoreCase);
                downloadName = virtualPath.Substring(pos + 1);
            }
           
            FileDownloadName = downloadName;
        }


        public override void ExecuteResult(ControllerContext context)
        {
            if (!string.IsNullOrEmpty(FileDownloadName))
            {
                context.HttpContext.Response.AddHeader("content-disposition", "attachment; filename=" + FileDownloadName);
                if(!string.IsNullOrEmpty(MimeType))
                    context.HttpContext.Response.AddHeader("Content-type", MimeType);
            }

            var filePath = context.HttpContext.Server.MapPath(VirtualPath);
            context.HttpContext.Response.TransmitFile(filePath);
        }
    }
}