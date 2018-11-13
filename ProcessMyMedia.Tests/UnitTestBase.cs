namespace ProcessMyMedia.Tests
{
    using System;

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
        private Mock<IDelayService> delayService;

        protected  Mock<IMediaService> mediaService;

        public UnitTestBase()
        {
            this.delayService = new Mock<IDelayService>();
            this.mediaService = new Mock<IMediaService>();
        }

        protected override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);

            this.delayService.Setup(mock => mock.GetTimeToSleep(It.IsAny<DateTime>())).Returns(TimeSpan.Zero);

            services.AddMediaTasks();
            services.AddSingleton<IDelayService>(provider => this.delayService.Object);
            services.AddTransient<IMediaService>(provider => this.mediaService.Object);
        }
    }
}
