
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Faladau_Andreea_Lab2.Models;

namespace Faladau_Andreea_Lab2.Pages.BorrowingPages;

public class EditModel : PageModel
{
    private readonly Faladau_Andreea_Lab2Context _context;

    public EditModel(Faladau_Andreea_Lab2Context context)
    {
        _context = context;
    }

    [BindProperty]
    public Borrowing Borrowing { get; set; } = default!;

    private void LoadDropdowns()
    {
        var bookList = _context.Book
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
            .ToList();

        ViewData["BookID"] = new SelectList(
            bookList, "ID", "BookFullName", Borrowing?.BookID);

        var memberList = _context.Member
            .Select(m => new
            {
                m.ID,
                FullName = m.FirstName + " " + m.LastName
            })
            .ToList();

        ViewData["MemberID"] = new SelectList(
            memberList, "ID", "FullName", Borrowing?.MemberID);
    }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var borrowing = await _context.Borrowing
            .FirstOrDefaultAsync(m => m.ID == id);

        if (borrowing is null)
        {
            return NotFound();
        }

        Borrowing = borrowing;

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

        _context.Attach(Borrowing).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!BorrowingExists(Borrowing.ID))
            {
                return NotFound();
            }

            throw;
        }

        return RedirectToPage("./Index");
    }

    private bool BorrowingExists(int id)
    {
        return _context.Borrowing.Any(e => e.ID == id);
    }
}
