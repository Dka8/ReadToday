using ReadToday.Model;

namespace ReadToday.UI.DataProvider
{
    public interface IBookRepository : IGenericRepository<CBook>
    {
        void RemoveCharacter(CCharacter model);
    }
}
