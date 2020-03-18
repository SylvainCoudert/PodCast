using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Podcast.Domain;

namespace Podcast.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IStudentRepository _studentRepository;
        public PlayList Playlist { get; set; }

        public IndexModel(ILogger<IndexModel> logger, IStudentRepository studentRepository)
        {
            _logger = logger;
            _studentRepository = studentRepository;
        }

        public void OnGet()
        {
            Playlist = _studentRepository.LoadPlaylist().GetAwaiter().GetResult();
        }
    }
}
