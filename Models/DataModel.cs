using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


public class Playlist
{
    // [BsonId]
    // [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Name { get; set; }
    public List<Song> songs { get; set; }
}

public class Song
{
    // [BsonId]
    // [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Name { get; set; }
    public string ArtistName { get; set; }
    public string Genre { get; set; }
    // public DateTimeOffset? ReleaseDate { get; set; }
    // public string? ContentAdvisoryRating { get; set; }
    // public Uri? Url { get; set; }
}