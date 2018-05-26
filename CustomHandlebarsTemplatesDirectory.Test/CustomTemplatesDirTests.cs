using System.IO;
using System.Linq;
using CustomHandlebarsTemplatesDirectory.Test.Contexts;
using CustomHandlebarsTemplatesDirectory.Test.Helpers;
using CustomHandlebarsTemplatesDirectory.Test.Services;
using EntityFrameworkCore.Scaffolding.Handlebars;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.EntityFrameworkCore.SqlServer.Design.Internal;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CustomHandlebarsTemplatesDirectory.Test
{
    [Collection("NorthwindDbContext")]
    public partial class CustomTemplatesDirTests
    {
        private NorthwindDbContextFixture Fixture { get; }

        public CustomTemplatesDirTests(NorthwindDbContextFixture fixture)
        {
            Fixture = fixture;
            Fixture.Initialize();
        }

        [Fact]
        public void Scaffolder_Should_Use_Custom_Templates_Directory()
        {
            // Arrange

            // Specify root directory
            var projectRootDir = Path.Combine("..", "..", "..");

            var services = new ServiceCollection()
                .AddEntityFrameworkDesignTimeServices()
                .AddHandlebarsScaffolding()

                // Add custom template file service with root directory
                .AddSingleton<ITemplateFileService>(provider => new CustomTemplateFileService(projectRootDir));

            new SqlServerDesignTimeServices().ConfigureDesignTimeServices(services);
            var scaffolder = services
                .BuildServiceProvider()
                .GetRequiredService<IReverseEngineerScaffolder>();

            // Act
            var model = scaffolder.ScaffoldModel(
                connectionString: Connections.NorthwindTestConnection,
                tables: Enumerable.Empty<string>(),
                schemas: Enumerable.Empty<string>(),
                @namespace: Parameters.RootNamespace,
                language: "C#",
                contextName: Parameters.ContextName,
                modelOptions: new ModelReverseEngineerOptions(),
                contextDir: Parameters.ProjectPath,
                codeOptions: new ModelCodeGenerationOptions());

            // Assert
            Assert.Equal(ExpectedContexts.ContextClass, model.ContextFile.Code);
            Assert.Equal(ExpectedEntities.CategoryClass, model.AdditionalFiles[0].Code);
            Assert.Equal(ExpectedEntities.ProductClass, model.AdditionalFiles[1].Code);
        }
    }
}
