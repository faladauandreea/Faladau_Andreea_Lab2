
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Faladau_Andreea_Lab2.Models;

namespace Faladau_Andreea_Lab2.Pages.BookPages
{
    public class IndexModel : PageModel
    {
        private readonly Faladau_Andreea_Lab2Context _context;

        public IndexModel(Faladau_Andreea_Lab2Context context)
        {
            _context = context;
        }

        public IList<Book> Book { get; set; } = new List<Book>();

        public BookData BookD { get; set; } = new BookData();

        public int BookID { get; set; }

        public int CategoryID { get; set; }

        public async Task OnGetAsync(int? id, int? categoryID)
        {
            BookD = new BookData();

            // Includem autorii, editurile si categoriile
            BookD.Books = await _context.Book
                .Include(b => b.Publisher)
                .Include(b => b.Authors)
                .Include(b => b.BookCategories)
                    .ThenInclude(bc => bc.Category)
                .AsNoTracking()
                .OrderBy(b => b.Title)
                .ToListAsync();

            // Pastram lista pentru codul existent din Index.cshtml
            Book = BookD.Books.ToList();

            if (id != null)
            {
                BookID = id.Value;

                Book? book = BookD.Books
                    .SingleOrDefault(b => b.ID == id.Value);

                if (book != null)
                {
                    BookD.Categories = book.BookCategories!
                        .Where(bc => bc.Category != null)
                        .Select(bc => bc.Category!);
                }
            }

            if (categoryID != null)
            {
                CategoryID = categoryID.Value;
            }
        }
    }
}
