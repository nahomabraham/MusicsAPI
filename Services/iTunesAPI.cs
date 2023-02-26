using Music;

public class iTunesAPI
{
    static string url = "https://itunes.apple.com/search?term=";

    public static async Task<Result[]> searchMusic(string name)
    {
        var httpClient = new HttpClient();

        name = System.Web.HttpUtility.UrlEncode(name);
        var song = await httpClient.GetFromJsonAsync<AppleSong>(url + name + "&limit=10");

        return song.Results;
    }
}