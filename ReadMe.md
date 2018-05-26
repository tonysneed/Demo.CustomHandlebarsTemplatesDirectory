# Custom Handlebars Templates Directory

## Setup

1. Download and extract the NorthwindSlim database: <http://bit.lynorthwindslim>
2. Connect to SQL Server LocalDb: `(localdb)\MsSqlLocalDb`
3. Create a new database named **NorthwindSlim**.
4. Execute **NorthwindSlim.sql** to create tables and populate them with data.
5. Run the unit test: `Scaffolder_Should_Use_Custom_Templates_Directory`

## Instructions

1. Create a .NET Core Class Library project.
2. Add the NuGet package for scaffolding with Handlebars templates: `EntityFrameworkCore.Scaffolding.Handlebars` (prerelease)
3. Add EF Core SQL Server pacakge: `Microsoft.EntityFrameworkCore.SqlServer`
4. Create a class that extends `FileSystemTemplateFileService` and override `RetrieveTemplateFileContents`

    ```csharp
    public class CustomTemplateFileService : FileSystemTemplateFileService
    {
        public string RootDirectory { get; }

        public CustomTemplateFileService(string rootDirectory)
        {
            RootDirectory = rootDirectory;
        }

        public override string RetrieveTemplateFileContents(
            string relativeDirectory,
            string fileName,
            string altRelativeDirectory = null)
        {
            var localDirectory = Path.Combine(RootDirectory, relativeDirectory);
            var contents = RetrieveFileContents(localDirectory, fileName);
            return contents;
        }
    }
    ```
5. Add a class that implements `IDesignTimeServices`, in which you call `AddHandlebarsScaffolding`.
    - Register `ITemplateFileService` with `CustomTemplateFileService`.
    - Specify a root directory where the **CodeTemplates** folder is located.

    ```csharp
    public class ScaffoldingDesignTimeServices : IDesignTimeServices
    {
        public void ConfigureDesignTimeServices(IServiceCollection services)
        {
            // Specify any root directory you wish
            var projectRootDir = Path.Combine("..", "..", "..");
            var options = ReverseEngineerOptions.DbContextAndEntities;
            services.AddHandlebarsScaffolding(options)
                .AddSingleton<ITemplateFileService>(provider => new CustomTemplateFileService(projectRootDir));
        }
    }
    ```
