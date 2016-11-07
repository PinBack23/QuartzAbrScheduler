using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abraxas.Scheduler.TestService
{
    public class OwinHosting
    {
        private IDisposable moDisposable = null;

        public void Start()
        {
            //string lsBaseAddress = "http://+:9788/";  //Starten als Amdmin über Konsole
            string lsBaseAddress = "http://localhost:9788/";
            this.moDisposable = WebApp.Start<Startup>(url: lsBaseAddress);
            //Abraxas.Scheduler.Core.Scheduler.Instance.Start();
            Console.WriteLine("Start now listening - {0}. Press ctrl-c to stop", lsBaseAddress);
        }

        public void Stop()
        {
            if (this.moDisposable != null)
                this.moDisposable.Dispose();
            Abraxas.Scheduler.Core.Scheduler.Instance.Shutdown();
            Console.WriteLine("Stopped. Good bye!");
        }
    }
}
