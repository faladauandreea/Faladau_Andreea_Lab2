
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Faladau_Andreea_Lab2.Models;

namespace Faladau_Andreea_Lab2.Pages.BookPages;

public class DetailsModel : PageModel
{
    private readonly Faladau_Andreea_Lab2Context _context;

    public DetailsModel(Faladau_Andreea_Lab2Context context)
    {
        _context = context;
    }

    public Book Book { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var book = await _context.Book
            .Include(b => b.Authors)
            .Include(b => b.Publisher)
            .FirstOrDefaultAsync(m => m.ID == id);

        if (book == null)
        {
            return NotFound();
        }

        Book = book;

        return Page();
    }
}

