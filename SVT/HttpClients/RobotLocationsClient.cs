namespace SVT.HttpsClients;

using SVT.Models;
public class RobotsClient 
{
    private readonly HttpClient _httpClient;

    public RobotsClient(HttpClient httpClient)
    {
        _httpClient = httpClient;

        _httpClient.BaseAddress = new Uri("https://60c8ed887dafc90017ffbd56.mockapi.io/");
    }

    public async Task<IEnumerable<Robot>> GetRobotLocations() {
        return await _httpClient.GetFromJsonAsync<IEnumerable<Robot>>("robots") ?? new List<Robot>();
    }
}