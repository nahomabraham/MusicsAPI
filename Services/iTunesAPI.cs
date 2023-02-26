using Music;

public class iTunesAPI
{
    static string url = "https://itunes.apple.com/search?term=";

    public static async Task<Result[]> searchMusic(string name)
    {
        var httpClient = new HttpClient();
        // var response = await httpClient.GetStringAsync(url + name);
        name = System.Web.HttpUtility.UrlEncode(name);
        var song = await httpClient.GetFromJsonAsync<AppleSong>(url + name + "&limit=10");
        Console.WriteLine(song.Results[0].ArtistName);
        return song.Results;
    }
}