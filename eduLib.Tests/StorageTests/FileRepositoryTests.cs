using Microsoft.VisualStudio.TestTools.UnitTesting;
using eduLib.Infrastructure.Storage;
using System;
using System.Diagnostics;

namespace eduLib.Tests.StorageTests
{
    [TestClass]
    public class FileRepositoryTests
    {
        [TestMethod]
        public void Save_ValidData_ShouldStoredInList()
        {
            // Simulasi Runtime Config (Teknik Konstruksi Rifki)
            var repo = new FileRepository<string>(50);

            repo.Save("Buku_Algoritma.pdf");

            Assert.AreEqual(1, repo.GetAll().Count);
        }

        [TestMethod]
        public void Save_NullItem_ShouldThrowException()
        {
            var repo = new FileRepository<string>(50);

            // Perbaikan CS0117: Menggunakan Try-Catch-Fail
            try
            {
                repo.Save(null);
                Assert.Fail("Seharusnya melempar ArgumentNullException");
            }
            catch (ArgumentNullException)
            {
                // Sukses: Exception tertangkap (Defensive Programming)
            }
        }

        [TestMethod]
        public void Storage_PerformanceTest_BulkSave()
        {
            // Perbaikan CS0452: Menggunakan string agar sesuai batasan 'where T : class'
            var repo = new FileRepository<string>(100);
            var stopwatch = Stopwatch.StartNew();

            for (int i = 0; i < 1000; i++)
            {
                repo.Save("DummyData" + i);
            }

            stopwatch.Stop();

            // Performance Testing: Simpan 1000 data harus di bawah 50ms [cite: 403, 419]
            Assert.IsTrue(stopwatch.ElapsedMilliseconds < 50,
                $"Eksekusi terlalu lambat: {stopwatch.ElapsedMilliseconds}ms");
        }
    }
}