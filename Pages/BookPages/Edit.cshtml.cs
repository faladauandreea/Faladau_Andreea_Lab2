
using Faladau_Andreea_Lab2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Faladau_Andreea_Lab2.Pages.BookPages;

public class EditModel : PageModel
{
    private readonly Faladau_Andreea_Lab2Context _context;

    public EditModel(Faladau_Andreea_Lab2Context context)
    {
        _context = context;
    }

    [BindProperty]
    public Book Book { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var book = await _context.Book
            .FirstOrDefaultAsync(m => m.ID == id);

        if (book == null)
        {
            return NotFound();
        }

        Book = book;

        LoadDropdowns();

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            LoadDropdowns();
            return Page();
        }

        var bookToUpdate = await _context.Book
            .FirstOrDefaultAsync(b => b.ID == Book.ID);

        if (bookToUpdate == null)
        {
            return NotFound();
        }

        bookToUpdate.Title = Book.Title;
        bookToUpdate.Price = Book.Price;
        bookToUpdate.PublishingDate = Book.PublishingDate;
        bookToUpdate.PublisherID = Book.PublisherID;
        bookToUpdate.AuthorsID = Book.AuthorsID;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!BookExists(Book.ID))
            {
                return NotFound();
            }

            throw;
        }

        return RedirectToPage("./Index");
    }

    private void LoadDropdowns()
    {
        ViewData["PublisherID"] = new SelectList(
            _context.Publisher,
            "ID",
            "PublisherName",
            Book.PublisherID
        );

        ViewData["AuthorsID"] = new SelectList(
            _context.Authors
                .AsEnumerable()
                .Select(a => new
                {
                    a.ID,
                    FullName = a.FirstName + " " + a.LastName
                }),
            "ID",
            "FullName",
            Book.AuthorsID
        );
    }

    private bool BookExists(int id)
    {
        return _context.Book.Any(e => e.ID == id);
    }
}
