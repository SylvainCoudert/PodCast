using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Podcast.Domain
{
    public interface IAdminRepository
    {
        Task PublishEpisode(Episode episode);
        Task<PlayList> LoadAllEpisodes();
    }
}
