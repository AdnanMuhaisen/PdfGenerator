﻿using FluentAssertions;
using PdfGenerator.Infrastructure;
using System.Reflection;

namespace PdfGenerator.UnitTests.Tests;

public class PdfServiceTests
{
    [Theory]
    [InlineData("SimpleTemplate.html", 300)]
    [InlineData("ComplexTemplate.html", 300)]
    public async Task GenerateAsync_GenerateSimplePdfDocument_ShouldReturnPdfDocument(string fileName, int dpi)
    {
        // Arrange
        var workingDirectory = Directory.GetParent(Assembly.GetExecutingAssembly().Location)!
            .Parent!
            .Parent!
            .Parent!
            .FullName;

        var pdfService = new PdfService();
        var templatePath = Path.Combine(workingDirectory, $"Templates\\HtmlTemplates\\{fileName}");
        var fileContent = await File.ReadAllTextAsync(templatePath);

        //Act
        var pdfFileBytes = await pdfService.GenerateAsync(fileContent, dpi);

        // Assert
        pdfFileBytes
            .Should()
            .HaveCountGreaterThan(0);
    }

    [Fact]
    public async Task GenerateAsync_InvalidHtmlContent_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var content = "Invalid Html Content!!!";
        var pdfService = new PdfService();

        //Act

        // Assert
        await pdfService
            .Invoking(x => x.GenerateAsync(content))
            .Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage("Invalid Content!");
    }
}