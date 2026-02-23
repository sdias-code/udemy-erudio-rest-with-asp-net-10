namespace RestWithAspNet10_Scaffold.Files.Exporters.Contract.Factory
{
    public class FileExporterFactory
    {
        private readonly XlsxExporter _xlsxExporter;
        private readonly CsvExporter _csvExporter;
        private readonly ILogger<FileExporterFactory> _logger;

        public FileExporterFactory(
            XlsxExporter xlsxExporter,
            CsvExporter csvExporter,
            ILogger<FileExporterFactory> logger)
        {
            _xlsxExporter = xlsxExporter;
            _csvExporter = csvExporter;
            _logger = logger;
        }

        public IFileExporter GetExporter(string acceptHeader)
        {
            if (string.Equals(acceptHeader, MediaTypes.ApplicationXlsx, StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogInformation(
                    "Selected Excel file exporter for media type: {AcceptHeader}", acceptHeader);

                return _xlsxExporter;
            }

            if (string.Equals(acceptHeader, MediaTypes.ApplicationCsv, StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogInformation(
                    "Selected CSV file exporter for media type: {AcceptHeader}", acceptHeader);

                return _csvExporter;
            }

            _logger.LogError("Unsupported media type: {AcceptHeader}", acceptHeader);
            throw new NotSupportedException($"The media type of '{acceptHeader}' is not supported.");
        }
    }
}
