using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataAccessLayer.Entities
{
    [Table("Artists")]
   public class Artist
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string ArtistName { get; set; }
        public Contact Contact { get; set; }
        public Manager Manager { get; set; }
        public string Gender { get; set; }
        public ICollection<Album> Albums { get; set; } = new List<Album>();
    }


    public class Contact
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Tweeter { get; set; }
    }

    public class Manager
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Tweeter { get; set; }
    }
}
