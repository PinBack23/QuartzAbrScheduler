using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace Abraxas.Scheduler.TestService
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(x =>                                   //1
            {
                x.Service<OwinHosting>(s =>                      //2
                {
                    s.ConstructUsing(name => new OwinHosting()); //3
                    s.WhenStarted(tc => tc.Start());               //4
                    s.WhenStopped(tc => tc.Stop());                //5
                });


                x.RunAsLocalSystem();                              //6
                x.SetDescription("abraxas Scheduler Host");           //7
                x.SetDisplayName("abraxas Scheduler Host");   //8
                x.SetServiceName("abraxasSchedulerHost");          //9

            });
        }
    }
}
