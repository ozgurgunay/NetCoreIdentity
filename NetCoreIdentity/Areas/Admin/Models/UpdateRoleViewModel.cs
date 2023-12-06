using System.ComponentModel.DataAnnotations;

namespace NetCoreIdentity.Areas.Admin.Models
{
    public class UpdateRoleViewModel
    {
        public string Id { get; set; } = null!;
        [Required(ErrorMessage = "Role Name can not be left empty!")]
        [Display(Name = "Role Name :")]
        public string Name { get; set; } = null!;
    }
}
