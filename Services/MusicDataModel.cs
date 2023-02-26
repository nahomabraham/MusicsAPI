namespace Music
{

    public partial class AppleSong
    {
        public long ResultCount { get; set; }
        public Result[] Results { get; set; }
    }

    public partial class Result
    {
        // public WrapperType WrapperType { get; set; }
        // public Kind Kind { get; set; }
        // public long? ArtistId { get; set; }
        // public long? CollectionId { get; set; }
        public long TrackId { get; set; }
        public string TrackName { get; set; } = null!;
        public string ArtistName { get; set; } = null!;
        public string[] Genres { get; set; } = null!;
        // public string CollectionName { get; set; }
        // public string CollectionCensoredName { get; set; }
        // public string TrackCensoredName { get; set; }
        // public Uri ArtistViewUrl { get; set; }
        // public Uri CollectionViewUrl { get; set; }
        // public Uri TrackViewUrl { get; set; }
        public Uri PreviewUrl { get; set; } = null!;
        public Uri ArtworkUrl30 { get; set; } = null!;
        public Uri ArtworkUrl60 { get; set; } = null!;
        public Uri ArtworkUrl100 { get; set; } = null!;
        // public double CollectionPrice { get; set; }
        // public double TrackPrice { get; set; }
        // public DateTimeOffset ReleaseDate { get; set; }
        // public Explicitness CollectionExplicitness { get; set; }
        // public Explicitness TrackExplicitness { get; set; }
        // public long? DiscCount { get; set; }
        // public long? DiscNumber { get; set; }
        // public long? TrackCount { get; set; }
        // public long? TrackNumber { get; set; }
        // public long TrackTimeMillis { get; set; }
        // public Country Country { get; set; }
        // public Currency Currency { get; set; }
        // public PrimaryGenreName PrimaryGenreName { get; set; }
        // public bool? IsStreamable { get; set; }
        // public double? TrackRentalPrice { get; set; }
        // public double? CollectionHdPrice { get; set; }
        // public double? TrackHdPrice { get; set; }
        // public double? TrackHdRentalPrice { get; set; }
        // public string ContentAdvisoryRating { get; set; }
        // public string ShortDescription { get; set; }
        // public string LongDescription { get; set; }
        // public bool? HasITunesExtras { get; set; }
        // public long? CollectionArtistId { get; set; }
        // public string CollectionArtistName { get; set; }
        // public Uri FeedUrl { get; set; }
        // public Uri ArtworkUrl600 { get; set; }
        // public long[] GenreIds { get; set; }
    }

    // public enum Explicitness { Cleaned, NotExplicit };

    // public enum Country { Usa };

    // public enum Currency { Usd };

    // public enum Kind { FeatureMovie, Podcast, Song };

    // public enum PrimaryGenreName { Ccm, Christian, Christianity, Documentary, Reggae, ReligionSpirituality, SingerSongwriter };

    // public enum WrapperType { Track };
}
