using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Entities
{
    //[Table("Albums")]
    public class Album
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Genre { get; set; }
        public decimal Price { get; set; }
        public DateTime ReleasedDate { get; set; }
        public string Producer { get; set; }

        public Artist Artist { get; set; }
    }
}