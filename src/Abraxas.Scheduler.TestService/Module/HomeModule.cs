using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abraxas.Scheduler.TestService.Module
{
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            this.Get["/"] = v => this.View["index.html"];
        }
    }
}
