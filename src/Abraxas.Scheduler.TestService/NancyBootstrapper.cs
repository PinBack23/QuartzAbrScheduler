using Nancy;
using Nancy.Conventions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abraxas.Scheduler.TestService
{
    public class NancyBootstrapper : DefaultNancyBootstrapper
    {

        private byte[] favicon;

        protected override void ConfigureConventions(Nancy.Conventions.NancyConventions nancyConventions)
        {
            nancyConventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("scripts", "scripts"));
            nancyConventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("Content", "Content"));
            nancyConventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("fonts", "fonts"));
            nancyConventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("Pages", "Pages"));
            base.ConfigureConventions(nancyConventions);
        }

        protected override byte[] FavIcon
        {
            get { return this.favicon ?? (this.favicon = LoadFavIcon()); }
        }

        private byte[] LoadFavIcon()
        {
            using (MemoryStream loMemoryStream = new MemoryStream())
            {
                Abraxas.Scheduler.TestService.Properties.Resources.scheduler.Save(loMemoryStream);
                loMemoryStream.Flush();
                return loMemoryStream.ToArray();
            }
        }
    }
}
