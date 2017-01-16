using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentsExchange.DataLayer.Entity;

namespace DocumentsExchange.DataAccessLayer
{
   public class DocumentsExchangeContext: DbContext
    {
        public IDbSet<Change> Changes { get; set; }

        public IDbSet<File> Files { get; set; }

        public IDbSet<FilePath> FilePaths { get; set; }

        public IDbSet<FileCategory> FileCategories { get; set; }

        public IDbSet<Log> Logs { get; set; }

        public IDbSet<Message> Messages { get; set; }

        public IDbSet<Organization> Organizations { get; set; }

        public IDbSet<RecordT1> RecordT1s { get; set; }

        public IDbSet<RecordT2> RecordT2s { get; set; }

        public IDbSet<User> Users { get; set; }


        public DocumentsExchangeContext() : base("DefaultConnection")
        {
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.AutoDetectChangesEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
        }

        public DocumentsExchangeContext(string cstring) : base(cstring)
        {
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.AutoDetectChangesEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
        }


       protected override void OnModelCreating(DbModelBuilder modelBuilder)
       {
           modelBuilder.Entity<File>()
               .HasRequired(f => f.Category)
               .WithMany().HasForeignKey(f => f.CategoryId);

            modelBuilder.Entity<File>()
             .HasRequired<Organization>(f => f.Organization)
             .WithMany(o => o.Files).HasForeignKey(f => f.OranizationId);

            modelBuilder.Entity<RecordT1>()
               .HasRequired<Organization>(r => r.Organization)
               .WithMany(o => o.RecordT1s).HasForeignKey(r=>r.OranizationId);

            //todo
           modelBuilder.Entity<RecordT1>()
               .HasRequired<FilePath>(r => r.File)
               .WithOptional();

           modelBuilder.Entity<RecordT1>()
               .HasMany(r => r.Logs)
               .WithOptional();

            modelBuilder.Entity<RecordT2>()
             .HasRequired<FilePath>(r => r.File)
             .WithOptional();

            modelBuilder.Entity<RecordT2>()
               .HasRequired<Organization>(r => r.Organization)
               .WithMany(o=> o.RecordT2s).HasForeignKey(r=>r.OranizationId);

           modelBuilder.Entity<RecordT2>()
               .HasMany(r => r.Logs)
               .WithOptional();

          
            modelBuilder.Entity<Message>()
               .HasRequired<User>(m => m.Sender)
               .WithMany(u => u.Messages).HasForeignKey(m=>m.SenderId);

           modelBuilder.Entity<Log>()
               .HasMany(l => l.Changes)
               .WithOptional();

            base.OnModelCreating(modelBuilder);
       }
    }
}
