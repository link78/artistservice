using System;

namespace ArtistWebService.Data
{
   public class ArtistDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string ArtistName { get; set; }
       
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public string ContactTweeter { get; set; }
        public string ManagerFullName { get; set; }
        public string ManagerEmail { get; set; }
        public string ManagerPhone { get; set; }
        public string ManagerTweeter { get; set; }
        public string Gender { get; set; }
    }
}