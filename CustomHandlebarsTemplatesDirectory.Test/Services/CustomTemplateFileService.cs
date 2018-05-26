using System.IO;
using EntityFrameworkCore.Scaffolding.Handlebars;

namespace CustomHandlebarsTemplatesDirectory.Test.Services
{
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
}