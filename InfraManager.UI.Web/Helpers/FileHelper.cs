using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.IO;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Helpers;

public static class FileHelper
{
    public static async Task<byte[]> ProcessStreamedFile(
        MultipartSection section,
        long sizeLimit)
    {
        try
        {
            using (var memoryStream = new MemoryStream())
            {
                await section.Body.CopyToAsync(memoryStream);

                // Check if the file is empty or exceeds the size limit.
                if (memoryStream.Length == 0)
                {
                    throw new FileNotFoundException("File is empty.");
                }
                else if (memoryStream.Length > sizeLimit)
                {
                    var megabyteSizeLimit = sizeLimit / 1048576;
                    throw new FileNotFoundException($"The file exceeds {megabyteSizeLimit:N1} MB.");
                }
                else
                {
                    return memoryStream.ToArray();
                }
            }
        }
        catch (Exception ex)
        {
            throw new FileNotFoundException($"The upload failed. Please contact the Help Desk for support. Error: {ex.HResult}");
        }

        return Array.Empty<byte>();
    }
    internal static bool IsFromForm(this ApiParameterDescription
    apiParameter)
    {
        var source = apiParameter.Source;
        var elementType = apiParameter.ModelMetadata?.ElementType;

        return (source == BindingSource.Form || source ==
        BindingSource.FormFile) || (elementType != null &&
        typeof(IFormFile).IsAssignableFrom(elementType));
    }
}
