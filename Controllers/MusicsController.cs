using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

[ApiController]
[Route("api/[controller]")]
public class MusicsController : ControllerBase
{
    static IMongoCollection<Playlist>? PlaylistCollection;
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
    [HttpGet("{id}")]
    public Playlist GetPlaylistById(string id)
    {
        return PlaylistCollection.Find(song => song.Id == id).FirstOrDefault();
    }

    [HttpPost]
    public void CreatePlaylist(Playlist playlist)
    {
        PlaylistCollection?.InsertOne(playlist);
    }

    [HttpPost("{PlaylistName}")]
    public void AddSongToPlaylist(string PlaylistName, Song song)
    {
        var playlist = PlaylistCollection?.FindOneAndDelete(playlist => playlist.Name == PlaylistName);
        playlist?.songs.Add(song);
        if (playlist != null)
            PlaylistCollection?.InsertOne(playlist);
    }

    [HttpPut("{PlaylistId}")]
    public void EditPlaylistNameById(string PlaylistId, string NewPlaylistName)
    {
        var playlist = PlaylistCollection?.Find(playlist => playlist.Id == PlaylistId).FirstOrDefault();
        if (playlist != null)
            playlist.Name = NewPlaylistName;
        PlaylistCollection.ReplaceOne(playlist => playlist.Id == PlaylistId, playlist);
    }
    [HttpPut("{SongId}")]
    public void EditSongById(string PlaylistId, string SongId, Song EdittedSong)
    {
        var playlist = PlaylistCollection.Find(playlist => playlist.Id == PlaylistId).FirstOrDefault();
        var song = playlist.songs.Find(song => song.Id == SongId);
        playlist.songs.Remove(song);
        playlist.songs.Add(EdittedSong);
        PlaylistCollection.ReplaceOne(playlist => playlist.Id == PlaylistId, playlist);
    }
    [HttpDelete("deleteplaylist/{id}")]
    public void DeletePlaylistById(string Id)
    {
        var playlist = PlaylistCollection.Find(playlist => playlist.Id == Id).FirstOrDefault();
        PlaylistCollection.ReplaceOne(playlist => playlist.Id == Id, new Playlist());
    }
    [HttpDelete("deletesong/{SongId}")]
    public void DeleteSongFromPlaylistById(string playlistId, string SongId)
    {
        var playlist = PlaylistCollection.Find(playlist => playlist.Id == playlistId).FirstOrDefault();
        var song = playlist.songs.Find(song => song.Id == SongId);
        if (song != null)
            playlist.songs.Remove(song);
        PlaylistCollection.ReplaceOne(playlist => playlist.Id == playlistId, playlist);
    }
}