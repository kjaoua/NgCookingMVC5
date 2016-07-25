using System.Linq;


namespace NgCookingMVC_BackEND.Models.Repositories
{
    public interface IREpository<T> where T : class
    {
        IQueryable<T> GetAll();
        //T GetbyID(int id);
        // ObservableCollection<T> LoadAll();
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        //void Delete(int id);
        void Save();

    }
}
