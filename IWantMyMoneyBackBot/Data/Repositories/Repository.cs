using System;
using Microsoft.EntityFrameworkCore;

namespace IWantMyMoneyBackBot.Data.Repositories
{
    public abstract class Repository<T> where T : class
    {
        protected DbSet<T> _set;

        protected readonly Action DataChanged;

        protected Repository(Context ctx) => DataChanged += () => ctx.SaveChanges();
    }
}