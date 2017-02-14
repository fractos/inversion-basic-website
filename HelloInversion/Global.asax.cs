using System;
using System.Collections.Generic;

using Inversion.Collections;
using Inversion.Data;
using Inversion.Naiad;
using Inversion.Process;
using Inversion.Process.Behaviour;
using Inversion.Process.Pipeline;

using HelloInversion.Code.Application;

namespace HelloInversion
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            IDictionary<string, string> settings = new Settings(ConfigurationHelper.GetConfiguration());

            PipelineConfigurationHelper.Configure(ServiceContainer.Instance, settings, new List<string>
            {
                settings["prototype-provider"],
                settings["pipeline-provider"],
                settings["storage-provider"]
            });

            IProcessContext context = new ProcessContext(ServiceContainer.Instance, FileSystemResourceAdapter.Instance);

            IEnumerable<IProcessBehaviour> behaviours = ServiceContainer.Instance.GetService<List<IProcessBehaviour>>("application-behaviours");
            context.Register(behaviours);

            context.Timers.Begin("application-start");
            context.Fire("application-start");
            context.Timers.End("application-start");
        }
    }
}