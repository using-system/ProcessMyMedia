namespace ProcessMyMedia.Tests
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using WorkflowCore.Interface;

    using Moq;

    using ProcessMyMedia.Services.Contract;


    [TestClass]
    [TestCategory("Unit")]
    public class UnitTestBase<TWorkflow, TData> : TestBase<TWorkflow, TData>
        where TWorkflow : IWorkflow<TData>, new()
        where TData : class, new()
    {
        protected Mock<IMediaService> mediaService;

        public UnitTestBase()
        {
            this.mediaService = new Mock<IMediaService>();
        }

        protected override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);

            services.AddMediaTasks();
            services.AddTransient<IMediaService>(provider => this.mediaService.Object);
        }
    }
}
