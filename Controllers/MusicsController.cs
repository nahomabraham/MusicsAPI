using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

[ApiController]
[Route("[controller]")]
public class MusicsController : ControllerBase
{
    static IMongoCollection<Playlist> PlaylistCollection;
    // static List<Playlist> PlaylistCollection = new List<Playlist>{
    //      new Music Data("1", "Nobody", "Casting Crowns", "Christian, Rock"),
    //      new Playlist("2", "Be Alright", "Evan Craft", "Christian, Rap/Hip-Hop"),
    //      new Playlist("3", "Less Like Me", "Jack Williams", "Christian, Soft Rock"),
    // };
    public MusicsController(IOptions<MusicDatabaseSettings> musicDatabaseSettings)
    {
        var mongoClient = new MongoClient(musicDatabaseSettings.Value.ConnectionString);
        var database = mongoClient.GetDatabase(musicDatabaseSettings.Value.DatabaseName);
        PlaylistCollection = database.GetCollection<Playlist>(musicDatabaseSettings.Value.CollectionName);
    }


    [HttpGet]
    public List<Playlist> GetAllPlaylists()
    {
        return PlaylistCollection.Find(_ => true).ToList();
    }
    [HttpGet("id/{id}")]
    public Playlist GetPlaylistById(string id)
    {
        return PlaylistCollection.Find(song => song.Id == id).FirstOrDefault();
    }

    [HttpPost]
    public void AddPlaylist(Playlist playlist)
    {
        PlaylistCollection.InsertOne(playlist);
    }

    [HttpPost("name/{PlaylistName}")]
    public void AddSongToPlaylist(string PlaylistName, Song song)
    {
        var playlist = PlaylistCollection.FindOneAndDelete(playlist => playlist.Name == PlaylistName);
        playlist.songs.Add(song);
        PlaylistCollection.InsertOne(playlist);
    }

    // [HttpPut]
    // public void EditSongById(string Id, Playlist EdittedSong)
    // {
    //     var songs = PlaylistCollection.Find(_ => true).ToList();

    //     if (songs != null)
    //     {
    //         var song = songs.Find(song => song.Id == Id);
    //         songs.Insert(songs.IndexOf(song!), EdittedSong);
    //         songs.Remove(song!);
    //         PlaylistCollection = (IMongoCollection<Playlist>)songs;
    //     }
    // }
    [HttpDelete("id")]
    public void DeletePlaylistById(string Id)
    {
        // var playlists = PlaylistCollection.Find(_ => true).ToList();

        // if (playlists != null)
        // {
        //     var song = playlists.Find(song => song.Id == Id);
        //     playlists.Remove(song!);
        //     PlaylistCollection = (IMongoCollection<Playlist>)playlists;
        // }
        // var playlist = PlaylistCollection.FindOneAndDelete(playlist => playlist.Id == Id);
        // PlaylistCollection.InsertOne(playlist);
        var playlist = new Playlist();
        PlaylistCollection.FindOneAndDelete(playlist => playlist.Id == Id);
    }
    [HttpDelete("id/songId")]
    public void DeleteSongFromPlaylistById(string playlistId, string songId)
    {
        var playlist = PlaylistCollection.Find(playlist => playlist.Id == playlistId).FirstOrDefault();
        foreach (var song in playlist.songs)
        {
            if (song.Id == songId)
            {
                playlist.songs.Remove(song);
                PlaylistCollection.ReplaceOne(playlist => playlist.Id == playlistId, playlist);
                break;
            }
        }
        // if (songs != null)
        // {
        //     var song = songs.Find(song => song.Id == Id);
        //     songs.Remove(song!);
        //     PlaylistCollection = (IMongoCollection<Playlist>)songs;
        // }
    }
}