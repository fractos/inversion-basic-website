using System;
using System.Collections.Generic;

using Inversion.Process;
using Inversion.Process.Behaviour;
using Inversion.Process.Pipeline;
using Inversion.Razor.Plugins;
using Inversion.Web.Behaviour;
using Inversion.Web.Behaviour.View;

namespace HelloInversion.Code
{
    public class Pipeline : IPipelineProvider
    {
        public void Register(IServiceContainerRegistrar registrar, IDictionary<string, string> settings)
        {
            registrar.RegisterService("application-behaviours",
                container => new List<IProcessBehaviour>
                {
                });

            registrar.RegisterService("request-behaviours",
                container => new List<IProcessBehaviour>
                {
                    new ParameterisedSequenceBehaviour("process-request",
                        new Configuration.Builder {
                            {"fire", "bootstrap"},
                            {"fire", "parse-request"},
                            {"fire", "dispatch"},
                            {"fire", "view-state"},
                            {"fire", "process-views"},
                            {"fire", "render"}
                        }),
                    new BootstrapBehaviour("bootstrap",
                        new Configuration.Builder {
                            {"context", "set", "area", "default"},
                            {"context", "set", "concern", "default"},
                            {"context", "set", "action", "default"},
                            {"context", "set", "appPath", String.Empty}
                        }),
                    new ParseRequestBehaviour("parse-request", String.Empty),
                    new ViewStateBehaviour("view-state"),
                    new ProcessViewsBehaviour("process-views",
                        new Configuration.Builder
                        {
                            {"config", "default-view", "rzr"}
                        }),
                    new RenderBehaviour("render"),

                    new Inversion.Razor.Behaviour.RazorViewBehaviour("rzr::view",
                        templatePlugins: new List<IRazorViewPlugin>
                        {
                            new RazorViewLayoutPlugin(
                                layoutPlugins: new List<IRazorViewPlugin>
                                {
                                    new RazorViewIncludePlugin()
                                }),
                            new RazorViewIncludePlugin()
                        }),
                    new XmlViewBehaviour("xml::view", "text/xml"),
                    new JsonViewBehaviour("json::view", "text/json"),

                });
        }
    }
}