using Rest.Models;

namespace Rest.Repository
{
    public interface IRepository<T>
    {
        void Add(T dm);
        List<T> GetAll();
        T GetById(string id);
        void RemoveById(string id);
        void Modify(string id, T dm);
    }
}
