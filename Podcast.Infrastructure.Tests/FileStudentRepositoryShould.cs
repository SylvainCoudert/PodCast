using NUnit.Framework;
using Podcast.Domain;
using Podcast.Infrastructure.FileRepositories;
using System;
using System.Linq;
using System.IO;

namespace Podcast.Infrastructure.Tests
{
    public class FileStudentRepositoryShould
    {
        private IStudentRepository studentRepository;
        private IAdminRepository adminRepository;
        private string _saveFileName = "Test.save";

        [SetUp]
        public void Setup()
        {
            studentRepository = new StudentRepository(_saveFileName);
            adminRepository = new AdminRepository(_saveFileName);
        }

        [TearDown]
        public void TearDown()
        {
            if (File.Exists(_saveFileName)) File.Delete(_saveFileName);
        }

        [Test]
        public void RetrieveOnePublishedEpisode()
        {
            var episode = new Episode(new EpisodeName("Test01"), new EpisodeTitle("Episode 1"), new PublicationDate(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 1)));

            adminRepository.PublishEpisode(episode);

            var actualPlaylist = studentRepository.LoadPlaylist().GetAwaiter().GetResult();

            Assert.IsTrue(actualPlaylist.Episodes.Contains(episode));
        }

        [Test]
        public void RetrieveTodayPublishedEpisode()
        {
            var episode = new Episode(new EpisodeName("Test01"), new EpisodeTitle("Episode 1"), new PublicationDate(DateTime.Now));

            adminRepository.PublishEpisode(episode);

            var actualPlaylist = studentRepository.LoadPlaylist().GetAwaiter().GetResult();

            Assert.IsTrue(actualPlaylist.Episodes.Contains(episode));
        }

        [Test]
        public void RetrieveOrderedEpisode()
        {
            var episode1 = new Episode(new EpisodeName("Test01"), new EpisodeTitle("Episode 1"), new PublicationDate(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 10)));
            var episode2 = new Episode(new EpisodeName("Test02"), new EpisodeTitle("Episode 2"), new PublicationDate(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 1)));

            adminRepository.PublishEpisode(episode1);
            adminRepository.PublishEpisode(episode2);

            var actualPlaylist = studentRepository.LoadPlaylist().GetAwaiter().GetResult();

            Assert.IsTrue(actualPlaylist.Episodes.Contains(episode1));
            Assert.IsTrue(actualPlaylist.Episodes.Contains(episode2));
            Assert.IsTrue(actualPlaylist.Episodes.ElementAt(0) == episode2);
            Assert.IsTrue(actualPlaylist.Episodes.ElementAt(1) == episode1);

        }

        [Test]
        public void NotRetrievePendingEpisode()
        {
            var episode = new Episode(new EpisodeName("Test01"), new EpisodeTitle("Episode 1"), new PublicationDate(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day + 10)));

            adminRepository.PublishEpisode(episode);

            var actualPlaylist = studentRepository.LoadPlaylist().GetAwaiter().GetResult();

            Assert.IsFalse(actualPlaylist.Episodes.Any());
        }

        [Test]
        public void RetrieveEmptyPlaylistWithNoFile()
        {
            var actualPlaylist = studentRepository.LoadPlaylist().GetAwaiter().GetResult();

            Assert.IsFalse(actualPlaylist.Episodes.Any());
        }

    }
}