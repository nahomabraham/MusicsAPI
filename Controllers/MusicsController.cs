using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

[ApiController]
[Route("[controller]")]
public class MusicsController : ControllerBase
{
    static IMongoCollection<MusicData> SongList;
    // static List<MusicData> SongList = new List<MusicData>{
    //      new Music Data("1", "Nobody", "Casting Crowns", "Christian, Rock"),
    //      new MusicData("2", "Be Alright", "Evan Craft", "Christian, Rap/Hip-Hop"),
    //      new MusicData("3", "Less Like Me", "Jack Williams", "Christian, Soft Rock"),
    // };
    public MusicsController(IOptions<MusicDatabaseSettings> musicDatabaseSettings)
    {
        var mongoClient = new MongoClient(musicDatabaseSettings.Value.ConnectionString);
        var database = mongoClient.GetDatabase(musicDatabaseSettings.Value.DatabaseName);
        SongList = database.GetCollection<MusicData>(musicDatabaseSettings.Value.CollectionName);
    }


    [HttpGet]
    public List<MusicData> GetAllSongs()
    {
        return SongList.Find(_ => true).ToList();
    }
    [HttpGet("id/{id}")]
    public MusicData GetSongById(string id)
    {
        return SongList.Find(song => song.Id == id).FirstOrDefault();
    }

    [HttpPost]
    public void AddSong(MusicData Song)
    {
        SongList.InsertOne(Song);
    }
    [HttpPut]
    public void EditSongById(string Id, MusicData EdittedSong)
    {
        var songs = SongList.Find(_ => true).ToList();

        if (songs != null)
        {
            var song = songs.Find(song => song.Id == Id);
            songs.Insert(songs.IndexOf(song!), EdittedSong);
            songs.Remove(song!);
            SongList = (IMongoCollection<MusicData>)songs;
        }
    }
    [HttpDelete]
    public void DeleteSongById(string Id)
    {
        var songs = SongList.Find(_ => true).ToList();

        if (songs != null)
        {
            var song = songs.Find(song => song.Id == Id);
            songs.Remove(song!);
            SongList = (IMongoCollection<MusicData>)songs;
        }
    }
}