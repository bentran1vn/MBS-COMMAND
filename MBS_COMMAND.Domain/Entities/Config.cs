using System.ComponentModel.DataAnnotations;

namespace MBS_COMMAND.Domain.Entities;

public class Config
{
    [Key]
    public string Key { get; set; }
    public string Value { get; set; }
}
