using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using eduLib.Core.Entities;
using eduLib.Core.Enums;
using eduLib.Application.Auth;

namespace eduLib.Tests.AuthTests
{
    [TestClass]
    public class AuthServiceTests
    {
        private List<User> _mockDb;
        private AuthService _authService;

        [TestInitialize]
        public void Setup()
        {
            _mockDb = new List<User>
            {
                new User { Username = "azka_admin", Password = "123", UserRole = Role.Admin },
                new User { Username = "rifqie_pelajar", Password = "abc", UserRole = Role.Pelajar }
            };
            _authService = new AuthService(_mockDb);
        }

        [TestMethod]
        public void Login_ValidCredentials_ReturnsUserAndSetsStateToLoggedIn()
        {
            // Act
            var user = _authService.Login("azka_admin", "123");

            // Assert
            Assert.IsNotNull(user);
            Assert.AreEqual(SessionState.LoggedIn, _authService.GetCurrentState());
        }

        // PERBAIKAN 1: Menggunakan pola Try-Catch-Fail
        [TestMethod]
        public void Login_InvalidCredentials_ThrowsUnauthorizedAccess()
        {
            try
            {
                // Act
                _authService.Login("azka_admin", "salah_password");

                // Jika baris di atas lolos (tidak error), paksa tes ini gagal
                Assert.Fail("Seharusnya melempar UnauthorizedAccessException, tapi malah lolos.");
            }
            catch (UnauthorizedAccessException)
            {
                // Assert sukses! Exception yang diharapkan benar-benar dilempar.
            }
        }

        // PERBAIKAN 2: Menggunakan Assert.ThrowsException
        [TestMethod]
        public void Login_Failed3Times_LocksAccount()
        {
            // Arrange
            try { _authService.Login("azka_admin", "salah1"); } catch { }
            try { _authService.Login("azka_admin", "salah2"); } catch { }
            try { _authService.Login("azka_admin", "salah3"); } catch { }

            try
            {
                // Act
                _authService.Login("azka_admin", "123");

                Assert.Fail("Seharusnya melempar InvalidOperationException karena state akun terkunci.");
            }
            catch (InvalidOperationException)
            {
                // Assert sukses! Automata berhasil mengunci akun.
            }
        }

        [TestMethod]
        public void GetUserMenus_LoggedInAdmin_ReturnsAdminMenus()
        {
            // Arrange
            var user = _authService.Login("azka_admin", "123");

            // Act
            var menus = _authService.GetUserMenus(user);

            // Assert
            Assert.IsTrue(menus.Contains("Manajemen Akun"));
            Assert.IsFalse(menus.Contains("Ulasan/Komentar"));
        }

        // PERBAIKAN 3: Menggunakan Assert.ThrowsException
        [TestMethod]
        public void Login_EmptyUsername_TriggersDefensiveProgramming()
        {
            try
            {
                // Act
                _authService.Login("", "123");

                Assert.Fail("Seharusnya melempar ArgumentException karena username kosong.");
            }
            catch (ArgumentException)
            {
                // Assert sukses! Defensive programming (DbC) bekerja dengan baik.
            }
        }
    }
}