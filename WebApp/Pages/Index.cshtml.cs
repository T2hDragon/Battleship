using System.Threading.Tasks;
using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace WebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<IndexModel> _logger;


        public IndexModel(ILogger<IndexModel> logger, ApplicationDbContext context)
        {
            _context = context;
            _logger = logger;
        }

        [BindProperty] public int PlayerCount { get; set; }

        [BindProperty] public bool UsePresetRules { get; set; }


        public async Task<IActionResult> OnPostAsync()
        {
            return RedirectToPage($"./GameCreation/{(UsePresetRules ? "Preset" : "Custom")}Rules",
                new {playerCount = PlayerCount});
        }
    }
}