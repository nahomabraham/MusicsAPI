


public class DataModel
{
    public DataModel(string Id, string Name, string ArtistName, string Genre)
    {
        this.Id = Id;
        this.Name = Name;
        this.ArtistName = ArtistName;
        this.Genre = Genre;

    }
    public string Id { get; set; }
    public string Name { get; set; }
    public string ArtistName { get; set; }
    public string Genre { get; set; }
    // public DateTimeOffset? ReleaseDate { get; set; }
    // public string? ContentAdvisoryRating { get; set; }
    // public Uri? Url { get; set; }
}

