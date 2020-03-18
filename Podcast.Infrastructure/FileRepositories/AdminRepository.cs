using Podcast.Domain;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using Podcast.Infrastructure.Dtos;

namespace Podcast.Infrastructure.FileRepositories
{
    public class AdminRepository : BaseRepository, IAdminRepository
    {
        public AdminRepository(string connectionString) : base(connectionString)
        {

        }

        public Task<PlayList> LoadAllEpisodes()
        {
            if (!File.Exists(SaveFileName)) return Task.FromResult(new PlayList(new Episode[0]));

            using (var fileStream = File.OpenText(SaveFileName))
            {
                JsonSerializer serializer = new JsonSerializer();
                PlaylistDto playlist = (PlaylistDto)serializer.Deserialize(fileStream, typeof(PlaylistDto));

                return Task.FromResult(playlist.ToPlayList());
            }
        }

        public Task PublishEpisode(Episode episode)
        {
            PlaylistDto majedPlaylist = null;
            var serializer = new JsonSerializer();
            var dtoToSave = EpisodeDto.CreateFromEpisode(episode);
            if (new FileInfo(SaveFileName).Exists)
            {
                using (var fileStream = File.OpenText(SaveFileName))
                {
                    var playlist = (PlaylistDto)serializer.Deserialize(fileStream, typeof(PlaylistDto));
                    var episodes = playlist.Episodes.Concat(new[] { dtoToSave }).ToArray();
                    majedPlaylist = new PlaylistDto { Episodes = episodes };
                }
                File.Delete(SaveFileName);
            }
            else
            {
                majedPlaylist = new PlaylistDto { Episodes = new[] { dtoToSave } };
            }

            using (var fileStream = File.CreateText(SaveFileName))
            {
                serializer.Serialize(fileStream, majedPlaylist);
            }

            return Task.CompletedTask;
        }
    }
}
