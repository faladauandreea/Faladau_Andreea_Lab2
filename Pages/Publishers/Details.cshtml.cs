
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Faladau_Andreea_Lab2.Models;

namespace Faladau_Andreea_Lab2.Pages.Publishers;

public class DetailsModel : PageModel
{
    private readonly Faladau_Andreea_Lab2Context _context;

    public DetailsModel(Faladau_Andreea_Lab2Context context)
    {
        _context = context;
    }

    public Publisher Publisher { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var publisher = await _context.Publisher
            .FirstOrDefaultAsync(p => p.ID == id);

        if (publisher == null)
        {
            return NotFound();
        }

        Publisher = publisher;

        return Page();
    }
}
