using NUnit.Framework;
using Podcast.Domain;
using Podcast.Infrastructure.FileRepositories;
using System;
using System.Linq;
using System.IO;

namespace Podcast.Infrastructure.Tests
{
    public class FileAdminRepositoryShould
    {
        private IAdminRepository adminRepository;
        private string _saveFileName = "Test.save";

        [SetUp]
        public void Setup()
        {
            adminRepository = new AdminRepository(_saveFileName);
        }

        [TearDown]
        public void TearDown()
        {
            if (File.Exists(_saveFileName)) File.Delete(_saveFileName);
        }

        [Test]
        public void RetrieveEmptyPlaylistWithNoFile()
        {
            var actualPlaylist = adminRepository.LoadAllEpisodes().GetAwaiter().GetResult();

            Assert.IsFalse(actualPlaylist.Episodes.Any());
        }

        [Test]
        public void NotThrowExceptionWhenTryingToSaveEpisodeOnEmptyFile()
        {
            var episode = new Episode(new EpisodeName("Test01"), new EpisodeTitle("Episode 1"), new PublicationDate(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 1)));

            Assert.DoesNotThrow(() => adminRepository.PublishEpisode(episode));
            Assert.IsTrue(File.Exists(_saveFileName));
            Assert.Greater(new FileInfo(_saveFileName).Length, 0);
        }

        [Test]
        public void NotThrowExceptionWhenTryingToSaveEpisodeOnNonEmptyFile()
        {
            var episode1 = new Episode(new EpisodeName("Test01"), new EpisodeTitle("Episode 1"), new PublicationDate(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 1)));
            var episode2 = new Episode(new EpisodeName("Test02"), new EpisodeTitle("Episode 1"), new PublicationDate(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day + 10)));

            Assert.DoesNotThrow(() => adminRepository.PublishEpisode(episode1));
            Assert.DoesNotThrow(() => adminRepository.PublishEpisode(episode2));
            Assert.IsTrue(File.Exists(_saveFileName));
            Assert.Greater(new FileInfo(_saveFileName).Length, 0);
        }

        [Test]
        public void SaveAndLoadEpisode()
        {
            var episode1 = new Episode(new EpisodeName("Test01"), new EpisodeTitle("Episode 1"), new PublicationDate(new DateTime(2020, 1, 1)));
            var episode2 = new Episode(new EpisodeName("Test02"), new EpisodeTitle("Episode 1"), new PublicationDate(new DateTime(2020, 1, 1)));

            adminRepository.PublishEpisode(episode1);
            adminRepository.PublishEpisode(episode2);

            var playlist = adminRepository.LoadAllEpisodes().GetAwaiter().GetResult();
            Assert.IsTrue(playlist.Episodes.Contains(episode1));
            Assert.IsTrue(playlist.Episodes.Contains(episode2));
        }
    }
}