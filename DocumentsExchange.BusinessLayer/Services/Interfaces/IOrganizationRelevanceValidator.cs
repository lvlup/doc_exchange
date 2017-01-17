namespace DocumentsExchange.BusinessLayer.Services.Interfaces
{
    public interface IOrganizationRelevanceValidator
    {
        bool Check(int userId, int organizationId);
    }

    class OrganizationRelevanceValidator : IOrganizationRelevanceValidator
    {
        public bool Check(int userId, int organizationId)
        {
            return true;
        }
    }
}
