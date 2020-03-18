using System;
using System.Collections.Generic;
using Value;

namespace Podcast.Domain
{
    public class PlayList
    {
        public IEnumerable<Episode> Episodes { get; }

        public PlayList(IEnumerable<Episode> episodes)
        {
            Episodes = episodes ?? throw new ArgumentNullException(nameof(episodes));
        }
    }

    public class Episode : ValueType<Episode>
    {
        public EpisodeName NomEpisode { get; }
        public EpisodeTitle TitreEpisode { get; }
        public PublicationDate DatePublication { get; }

        public Episode(EpisodeName nomEpisode, EpisodeTitle titreEpisode, PublicationDate datePublication)
        {
            NomEpisode = nomEpisode ?? throw new ArgumentNullException(nameof(nomEpisode));
            TitreEpisode = titreEpisode ?? throw new ArgumentNullException(nameof(titreEpisode));
            DatePublication = datePublication ?? throw new ArgumentNullException(nameof(datePublication));
        }

        protected override IEnumerable<object> GetAllAttributesToBeUsedForEquality()
            => new object[] { NomEpisode, TitreEpisode };
    }

    public class EpisodeName : ValueType<EpisodeName>
    {
        private readonly string _value;

        public EpisodeName(string value)
        {
            _value = value;
        }

        public static implicit operator string(EpisodeName vo) => vo._value;


        public override string ToString() => _value;

        protected override IEnumerable<object> GetAllAttributesToBeUsedForEquality() => new[] { _value };
    }

    public class EpisodeTitle : ValueType<EpisodeTitle>
    {
        private readonly string _value;

        public EpisodeTitle(string value)
        {
            _value = value;
        }

        public static implicit operator string(EpisodeTitle vo) => vo._value;


        public override string ToString() => _value;

        protected override IEnumerable<object> GetAllAttributesToBeUsedForEquality() => new[] { _value };
    }

    public class PublicationDate : ValueType<PublicationDate>
    {
        private readonly DateTime _value;

        public PublicationDate(DateTime value)
        {
            _value = value;
        }

        public static implicit operator DateTime(PublicationDate vo) => vo._value;


        public override string ToString() => _value.ToString("yyyy/MM/dd");

        protected override IEnumerable<object> GetAllAttributesToBeUsedForEquality() => new object[] { _value };
    }
}
