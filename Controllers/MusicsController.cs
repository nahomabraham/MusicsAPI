using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class MusicsController : ControllerBase
{
    static List<DataModel> SongList = new List<DataModel>{
         new DataModel("1", "Nobody", "Casting Crowns", "Christian, Rock"),
         new DataModel("2", "Be Alright", "Evan Craft", "Christian, Rap/Hip-Hop"),
         new DataModel("3", "Less Like Me", "Jack Williams", "Christian, Soft Rock"),
    };

    [HttpGet]
    public List<DataModel> GetAllSongs()
    {
        Console.WriteLine(SongList.Count);
        return SongList;
    }

    [HttpPost]
    public void AddSong(DataModel Song)
    {
        SongList.Add(Song);
        Console.WriteLine(SongList.Count);
    }
}