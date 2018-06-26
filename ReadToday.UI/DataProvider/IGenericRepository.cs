using System;

namespace ReadToday.UI.DataProvider
{
    public interface IGenericRepository<T>
    {
        T GetById(Guid id);

        void Add(T model);

        void Save();

        void Delete(T model);

        Boolean HasChanges();
    }
}
