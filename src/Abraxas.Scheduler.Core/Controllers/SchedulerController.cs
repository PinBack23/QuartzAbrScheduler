using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Net;
using System.Net.Http;
using System.IO;

namespace Abraxas.Scheduler.Core.Controllers
{
    public class SchedulerController : ApiController
    {
        public bool GetIsStartet()
        {
            return Scheduler.Instance.IsSchedulerStarted();
        }

        public void GetStartScheduler()
        {
            Scheduler.Instance.Start();
        }

        public void GetStopScheduler()
        {
            Scheduler.Instance.Shutdown();
        }

        public async Task<HttpResponseMessage> PostUploadJobs()
        {
            // Check if the request contains multipart/form-data.
            if (!this.Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            try
            {
                var loProvider = new MultipartFormDataStreamProvider(Constants.UploadDirectory);

                StringBuilder sb = new StringBuilder(); // Holds the response body

                // Read the MIME multipart asynchronously content using the stream provider we just created.
                await Request.Content.ReadAsMultipartAsync(loProvider);

                string lsFilename = loProvider.FormData.GetValues("filename").First();
                var loFile = loProvider.FileData.First();
                FileInfo loFileInfo = new FileInfo(loFile.LocalFileName);
                if (Path.GetExtension(lsFilename).ToUpper() == ".DLL")
                {
                    string lsDestinationFile = Path.Combine(Constants.JobDirectory, lsFilename);
                    File.Delete(lsDestinationFile);
                    loFileInfo.MoveTo(lsDestinationFile);
                }

                MefExporter.Instance.Recompose();
                 // This illustrates how to get the file names for uploaded files.
                //foreach (var file in loProvider.FileData)
                //{
                //    FileInfo fileInfo = new FileInfo(file.LocalFileName);
                //    sb.Append(string.Format("Uploaded file: {0} ({1} bytes)\n", fileInfo.Name, fileInfo.Length));
                //}

                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.ToString());
                return this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exp);
            }
        }

        public void GetReloadJobs()
        {
            MefExporter.Instance.Recompose();
            if (Scheduler.Instance.IsSchedulerStarted())
                Scheduler.Instance.Start();
        }
    }
}
