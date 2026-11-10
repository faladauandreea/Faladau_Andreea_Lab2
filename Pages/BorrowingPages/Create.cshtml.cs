
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Faladau_Andreea_Lab2.Models;

namespace Faladau_Andreea_Lab2.Pages.BorrowingPages;

public class CreateModel : PageModel
{
    private readonly Faladau_Andreea_Lab2Context _context;

    public CreateModel(Faladau_Andreea_Lab2Context context)
    {
        _context = context;
    }

    [BindProperty]
    public Borrowing Borrowing { get; set; } = default!;

    private async Task LoadDropdownsAsync()
    {
        var bookList = await _context.Book
            .Include(b => b.Authors)
            .Select(b => new
            {
                b.ID,
                BookFullName = b.Title + " - " +
                    (b.Authors != null
                        ? b.Authors.LastName + " " +
                          b.Authors.FirstName
                        : "")
            })
            .ToListAsync();

        ViewData["BookID"] = new SelectList(
            bookList, "ID", "BookFullName", Borrowing?.BookID);

        var memberList = await _context.Member
            .Select(m => new
            {
                m.ID,
                FullName = m.FirstName + " " + m.LastName
            })
            .ToListAsync();

        ViewData["MemberID"] = new SelectList(
            memberList, "ID", "FullName", Borrowing?.MemberID);
    }

    public async Task<IActionResult> OnGetAsync()
    {
        await LoadDropdownsAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        ModelState.Remove("Borrowing.Member");
        ModelState.Remove("Borrowing.Book");

        if (!await _context.Member.AnyAsync(m => m.ID == Borrowing.MemberID))
        {
            ModelState.AddModelError(
                "Borrowing.MemberID", "Select a valid member.");
        }

        if (!await _context.Book.AnyAsync(b => b.ID == Borrowing.BookID))
        {
            ModelState.AddModelError(
                "Borrowing.BookID", "Select a valid book.");
        }

        if (!ModelState.IsValid)
        {
            await LoadDropdownsAsync();
            return Page();
        }

        var borrowing = new Borrowing
        {
            MemberID = Borrowing.MemberID,
            BookID = Borrowing.BookID,
            ReturnDate = Borrowing.ReturnDate
        };

        _context.Borrowing.Add(borrowing);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}
