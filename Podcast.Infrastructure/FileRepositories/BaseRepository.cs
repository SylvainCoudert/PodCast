using System;

namespace Podcast.Infrastructure.FileRepositories
{
    public abstract class BaseRepository
    {
        protected string SaveFileName;

        protected BaseRepository(string saveFileName)
        {
            SaveFileName = saveFileName ?? throw new ArgumentNullException(nameof(saveFileName));
        }

    }
}
