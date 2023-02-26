using Music;

public class Playlist
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public List<Result> songs { get; set; } = null!;
}