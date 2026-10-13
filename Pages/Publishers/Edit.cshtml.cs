
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Faladau_Andreea_Lab2.Models;

namespace Faladau_Andreea_Lab2.Pages.Publishers;

public class EditModel : PageModel
{
    private readonly Faladau_Andreea_Lab2Context _context;

    public EditModel(Faladau_Andreea_Lab2Context context)
    {
        _context = context;
    }

    [BindProperty]
    public Publisher Publisher { get; set; } = new Publisher();

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

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var publisher = await _context.Publisher
            .FirstOrDefaultAsync(p => p.ID == Publisher.ID);

        if (publisher == null)
        {
            return NotFound();
        }

        publisher.PublisherName = Publisher.PublisherName;

        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}
