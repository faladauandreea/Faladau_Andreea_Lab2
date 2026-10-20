
using Faladau_Andreea_Lab2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Faladau_Andreea_Lab2.Pages.BookPages
{
    public class CreateModel : BookCategoriesPageModel
    {
        private readonly Faladau_Andreea_Lab2Context _context;

        public CreateModel(Faladau_Andreea_Lab2Context context)
        {
            _context = context;
        }

        [BindProperty]
        public Book Book { get; set; } = new Book();

        public IActionResult OnGet()
        {
            LoadDropdowns();

            var book = new Book();
            book.BookCategories = new List<BookCategory>();

            PopulateAssignedCategoryData(_context, book);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(
            string[] selectedCategories)
        {
            var newBook = new Book();
            newBook.BookCategories = new List<BookCategory>();

            if (selectedCategories != null)
            {
                foreach (var cat in selectedCategories)
                {
                    var catToAdd = new BookCategory
                    {
                        CategoryID = int.Parse(cat)
                    };

                    newBook.BookCategories.Add(catToAdd);
                }
            }

            Book.BookCategories = newBook.BookCategories;

            if (!ModelState.IsValid)
            {
                LoadDropdowns();
                PopulateAssignedCategoryData(_context, Book);
                return Page();
            }

            _context.Book.Add(Book);
            await _context.SaveChangesAsync();

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
    }
}
