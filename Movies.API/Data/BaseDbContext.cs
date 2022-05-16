using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Movies.API.Models;

namespace Movies.API.Data
{
    public abstract class BaseDbContext<T> : DbContext where T : DbContext
    {
        public BaseDbContext(DbContextOptions<T> options) : base(options)
        {
            this.ChangeTracker.StateChanged += ChangeTracker_StateChanged;
        }

        private void ChangeTracker_StateChanged(object sender, EntityStateChangedEventArgs e)
        {
            if (e.NewState == EntityState.Added || e.NewState == EntityState.Modified)
            {
                var entity = (BaseEntity)e.Entry.Entity;
                entity.UpdatedDate = DateTime.UtcNow;
            }
        }
    }
}
