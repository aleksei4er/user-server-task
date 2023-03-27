using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

string responseBody; HttpResponseMessage response; HttpContent content;
var client = GetClient();


Console.WriteLine("\nCreate User\n");
content = new StringContent(
    "<Request><user Id=\"3\" Name=\"Vladimir\"><Status>New</Status></user></Request>",
    Encoding.UTF8,
    "application/xml"
);

response = await client.PostAsync("CreateUser", content);
response.EnsureSuccessStatusCode();

responseBody = await response.Content.ReadAsStringAsync();
Console.WriteLine(responseBody);



Console.WriteLine("\nSet User Status\n");
content = new FormUrlEncodedContent(new Dictionary<string, string>
{
    { "Id", "3" },
    { "NewStatus", "New" }
});

response = await client.PostAsync("SetStatus", content);
response.EnsureSuccessStatusCode();

responseBody = await response.Content.ReadAsStringAsync();
Console.WriteLine(responseBody);



Console.WriteLine("\nRemove User\n");
content = new StringContent("{\"RemoveUser\":{\"Id\":3}}", Encoding.UTF8, "application/json");

response = await client.PostAsync("RemoveUser", content);
response.EnsureSuccessStatusCode();

responseBody = await response.Content.ReadAsStringAsync();
Console.WriteLine(responseBody);



HttpClient GetClient()
{
    var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes("test:test"));
    var authenticationHeader = new AuthenticationHeaderValue("Basic", credentials);

    var httpClientHandler = new HttpClientHandler()
    {
        UseDefaultCredentials = true
    };
    HttpClient client = new HttpClient(httpClientHandler)
    {
        BaseAddress = new Uri("https://localhost:7208/Auth/")
    };
    client.DefaultRequestHeaders.Authorization = authenticationHeader;

    return client;
}
