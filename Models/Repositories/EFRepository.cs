using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;


namespace NgCookingMVC_BackEND.Models.Repositories
{


    public class EFRepository<T> : IREpository<T> where T : class
    {
        protected DbContext Dbcontext { get; set; }
        protected DbSet<T> DbSet { get; set; }

        public EFRepository(DbContext dbcontext)
        {


            if (dbcontext == null)
                throw new ArgumentNullException("dbcontext");
            Dbcontext = dbcontext;

            DbSet = Dbcontext.Set<T>();

        }
        public virtual ObservableCollection<T> LoadAll()
        {
            DbSet.Load();
            return DbSet.Local;
        }
        /* public EFRepository(DbContext context)
         {
             // TODO: Complete member initialization
             this.context = context;
         }*/

        public virtual IQueryable<T> GetAll()
        {
            return DbSet;
        }



        public virtual T GetById(int id)
        {
            return DbSet.Find(id);
        }
        public virtual T GetById(string id)
        {
            return DbSet.Find(id);
        }

        public virtual void Add(T entity)
        {
            DbEntityEntry dbEntityEntry = Dbcontext.Entry(entity);
            if (dbEntityEntry.State != EntityState.Detached)
            {
                dbEntityEntry.State = EntityState.Added;
            }
            else
            {
                DbSet.Add(entity);
            }
        }
        public virtual void AddRange(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                Add(entity);
            }
        }


        public virtual void Update(T entity)
        {
            DbEntityEntry dbEntityEntry = Dbcontext.Entry(entity);
            if (dbEntityEntry.State == EntityState.Deleted)
            {
                DbSet.Attach(entity);
            }

            dbEntityEntry.State = EntityState.Modified;
        }

        public virtual void Delete(T entity)
        {
            DbEntityEntry dbEntityEntry = Dbcontext.Entry(entity);
            if (dbEntityEntry.State != EntityState.Deleted)
            {
                dbEntityEntry.State = EntityState.Deleted;
            }
            else
            {

                DbSet.Attach(entity);
                DbSet.Remove(entity);

            }

        }


        public virtual void Delete(int id)
        {
            var entity = GetById(id);
            if (entity == null) return;
            Delete(entity);

        }
        public virtual void Save()
        {
            Dbcontext.SaveChanges();

        }
    }
}