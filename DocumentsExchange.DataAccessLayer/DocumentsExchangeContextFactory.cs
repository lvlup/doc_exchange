using System.Data.Entity.Infrastructure;

namespace DocumentsExchange.DataAccessLayer
{
    public class DocumentsExchangeContextFactory : IDbContextFactory<DocumentsExchangeContext>
    {
        public DocumentsExchangeContext Create()
        {
            return new DocumentsExchangeContext();
        }

        public DocumentsExchangeContext Create(string connectionString)
        {
            return new DocumentsExchangeContext(connectionString);
        }
    }
}
