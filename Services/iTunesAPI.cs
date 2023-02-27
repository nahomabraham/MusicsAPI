using Music;

public class iTunesAPI
{
    static string url = "https://itunes.apple.com/search?term=";

    public static async Task<Result[]> searchMusic(string searchText)
    {
        searchText = System.Web.HttpUtility.UrlEncode(searchText);

        var httpClient = new HttpClient();
        var song = await httpClient.GetFromJsonAsync<AppleSong>(url + searchText + "&limit=10");

        return song.Results;
    }
}