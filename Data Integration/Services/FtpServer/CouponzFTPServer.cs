using Data_Integration.Models;
using Data_Integration.Services.RabbitMQ;
using System.Formats.Asn1;
using System.Globalization;
using ClosedXML.Excel;
using Microsoft.Data.SqlClient;
using DocumentFormat.OpenXml.Spreadsheet;
using OfficeOpenXml;

namespace Data_Integration.Services.FtpServer
{
    public class CouponzFTPServer : BackgroundService
    {
        private readonly ILogger<CouponzFTPServer> _logger;
        IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;


        public CouponzFTPServer(ILogger<CouponzFTPServer> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("CouponzFtpWorker running.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("Executing CouponzFTPServer.ReadFile at: {time}", DateTimeOffset.Now);
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var applicationContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                        ReadFile(applicationContext);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while executing CouponzFTPServer.ReadFile.");
                }

                await Task.Delay(TimeSpan.FromHours(1), stoppingToken); // Wait for 1 hour
            }
        }
        public List<SubscribeToOfferFTP> ReadFile(ApplicationDbContext applicationContext)
        {
            try
            {
                var localFilePath = "C:\\Users\\Radwa.Mohamed\\Downloads\\CouponzSheet.xlsx";
                var records = new List<SubscribeToOfferFTP>();

                using (var workbook = new XLWorkbook(localFilePath))
                {
                    var worksheet = workbook.Worksheet(1); 

                    // Find the header row
                    var headerRow = worksheet.FirstRowUsed();
                    var columnCount = headerRow.CellsUsed().Count();

                    // Define column indexes based on header names
                    var columnIndexMap = new Dictionary<string, int>();
                    for (int i = 1; i <= columnCount; i++)
                    {
                        var columnName = headerRow.Cell(i).GetString();
                        columnIndexMap[columnName] = i;
                    }

                    // Read data rows
                    var dataRows = worksheet.RangeUsed().RowsUsed().Skip(1);

                    foreach (var row in dataRows)
                    {
                        var record = new SubscribeToOfferFTP();

                        if (columnIndexMap.TryGetValue("MSISDN", out int msisdnIndex))
                        {
                            record.MSISDN = row.Cell(msisdnIndex).GetString();
                        }
                        if (columnIndexMap.TryGetValue("OfferNumbers", out int couponNumberIndex))
                        {
                            record.OfferNumbers = row.Cell(couponNumberIndex).GetString();
                        }
                        records.Add(record);
                        applicationContext.SubscribeToOffersFTP.Add(record);    
                    }
                    applicationContext.SaveChanges();
                    _logger.LogInformation("Save File Successfully into Database");
                    DeleteFile(localFilePath);

                }
                return records;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error reading CSV file" + ex.Message);
                throw;
            }
        }

        public void DeleteFile(string localFilePath)
        {
            try
            {
                if (File.Exists(localFilePath))
                {
                    File.Delete(localFilePath);
                    _logger.LogInformation("File is Deleted");
                }
            }catch(Exception ex)
            {
                _logger.LogError("Error in Deleting CSV file" + ex.Message);
                throw;
            }
        }
    }
}
