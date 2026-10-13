
using Faladau_Andreea_Lab2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Faladau_Andreea_Lab2.Pages.Authors;

public class CreateModel : PageModel
{
    private readonly Faladau_Andreea_Lab2Context _context;

    public CreateModel(Faladau_Andreea_Lab2Context context)
    {
        _context = context;
    }

    [BindProperty]
    public Faladau_Andreea_Lab2.Models.Authors Author
    { get; set; } = new();

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        _context.Authors.Add(Author);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}
