using Microsoft.AspNetCore.Http;

namespace MBS_COMMAND.Application.Abstractions;

public interface IMediaService
{
    Task<string> UploadImageAsync(IFormFile file);
}