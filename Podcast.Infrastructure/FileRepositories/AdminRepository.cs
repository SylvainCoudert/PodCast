using Newtonsoft.Json;
using Podcast.Domain;
using Podcast.Infrastructure.Dtos;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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

                if (playlist == null)
                    return Task.FromResult(new PlayList(new Episode[0]));

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
                    var playlist = (PlaylistDto)serializer.Deserialize(fileStream, typeof(PlaylistDto)) ?? new PlaylistDto();

                    var existingEpisodes = playlist.Episodes ?? new EpisodeDto[0];
                    var concatenedEpisodes = existingEpisodes.Concat(new[] { dtoToSave }).ToArray();
                    majedPlaylist = new PlaylistDto { Episodes = concatenedEpisodes };
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