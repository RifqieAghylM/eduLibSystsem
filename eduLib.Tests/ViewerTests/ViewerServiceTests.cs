using Microsoft.VisualStudio.TestTools.UnitTesting;
using eduLib.Infrastructure.Viewer;
using System;
using System.Diagnostics;

namespace eduLib.Tests.ViewerTests
{
    [TestClass]
    public class ViewerServiceTests
    {
        [TestMethod]
        public void ExtractMetadata_ValidPdf_ReturnsMockMetadata()
        {
            // Arrange
            var reader = new PdfMetadataReader();

            // Act
            var result = reader.ExtractMetadata("buku_rekayasa_perangkat_lunak.pdf");

            // Assert
            Assert.IsTrue(result.Contains("iText7 Mock"));
            Assert.IsTrue(result.Contains("buku_rekayasa_perangkat_lunak.pdf"));
        }

        [TestMethod]
        public void ExtractMetadata_NonPdf_ThrowsInvalidOperationException()
        {
            var reader = new PdfMetadataReader();

            // Menggunakan Try-Catch-Fail untuk menghindari error CS0117
            try
            {
                reader.ExtractMetadata("gambar_sampul.png");
                Assert.Fail("Seharusnya melempar InvalidOperationException karena bukan file PDF.");
            }
            catch (InvalidOperationException)
            {
                // Sukses: Defensive programming bekerja
            }
        }

        [TestMethod]
        public void DownloadManager_PerformanceTest_ExecutesUnder50ms()
        {
            // Simulasi path direktori yang diambil dari appsettings.json
            var manager = new DownloadManager("C:/Users/Raka/Downloads/eduLib");
            var sw = Stopwatch.StartNew();

            for (int i = 0; i < 1000; i++)
            {
                manager.ProcessDownload("B" + i, "Buku_PDF_" + i);
            }

            sw.Stop();

            // Performance Testing: Simulasi unduhan massal di memori
            Assert.IsTrue(sw.ElapsedMilliseconds < 50, $"Eksekusi terlalu lambat: {sw.ElapsedMilliseconds}ms");
        }
    }
}