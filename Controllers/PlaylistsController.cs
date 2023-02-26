using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Music;

[ApiController]
[Route("api/[controller]")]
public class PlaylistsController : ControllerBase
{
    static IMongoCollection<Playlist>? PlaylistCollection;
    public PlaylistsController(IOptions<MusicDatabaseSettings> musicDatabaseSettings)
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
        return PlaylistCollection.Find(playlist => playlist.Id == id).FirstOrDefault(); ;
    }

    [HttpGet("search/{searchString}")]
    public async Task<Result[]> search(string searchString)
    {
        return await iTunesAPI.searchMusic(searchString);
    }
    [HttpPost]
    public void CreatePlaylist(Playlist playlist)
    {
        PlaylistCollection?.InsertOne(playlist);
    }

    [HttpPost("{PlaylistId}")]
    public void AddSongToPlaylist(string PlaylistId, Result song)
    {
        var playlist = GetPlaylistById(PlaylistId);

        playlist?.songs.Add(song);
        if (playlist != null)
        {
            DeletePlaylistById(PlaylistId);
            PlaylistCollection?.InsertOne(playlist);
        }
    }

    [HttpPut("editPlaylist/{PlaylistId}")]
    public void EditPlaylistById(string PlaylistId, string NewPlaylistName)
    {
        var playlist = GetPlaylistById(PlaylistId);
        if (playlist != null)
            playlist.Name = NewPlaylistName;
        PlaylistCollection.ReplaceOne(playlist => playlist.Id == PlaylistId, playlist);
    }
    [HttpDelete("deleteplaylist/{Id}")]
    public void DeletePlaylistById(string Id)
    {
        PlaylistCollection.DeleteOne(i => i.Id == Id);
    }
    [HttpDelete("deletesong/{SongId}")]
    public void DeleteSongFromPlaylistById(string PlaylistId, string SongId)
    {
        var playlist = GetPlaylistById(PlaylistId);
        var song = playlist.songs.Find(song => song.TrackId.ToString() == SongId);
        if (song != null)
            playlist.songs.Remove(song);
        PlaylistCollection.ReplaceOne(playlist => playlist.Id == PlaylistId, playlist);
    }
}