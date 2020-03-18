using Podcast.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Podcast.Infrastructure.Dtos
{
    public class PlaylistDto
    {
        public EpisodeDto[] Episodes { get; set; }

        public PlayList ToPlayList() => new PlayList(Episodes.OrderByDescending(e => e.DatePublication).Select(e => e.ToEpisode()));

        public static PlaylistDto CreateFromPlayList(PlayList playlist) => new PlaylistDto { Episodes = playlist.Episodes.Select(e => EpisodeDto.CreateFromEpisode(e)).ToArray() };
    }
}
