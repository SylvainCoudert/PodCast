using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Podcast.Domain;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Podcast.Web.Pages
{
    public class AdminModel : PageModel
    {
        public PlayList Playlist { get; set; }

        [BindProperty(Name = "title")]
        public string TitreEpisode { get; set; }

        [BindProperty(Name = "datePubli")]
        public DateTime DatePublication { get; set; }

        [BindProperty]
        public IFormFile Upload { get; set; }

        private IHostingEnvironment _environment;
        private IAdminRepository _adminRepository;

        public AdminModel(IHostingEnvironment environment, IAdminRepository adminRepository)
        {
            _environment = environment;
            _adminRepository = adminRepository;
        }

        public void OnGet()
        {
            RetrievePlaylist();
        }

        public async Task OnPostAsync()
        {
            var audioDir = Path.Combine(_environment.WebRootPath, "Content\\Audio");
            var file = Path.Combine(audioDir, Upload.FileName);

            if (!Directory.Exists(audioDir))
                Directory.CreateDirectory(audioDir);

            using (var fileStream = new FileStream(file, FileMode.Create))
            {
                await Upload.CopyToAsync(fileStream);
                await _adminRepository.PublishEpisode(new Episode(new EpisodeName(Upload.FileName.Replace(".mp3", "")), new EpisodeTitle(TitreEpisode), new PublicationDate(DatePublication)));
            }
            RetrievePlaylist();
        }

        private void RetrievePlaylist()
        {
            var getParam = Request.Query["pass"];
            if (getParam != Constantes.MotDePasseAdminEnseignant) throw new UnauthorizedAccessException("Bien essayé !");
            Playlist = _adminRepository.LoadAllEpisodes().GetAwaiter().GetResult();
        }
    }
}