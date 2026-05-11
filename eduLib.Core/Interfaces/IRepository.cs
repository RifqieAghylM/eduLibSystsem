using System.Collections.Generic;

namespace eduLib.Core.Interfaces
{
    // Teknik: Parameterization/Generics (T adalah parameter tipe)
    public interface IRepository<T>
    {
        void Save(T item);
        List<T> GetAll();
    }
}