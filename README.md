# WhuPdfGenerator

WhuPdfGenerator is a .NET library designed for generating PDF files from HTML content. It converts an HTML string into a PDF file represented as a byte array, which can be easily saved, sent, or displayed.

## Features

- Simple and fast HTML-to-PDF conversion
- Outputs PDF as a byte array for flexible usage
- Ideal for dynamic content generation such as invoices, reports, and more

## Installation

Install WhuPdfGenerator via NuGet Package Manager:

```bash
dotnet add package WhuPdfGenerator
```

Or install directly from the NuGet Package Manager UI in Visual Studio.

## Usage

To use WhuPdfGenerator, include the library in your project and pass your HTML string to the generator to produce a PDF as a byte array.

#### Example

```csharp
using PdfGenerator.Infrastructure;

// Sample HTML content
string htmlContent = """
    <html>
        <body>
            <h1>Hello, World!</h1>
            <p>This is a sample PDF document generated from HTML.</p>
        </body>
    </html>
    """;

// Generate PDF as byte array
var pdfService = new PdfService();
var pdfBytes = await pdfService.GenerateAsync(htmlContent, 300);

// Save the PDF to a file
string filePath = "output.pdf";
System.IO.File.WriteAllBytes(filePath, pdfBytes);

Console.WriteLine($"PDF generated successfully and saved to {filePath}");
```

### Method

```csharp
Task<byte[]> GenerateAsync(string htmlContent, int dpi = 180, CancellationToken cancellationToken = default)
```

Accepts an HTML string as input and returns a PDF document as a byte array.

## License

This project is licensed under the MIT License.
