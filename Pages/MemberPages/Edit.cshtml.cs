
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Faladau_Andreea_Lab2.Models;
using System.Text.RegularExpressions;

namespace Faladau_Andreea_Lab2.Pages.MemberPages;

public class EditModel : PageModel
{
    private readonly Faladau_Andreea_Lab2Context _context;

    public EditModel(Faladau_Andreea_Lab2Context context)
    {
        _context = context;
    }

    [BindProperty]
    public Member Member { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var member = await _context.Member
            .FirstOrDefaultAsync(m => m.ID == id);

        if (member is null)
        {
            return NotFound();
        }

        Member = member;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        ModelState.Remove("Member.FullName");
        ModelState.Remove("Member.Borrowings");

        if (!string.IsNullOrWhiteSpace(Member.Phone))
        {
            string phonePattern =
                @"^\(?([0-9]{4})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{3})$";

            if (!Regex.IsMatch(Member.Phone, phonePattern))
            {
                ModelState.AddModelError(
                    "Member.Phone",
                    "Telefonul trebuie sa fie de forma '0722-123-123' sau '0722.123.123' sau '0722 123 123'");
            }
        }

        if (!ModelState.IsValid)
        {
            return Page();
        }

        var memberToUpdate = await _context.Member
            .FirstOrDefaultAsync(m => m.ID == Member.ID);

        if (memberToUpdate is null)
        {
            return NotFound();
        }

        memberToUpdate.FirstName = Member.FirstName;
        memberToUpdate.LastName = Member.LastName;
        memberToUpdate.Adress = Member.Adress;
        memberToUpdate.Phone = Member.Phone;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!MemberExists(Member.ID))
            {
                return NotFound();
            }

            throw;
        }

        return RedirectToPage("./Index");
    }

    private bool MemberExists(int id)
    {
        return _context.Member.Any(e => e.ID == id);
    }
}
