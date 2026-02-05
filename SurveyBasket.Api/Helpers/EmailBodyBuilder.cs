namespace SurveyBasket.Api.Helpers;

public static class EmailBodyBuilder
{

    public static string GenerateEmailBody(string template, Dictionary<string, string> templateModel)
    {
        var pathTemplate = $"{Directory.GetCurrentDirectory()}/Template/{template}.html";

        var streamReader = new StreamReader(pathTemplate);
        var body = streamReader.ReadToEnd();
        streamReader.Close();

        foreach (var item in templateModel)
        {
            body = body.Replace(item.Key, item.Value);
        }
        return body;
    }
}
