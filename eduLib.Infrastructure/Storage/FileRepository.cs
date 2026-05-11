using System;
using System.Collections.Generic;
using eduLib.Core.Interfaces;

namespace eduLib.Infrastructure.Storage
{
    // Implementasi Teknik Generics
    public class FileRepository<T> : IRepository<T> where T : class
    {
        private readonly List<T> _dataStorage = new List<T>();

        // Atribut untuk Teknik Runtime Configuration
        public int MaxSizeMB { get; private set; }

        public FileRepository(int configSize)
        {
            // Defensive Programming: Design by Contract (Precondition)
            if (configSize <= 0)
                throw new ArgumentException("Ukuran maksimal file dalam konfigurasi harus lebih dari 0.");

            MaxSizeMB = configSize;
        }

        public void Save(T item)
        {
            // Defensive Programming: Precondition
            if (item == null)
                throw new ArgumentNullException(nameof(item), "Item yang diunggah tidak boleh null.");

            _dataStorage.Add(item);
        }

        public List<T> GetAll()
        {
            return _dataStorage;
        }
    }
}