using PdfGenerator.Core.Interfaces;
using PdfGenerator.Validators;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace PdfGenerator.Infrastructure;

public sealed class PdfService : IPdfService
{
    /// <summary>
    /// Generates a PDF document from the given HTML content.
    /// </summary>
    /// <param name="htmlContent">The HTML content to convert to PDF.</param>
    /// <param name="dpi">The DPI resolution of the generated PDF. Default is 180.</param>
    /// <param name="cancellationToken">A token to observe cancellation requests.</param>
    /// <returns>A byte array representing the generated PDF document.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the provided HTML content is invalid.</exception>
    public async Task<byte[]> GenerateAsync(string htmlContent, int dpi = 180, CancellationToken cancellationToken = default)
    {
        if (!await HtmlValidator.IsValidHtmlContentAsync(htmlContent))
        {
            throw new InvalidOperationException("Invalid Content!");
        }

        string workingDirectory = Directory.GetParent(Assembly.GetExecutingAssembly().Location)!.FullName;
        string wkhtmltopdfExecutableFilePath = Path.Combine(workingDirectory, "Core\\Executables\\wkhtmltopdf.exe");
        StringBuilder argumentsBuilder = new("-q -n " + "" + " - -");

        argumentsBuilder.Insert(0, $"--dpi {dpi} ");

        ProcessStartInfo processStartInfo = new()
        {
            CreateNoWindow = true,
            UseShellExecute = false,
            RedirectStandardError = true,
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            Arguments = argumentsBuilder.ToString(),
            FileName = wkhtmltopdfExecutableFilePath
        };

        using var process = Process.Start(processStartInfo)!;
        using var pdfStream = new MemoryStream();
        using var utf8Writer = new StreamWriter(process.StandardInput.BaseStream, Encoding.UTF8);
        await utf8Writer.WriteAsync(htmlContent);
        await utf8Writer.FlushAsync(cancellationToken);
        utf8Writer.Close();
        await process.StandardOutput.BaseStream.CopyToAsync(pdfStream, cancellationToken);
        var errors = await process.StandardError.ReadToEndAsync(cancellationToken);

        if (!string.IsNullOrEmpty(errors))
        {
            Debug.WriteLine(errors);

            return [];
        }

        return pdfStream.ToArray();
    }
}