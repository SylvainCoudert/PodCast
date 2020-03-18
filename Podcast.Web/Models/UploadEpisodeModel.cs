using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Podcast.Models
{
    public class UploadEpisodeModel
    {
        public string TitreEpisode { get; set; }
        public string NomEpisode { get; set; }
        public DateTime DatePublication { get; set; }
    }
}
