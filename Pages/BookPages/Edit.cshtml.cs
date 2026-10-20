
using Faladau_Andreea_Lab2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Faladau_Andreea_Lab2.Pages.BookPages
{
    public class EditModel : BookCategoriesPageModel
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

            // Includem Publisher, Authors si categoriile cartii
            var book = await _context.Book
                .Include(b => b.Publisher)
                .Include(b => b.Authors)
                .Include(b => b.BookCategories)
                    .ThenInclude(bc => bc.Category)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);

            if (book == null)
            {
                return NotFound();
            }

            Book = book;

            // Pregatim categoriile pentru checkbox-uri
            PopulateAssignedCategoryData(_context, Book);

            LoadDropdowns();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(
            int? id, string[] selectedCategories)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Incarcam cartea impreuna cu categoriile existente
            var bookToUpdate = await _context.Book
                .Include(b => b.Publisher)
                .Include(b => b.BookCategories)
                    .ThenInclude(bc => bc.Category)
                .FirstOrDefaultAsync(b => b.ID == id);

            if (bookToUpdate == null)
            {
                return NotFound();
            }

            // Actualizam datele cartii
            if (await TryUpdateModelAsync<Book>(
                bookToUpdate,
                "Book",
                b => b.Title,
                b => b.Price,
                b => b.PublishingDate,
                b => b.PublisherID,
                b => b.AuthorsID))
            {
                // Actualizam categoriile bifate
                UpdateBookCategories(
                    _context, selectedCategories, bookToUpdate);

                await _context.SaveChangesAsync();

                return RedirectToPage("./Index");
            }

            // Daca validarea esueaza, reincarcam categoriile
            UpdateBookCategories(
                _context, selectedCategories, bookToUpdate);

            Book = bookToUpdate;

            PopulateAssignedCategoryData(_context, Book);

            LoadDropdowns();

            return Page();
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
    }
}
