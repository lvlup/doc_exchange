using System;
using System.Data.Entity;
using DocumentsExchange.DataAccessLayer.Config;
using DocumentsExchange.DataLayer.Entity;
using DocumentsExchange.DataLayer.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DocumentsExchange.DataAccessLayer
{
    [DbConfigurationType(typeof(DocumentExchangeDbConfiguration))]
    public class DocumentsExchangeContext :
        IdentityDbContext<User, AppRole, int, AppUserLogin, AppUserRole, AppUserClaim>
    {
        public IDbSet<Change> Changes { get; set; }

        public IDbSet<File> Files { get; set; }

        public IDbSet<FileCategory> FileCategories { get; set; }

        public IDbSet<Log> Logs { get; set; }

        public IDbSet<Message> Messages { get; set; }

        public IDbSet<Organization> Organizations { get; set; }

        public IDbSet<RecordT1> RecordT1s { get; set; }

        public IDbSet<RecordT2> RecordT2s { get; set; }


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
            modelBuilder.Properties<DateTime>().Configure(c => c.HasColumnType("datetime2"));

            modelBuilder.ComplexType<FilePath>();
            modelBuilder.ComplexType<FilePath>().Property(a => a.FileName).HasColumnName("File_Name");
            modelBuilder.ComplexType<FilePath>().Property(a => a.OriginalFileName).HasColumnName("Original_File_Name");

            modelBuilder.Entity<WebSiteState>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<User>()
                .HasMany<Organization>(u => u.Organizations)
                .WithMany(o => o.Users)
                .Map(cs =>
                {
                    cs.MapLeftKey("UserId");
                    cs.MapRightKey("OrgId");
                    cs.ToTable("UsersOrganizations");
                });

            modelBuilder.Entity<File>()
                .HasRequired(f => f.Category)
                .WithMany().HasForeignKey(f => f.CategoryId);

            modelBuilder.Entity<File>()
                .HasRequired<Organization>(f => f.Organization)
                .WithMany(o => o.Files).HasForeignKey(f => f.OranizationId);

            modelBuilder.Entity<RecordT1>()
                .HasRequired<Organization>(r => r.Organization)
                .WithMany(o => o.RecordT1s).HasForeignKey(r => r.OranizationId);

            modelBuilder.Entity<RecordT1>().Property(x => x.File.FileName).HasColumnName("FileName");
            modelBuilder.Entity<RecordT1>().Property(x => x.File.OriginalFileName).HasColumnName("OriginalFileName");

            modelBuilder.Entity<RecordT2>().Property(x => x.File.FileName).HasColumnName("FileName");
            modelBuilder.Entity<RecordT2>().Property(x => x.File.OriginalFileName).HasColumnName("OriginalFileName");

            modelBuilder.Entity<RecordT2>()
                .HasKey(x => x.Id)
                .HasRequired(r => r.Log)
                .WithMany()
                .HasForeignKey(x => x.LogId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<RecordT1>()
               .HasKey(x => x.Id)
               .HasRequired(r => r.Log)
               .WithMany()
               .HasForeignKey(x => x.LogId)
               .WillCascadeOnDelete(false);

            modelBuilder.Entity<RecordT2>()
                .HasRequired<Organization>(r => r.Organization)
                .WithMany(o => o.RecordT2s).HasForeignKey(r => r.OranizationId);
            
            modelBuilder.Entity<Message>()
                .HasRequired<User>(m => m.Sender)
                .WithMany(u => u.Messages).HasForeignKey(m => m.SenderId);

            modelBuilder.Entity<Message>()
                .HasRequired(x => x.Organization)
                .WithMany()
                .HasForeignKey(x => x.OrganizationId);

            modelBuilder.Entity<Log>()
                .HasRequired(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId);

            modelBuilder.Entity<Change>()
                .HasRequired(x => x.User)
                .WithMany();

            modelBuilder.Entity<Log>()
                .HasMany(l => l.Changes)
                .WithOptional();

            base.OnModelCreating(modelBuilder);
        }
    }
}
