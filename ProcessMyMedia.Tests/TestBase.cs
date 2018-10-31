namespace ProcessMyMedia.Tests
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using WorkflowCore.Interface;
    using WorkflowCore.Testing;

    [TestClass]
    public abstract class TestBase<TWorkflow, TData> : WorkflowTest<TWorkflow, TData>
        where TWorkflow : IWorkflow<TData>, new()
        where TData : class, new()
    {
        protected override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);

            services.AddLogging();
        }
    }
}