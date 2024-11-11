using System.ComponentModel.DataAnnotations;

namespace MBS_COMMAND.Domain.Entities;

public class Config
{
    [Key]
    public string Key { get; set; }
    [MaxLength(Int32.MaxValue)]
    public string Value { get; set; }
}
