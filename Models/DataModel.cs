
public class Playlist
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public List<Song> songs { get; set; } = null!;
}

public class Song
{
    public string? Id { get; set; } = null!;
    public string? Name { get; set; } = null!;
    public string? ArtistName { get; set; } = null!;
    public string? Genre { get; set; } = null!;
    // public DateTimeOffset? ReleaseDate { get; set; }
    // public string? ContentAdvisoryRating { get; set; }
    // public Uri? Url { get; set; }
}