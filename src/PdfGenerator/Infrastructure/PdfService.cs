using PdfGenerator.Core.Interfaces;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace PdfGenerator.Infrastructure;

public sealed class PdfService : IPdfService
{
    public async Task<byte[]> GenerateAsync(string htmlContent, int dpi = 180, CancellationToken cancellationToken = default)
    {
        string workingDirectory = Directory.GetParent(Assembly.GetExecutingAssembly().Location)!.FullName;
        string wkhtmltopdfExecutableFilePath = Path.Combine(workingDirectory, "Core\\Executables\\wkhtmltopdf.exe");
        StringBuilder argumentsBuilder = new("-q -n ");

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