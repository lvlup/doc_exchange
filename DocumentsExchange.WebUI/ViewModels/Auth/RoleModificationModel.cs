using System.ComponentModel.DataAnnotations;

namespace DocumentsExchange.WebUI.ViewModels.Auth
{
    public class RoleModificationModel
    {
        [Required]
        public string RoleName { get; set; }
        public int[] IdsToAdd { get; set; }
        public int[] IdsToDelete { get; set; }
    }
}