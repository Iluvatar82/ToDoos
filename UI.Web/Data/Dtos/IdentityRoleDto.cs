using Core.Validation;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace UI.Web.Data.Dtos
{
    public class IdentityRoleDto
    {
        [Required(ErrorMessageResourceName = "RequiredMessage", ErrorMessageResourceType = typeof(ValidationResources))]
        [MinLength(3, ErrorMessageResourceName = "MinLengthMessage", ErrorMessageResourceType = typeof(ValidationResources))]
        public string Id { get; set; }

        [Required(ErrorMessageResourceName = "RequiredMessage", ErrorMessageResourceType = typeof(ValidationResources))]
        [MinLength(5, ErrorMessageResourceName = "MinLengthMessage", ErrorMessageResourceType = typeof(ValidationResources))]
        public string Rolle { get; set; }

        public IdentityRoleDto()
        {
            Id = string.Empty;
            Rolle = string.Empty;
        }

        public IdentityRoleDto(string? id, string? name)
        {
            Id = id ?? string.Empty;
            Rolle = name ?? string.Empty;
        }

        public static IdentityRole ToIdentityRole(IdentityRoleDto dto) => new() { Id = dto.Id, Name = dto.Rolle, NormalizedName = dto.Rolle };
        public static IdentityRoleDto ToIdentityRoleDto(IdentityRole entity) => new() { Id = entity.Id, Rolle = entity.Name ?? string.Empty };
    }
}
