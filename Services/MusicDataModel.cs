namespace Music
{

    public partial class AppleSong
    {
        public long ResultCount { get; set; }
        public Result[] Results { get; set; }
    }

    public partial class Result
    {
        public string TrackName { get; set; }
        public string ArtistName { get; set; }
        public string CollectionName { get; set; }
        public string CollectionCensoredName { get; set; }
        public string TrackCensoredName { get; set; }
        // public Uri ArtistViewUrl { get; set; }
        // public Uri CollectionViewUrl { get; set; }
        // public Uri TrackViewUrl { get; set; }
        public Uri PreviewUrl { get; set; }
        public Uri ArtworkUrl30 { get; set; }
        public Uri ArtworkUrl60 { get; set; }
        public Uri ArtworkUrl100 { get; set; }
        // public DateTimeOffset ReleaseDate { get; set; }
        // public Explicitness CollectionExplicitness { get; set; }
        // public Explicitness TrackExplicitness { get; set; }
        public long TrackCount { get; set; }
        public long TrackNumber { get; set; }
        public long TrackTimeMillis { get; set; }
        public PrimaryGenreName PrimaryGenreName { get; set; }
        public long? CollectionArtistId { get; set; }
        // public CollectionArtistName? CollectionArtistName { get; set; }
        public string ContentAdvisoryRating { get; set; }
    }

    // public enum CollectionArtistName { HillsongUnited, VariousArtists };

    // public enum Explicitness { Explicit, NotExplicit };

    // public enum Country { Usa };

    // public enum Currency { Usd };

    // public enum Kind { MusicVideo, Song };

    public enum PrimaryGenreName { Ambient, Ccm, Christian, Dance, HipHopRap, House };

    // public enum WrapperType { Track };
}
