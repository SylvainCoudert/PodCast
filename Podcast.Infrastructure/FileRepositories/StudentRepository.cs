using Newtonsoft.Json;
using Podcast.Domain;
using Podcast.Infrastructure.Dtos;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Podcast.Infrastructure.FileRepositories
{
    public class StudentRepository : BaseRepository, IStudentRepository
    {
        public StudentRepository(string connectionString) : base(connectionString)
        {
        }

        public Task<PlayList> LoadPlaylist()
        {
            if (!File.Exists(SaveFileName)) return Task.FromResult(new PlayList(new Episode[0]));

            using (var fileStream = File.OpenText(SaveFileName))
            {
                JsonSerializer serializer = new JsonSerializer();
                var playlist = (PlaylistDto)serializer.Deserialize(fileStream, typeof(PlaylistDto));

                var filterPlayList = new PlaylistDto { Episodes = playlist.Episodes.Where(e => e.DatePublication <= DateTime.Today.AddDays(1).AddMinutes(-1)).ToArray() };
                return Task.FromResult(filterPlayList.ToPlayList());
            }
        }
    }
}