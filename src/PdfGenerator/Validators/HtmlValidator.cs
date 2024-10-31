using AngleSharp;

namespace PdfGenerator.Validators;

public static class HtmlValidator
{
    public static async Task<bool> IsValidHtmlContentAsync(string content)
    {
        var config = Configuration.Default;
        var context = BrowsingContext.New(config);
        using var document = await context.OpenAsync(req => req.Content(content));

        return document.Doctype is not null;
    }
}