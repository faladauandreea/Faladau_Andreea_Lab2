
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Faladau_Andreea_Lab2.Models;

namespace Faladau_Andreea_Lab2.Pages.Publishers;

public class CreateModel : PageModel
{
    private readonly Faladau_Andreea_Lab2Context _context;

    public CreateModel(Faladau_Andreea_Lab2Context context)
    {
        _context = context;
    }

    [BindProperty]
    public Publisher Publisher { get; set; } = new Publisher();

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        _context.Publisher.Add(Publisher);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}
