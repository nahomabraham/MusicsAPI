using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Music;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using authentication.Model;

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


    [HttpDelete("DeleteAccount"), Authorize]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult<User>> DeleteAccount(string name, string password)
    {
        return Ok(_users.DeleteOne(u => u.UserName == name && u.UserPassword == password));

    }



    [HttpGet, Authorize]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public List<Playlist> GetAllPlaylists()
    {
        return PlaylistCollection.Find(_ => true).ToList();
    }
    [HttpGet("GetPlaylist/{id}"), Authorize]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public Playlist GetPlaylistById(string id)
    {
        return PlaylistCollection.Find(playlist => playlist.Id == id).FirstOrDefault(); ;
    }

    [HttpGet("SearchSong/{searchString}"), Authorize]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<Result[]> search(string searchString)
    {
        return await iTunesAPI.searchMusic(searchString);
    }
    [HttpPost("CreatePlaylist"), Authorize]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public void CreatePlaylist(Playlist playlist)
    {
        PlaylistCollection?.InsertOne(playlist);
    }

    [HttpPost("AddSong/{PlaylistId}"), Authorize]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<Result[]> AddSongToPlaylist(string PlaylistId, string searchText)
    {
        var playlist = GetPlaylistById(PlaylistId);
        var song = await iTunesAPI.searchMusic(searchText);

        if (song.Count() == 0)
            return Array.Empty<Result>();
        if (playlist != null)
        {
            playlist?.songs.Add(song[0]);
            DeletePlaylistById(PlaylistId);
            PlaylistCollection?.InsertOne(playlist);
        }
        return song;
    }

    [HttpPut("EditPlaylist/{PlaylistId}"), Authorize]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public void EditPlaylistById(string PlaylistId, string NewPlaylistName)
    {
        var playlist = GetPlaylistById(PlaylistId);
        if (playlist != null)
            playlist.Name = NewPlaylistName;
        PlaylistCollection.ReplaceOne(playlist => playlist.Id == PlaylistId, playlist);
    }
    [HttpDelete("DeletePlaylist/{Id}"), Authorize]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public void DeletePlaylistById(string Id)
    {
        PlaylistCollection.DeleteOne(i => i.Id == Id);
    }
    [HttpDelete("DeleteSong/{SongId}"), Authorize]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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