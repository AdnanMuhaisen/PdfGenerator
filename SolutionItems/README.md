# PDF Generator

A simple and efficient NuGet package to convert HTML to PDF.

## Installation

Install the package from NuGet:

Bash
dotnet add package PdfGenerator

Use code with caution.

## Usage

```csharp
C#
using HtmlToPdfConverter;

// ...

var generator = new PdfGenerator();
byte[] pdfBytes = await generator.GenerateAsync(htmlContent, dpi: 300);

// Save the PDF to a file
File.WriteAllBytes("output.pdf", pdfBytes);
```

Use code with caution.

## Customization

DPI: Customize the output PDF's DPI resolution using the dpi parameter in GenerateAsync.

## License

This package is licensed under the MIT License.

Note:

wkhtmltopdf: This is a third-party tool, and its licensing terms apply.
Error Handling: GenerateAsync throws an InvalidOperationException for invalid HTML or errors during PDF generation.
For more details and customization options, refer to the HtmlToPdfConverter class documentation.
