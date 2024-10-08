using System.ComponentModel.DataAnnotations;

namespace MBS_COMMAND.Infrastucture.DependencyInjection.Options;

public record CloudinaryOptions
{
    [Required]public string CloudName { get; set; }
    [Required]public string ApiKey { get; set; }
    [Required]public string ApiSecret { get; set; }
}