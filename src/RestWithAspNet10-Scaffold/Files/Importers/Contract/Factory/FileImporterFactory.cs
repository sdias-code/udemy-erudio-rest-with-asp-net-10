namespace RestWithAspNet10_Scaffold.Files.Importers.Contract.Factory
{
    public class FileImporterFactory
    {
        private readonly CsvImporter _csvImporter;
        private readonly XlsxImporter _xlsxImporter;
        private readonly ILogger<FileImporterFactory> _logger;

        public FileImporterFactory(
            CsvImporter csvImporter,
            XlsxImporter xlsxImporter,
            ILogger<FileImporterFactory> logger)
        {
            _csvImporter = csvImporter;
            _xlsxImporter = xlsxImporter;
            _logger = logger;
        }

        public IFileImporter GetImporter(string fileName)
        {
            if (fileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogInformation(
                    "Selected CSV file importer for file: {FileName}", fileName);

                return _csvImporter;
            }

            if (fileName.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogInformation(
                    "Selected Excel file importer for file: {FileName}", fileName);

                return _xlsxImporter;
            }

            _logger.LogError("Unsupported file format: {FileName}", fileName);
            throw new NotSupportedException(
                $"The file format of '{fileName}' is not supported.");
        }
    }
}