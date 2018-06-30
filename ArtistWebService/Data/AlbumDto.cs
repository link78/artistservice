using System;

namespace ArtistWebService.Data
{
    public class AlbumDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Genre { get; set; }
        public decimal Price { get; set; }
        public DateTime ReleasedDate { get; set; }
        public string Producer { get; set; }
    }
}