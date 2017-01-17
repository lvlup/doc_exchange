using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using DocumentsExchange.DataLayer.Entity;

namespace DocumentsExchange.DataAccessLayer.Config
{
    public class DocumentExchangeDbInitializer : IDatabaseInitializer<DocumentsExchangeContext>
    {
        public void InitializeDatabase(DocumentsExchangeContext context)
        {
            try
            {
                if (context.Database.CreateIfNotExists())
                {
                    context.Database.ExecuteSqlCommand("ALTER TABLE RecordT1 ADD CONSTRAINT uc_LogIdRecordT1 UNIQUE(LogId)");
                    context.Database.ExecuteSqlCommand("ALTER TABLE RecordT2 ADD CONSTRAINT uc_LogIdRecordT2 UNIQUE(LogId)");

                    context.Set<WebSiteState>().AddOrUpdate(x => x.SiteName, new WebSiteState()
                    {
                        IsActive = true,
                        SiteName = "WebSite"
                    });

                    context.Set<Organization>().AddOrUpdate(x => x.Name, new Organization()
                    {
                        CreatedDateTime = DateTime.UtcNow,
                        IsActive = true,
                        Name = "test org"
                    });

                    context.Set<FileCategory>().AddOrUpdate(x => x.Name, new FileCategory()
                    {
                        CreatedDateTime = DateTime.UtcNow,
                        IsActive = true,
                        Name = "Платежные поручения"
                    });

                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
            }
            
        }
    }
    public class DocumentExchangeDbConfiguration : DbConfiguration
    {
        public DocumentExchangeDbConfiguration()
        {
            SetDatabaseInitializer(new DocumentExchangeDbInitializer());
        }
    }
}
