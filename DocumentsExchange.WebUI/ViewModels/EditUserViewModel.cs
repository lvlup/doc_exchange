using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DocumentsExchange.BusinessLayer.Models.User;

namespace DocumentsExchange.WebUI.ViewModels
{
    public class CreateUserViewModel : EditUserViewModel
    {
        [Required]
        [DisplayName("Пароль")]
        public string Password { get; set; }

        [Compare(nameof(Password))]
        [DisplayName("Потверждение")]
        public string ConfirmPassword { get; set; }
    }

    public class PasswordResetViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Password { get; set; }

        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }

    public class EditUserViewModel
    {
        public int Id { get; set; }

        [Required]
        [DisplayName("Имя")]
        public string FirstName { get; set; }

        [Required]
        [DisplayName("Фамилия")]
        public string LastName { get; set; }

        [Required]
        [DisplayName("Логин")]
        public string UserName { get; set; }

        [DisplayName("Активность")]
        public bool IsActive { get; set; }

        [Required]
        [DisplayName("Роль")]
        public string Role { get; set; }

        [DisplayName("Организации")]
        public int[] Organizations { get; set; }

        public IEnumerable<OrganizationInfo> OrganizationInfoes { get; set; }

        public IEnumerable<string> Roles
            =>
                new []
                {
                    BusinessLayer.Identity.Roles.User,
                    BusinessLayer.Identity.Roles.Admin,
                    BusinessLayer.Identity.Roles.Observer,
                    BusinessLayer.Identity.Roles.Technician
                };
    }
}