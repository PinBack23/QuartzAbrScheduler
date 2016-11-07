using Abraxas.Scheduler.Core.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Abraxas.Scheduler.Core.Controllers
{
    public class LogController : ApiController
    {
        public IEnumerable<LogEntry> GetLogs()
        {
            DirectoryInfo loDirectoryInfo = new DirectoryInfo(Constants.LogDirectory);
            foreach (var loFileInfo in loDirectoryInfo.GetFiles("*.log", SearchOption.AllDirectories))
            {
                yield return new LogEntry()
                {
                    FullName = loFileInfo.FullName,
                    FileName = loFileInfo.Name,
                    LastModification = loFileInfo.LastWriteTime
                };
            }
        }

        public string GetLogContent(string psFileName)
        {
            return File.ReadAllText(psFileName, Encoding.GetEncoding("ISO-8859-1"));
        }

        //public HttpResponseMessage PostDownloadLogFile()
        //public HttpResponseMessage PostDownloadLogFile([FromBody]string psFileName)

        public class Param
        {
            public string psFileName { get; set; }
        }

        //public HttpResponseMessage PostDownloadLogFile([FromBody] dynamic poData)
        //public HttpResponseMessage PostDownloadLogFile(HttpRequestMessage poRequest)
        //public HttpResponseMessage PostDownloadLogFile([FromBody]string psFileName)
        //public HttpResponseMessage PostDownloadLogFile(HttpRequestMessage poRequest)
        //public HttpResponseMessage PostDownloadLogFile(HttpRequestMessage poRequest)
        public HttpResponseMessage PostDownloadLogFile(JObject poParameter)
        {

            string psFileName = poParameter["psFileName"].Value<string>();  //poParam.psFileName; //psFileName is set correct

            HttpResponseMessage loResult = new HttpResponseMessage(HttpStatusCode.OK);
            var loStream = new FileStream(psFileName, FileMode.Open);


            //var loFormData = poRequest.Content.ReadAsFormDataAsync().Result;
            //string psFileName = loFormData["psFileName"]; //psFileName is set correct
            loResult.Content = new StreamContent(loStream);
            loResult.Content.Headers.ContentType =
                new MediaTypeHeaderValue("application/octet-stream");
            loResult.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            loResult.Content.Headers.ContentDisposition.FileName = Path.GetFileName(psFileName);
            return loResult;
        }

    }
}
