using System.Text.Json;

namespace PostmanCloneLibrary;

public class ApiAccess : IApiAccess
{
    private readonly HttpClient client = new();

    // never return async void unless you are in an event
    public async Task<string> CallApiAsync(
        string url,
        bool formatOutput = true,
        HttpAction action = HttpAction.GET
    )
    {
        var response = await client.GetAsync(url); // call the url that gets passed in and get the response back

        if (response.IsSuccessStatusCode)
        {
            string json = await response.Content.ReadAsStringAsync(); // this will read the string content from the content, so convert it to a string

            if (formatOutput)
            {
                var jsonElement = JsonSerializer.Deserialize<JsonElement>(json); // this is a string that we are converting into a c# object called a json element
                json = JsonSerializer.Serialize(jsonElement,
                    new JsonSerializerOptions { WriteIndented = true }); // we are then taking that object and going the opposite direction and we are going back to a string
                                                                         // but write indent the value
            } // else return the raw json

            return json;
        }
        else
        {
            return $"Error: {response.StatusCode}";
        }
    }

    public bool IsValidUrl(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            return false;
        }

        // URLs are part of URIs (Http is Url), here it will try to create that uri if its true it continues with validating the uri if its https, if it 
        // doesnt create the uriOutput variable it does not continue because it is an AND (short circuits)
        bool output = Uri.TryCreate(url, UriKind.Absolute, out Uri uriOutput) &&
            (uriOutput.Scheme == Uri.UriSchemeHttps);

        return output;
    }
}
