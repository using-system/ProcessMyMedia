namespace ProcessMyMedia.Samples
{
    using System;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using ProcessMyMedia.Model;
    using WorkflowCore.Interface;

    public abstract class WofkflowSampleBase<TWorfklow, TWorkflowData> : SampleBase
        where TWorfklow : IWorkflow<TWorkflowData>, new()
        where TWorkflowData : class, new()

    {

        public WofkflowSampleBase(IConfigurationRoot configuration) : base(configuration)
        {
            this.configuration = configuration;
        }

        public override void Execute()
        {
            IServiceProvider serviceProvider = ConfigureServices();

            //start the workflow host
            var host = serviceProvider.GetService<IWorkflowHost>();
            host.RegisterWorkflow<TWorfklow, TWorkflowData>();
            host.Start();

            string result = host.StartWorkflow<TWorkflowData>(SampleBase.WORKFLOW_NAME, data: this.WorflowDatas).Result;

            Console.WriteLine(("Press Enter to stop the workflow host"));
            Console.ReadLine();

            host.Stop();
        }

        protected abstract TWorkflowData WorflowDatas { get; }

    }
}
