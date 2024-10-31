namespace PdfGenerator.Core.Interfaces;

public interface IPdfService
{
    Task<byte[]> GenerateAsync(string htmlContent, int dpi, CancellationToken cancellationToken = default);
}