using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Music;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicsAPI.Models;
using MusicsAPI.Services;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using ZstdSharp.Unsafe;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;


[ApiController]
[Route("api/[controller]")]
public class PlaylistsController : ControllerBase
{
    public static User user = new User();
    private readonly IMongoCollection<User> _users;
    private readonly IConfiguration _configuration;

    static IMongoCollection<Playlist>? PlaylistCollection;
    public PlaylistsController(IOptions<MusicDatabaseSettings> musicDatabaseSettings, IOptions<UserDatabaseSettings> settings, IConfiguration configuration)
    {
        var mongoClient = new MongoClient(musicDatabaseSettings.Value.ConnectionString);
        var database = mongoClient.GetDatabase(musicDatabaseSettings.Value.DatabaseName);
        PlaylistCollection = database.GetCollection<Playlist>(musicDatabaseSettings.Value.CollectionName);
   
        _users = database.GetCollection<User>(settings.Value.UserCollectionName);
        _configuration = configuration;
    }

    [HttpPost("Register")]
    public async Task<ActionResult<User>> Register(UserDto request)
    {

        user.UserName = request.UserName;
        user.UserPassword = request.Password;        
        _users.InsertOne(user);
        return Ok(user);


    }


    [HttpPost("Login")]
    public async Task<ActionResult<string>> Login(UserDto request)

    {
        var name = _users.Find(u => u.UserName == request.UserName).FirstOrDefault();
        if (name == null)
        {
            return BadRequest("User not found");
        }

        User user1 = _users.Find(u => u.UserName == request.UserName && u.UserPassword == request.Password).FirstOrDefault();
        if (user1 == null)
        {
            return BadRequest("Wrong password");
        }
        string token = CreateToken(user1);
        return Ok(token);

    }


    [HttpPut("UpdateAccount"), Authorize]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult<User>> UpdateAccount(string name, string password, string newusername, string newpassword)
    {

        User newuser = _users.Find(u => u.UserName == name && u.UserPassword == password).FirstOrDefault();
        if (newuser == null)
        {
            return BadRequest("wrong username or password");
        }
        newuser.UserName = newusername;
        newuser.UserPassword = newpassword;
        return Ok(_users.ReplaceOne(u => u.UserName == name, newuser));

    }


    [HttpDelete("delete")]
    public async Task<ActionResult<User>> DeleteAccount(string name, string password)
    {
        return Ok(_users.DeleteOne(u => u.UserName == name && u.UserPassword == password));

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




    private string CreateToken(User user)
    {
        List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName)
            };
        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("authentication:Token").Value));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(

            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: creds
            );

        var Jwt = new JwtSecurityTokenHandler().WriteToken(token);
        return Jwt;


    }

}