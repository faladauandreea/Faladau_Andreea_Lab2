
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Faladau_Andreea_Lab2.Models;

namespace Faladau_Andreea_Lab2.Pages.Authors;

public class IndexModel : PageModel
{
    private readonly Faladau_Andreea_Lab2Context _context;

    public IndexModel(Faladau_Andreea_Lab2Context context)
    {
        _context = context;
    }

    public IList<Faladau_Andreea_Lab2.Models.Authors> AuthorsList
    { get; set; } = new List<Faladau_Andreea_Lab2.Models.Authors>();

    public async Task OnGetAsync()
    {
        AuthorsList = await _context.Authors.ToListAsync();
    }
}
