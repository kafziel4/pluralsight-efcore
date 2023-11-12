using Microsoft.EntityFrameworkCore;
using PublisherData;
using PublisherDomain;

namespace PubAPI
{
    public class DataLogic
    {
        private PubContext _context;

        public DataLogic(PubContext context)
        {
            _context = context;
        }

        public async Task<List<AuthorDTO>> GetAllAuthors()
        {
            return await _context.Authors
                .Select(a => new AuthorDTO
                {
                    AuthorId = a.AuthorId,
                    FirstName = a.FirstName,
                    LastName = a.LastName
                })
                .ToListAsync();
        }

        public async Task<AuthorDTO?> GetAuthorById(int id)
        {
            var author = await _context.Authors.FindAsync(id);

            if (author == null)
            {
                return null;
            }

            return AuthorToDTO(author);
        }

        public async Task<AuthorDTO> SaveNewAuthor(AuthorDTO authorDTO)
        {
            var author = AuthorFromDTO(authorDTO);
            _context.Authors.Add(author);
            await _context.SaveChangesAsync();

            return AuthorToDTO(author);
        }

        public async Task<bool> UpdateAuthor(AuthorDTO authorDTO)
        {
            Author author = AuthorFromDTO(authorDTO);
            _context.Entry(author).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AuthorExists(authorDTO.AuthorId))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }

            return true;
        }

        public async Task<bool> DeleteAuthor(int id)
        {
            var recordCount = await _context.Database
                .ExecuteSqlInterpolatedAsync($"DELETE FROM authors WHERE authorid={id}");
            if (recordCount == 0)
            {
                return false;
            }

            return true;
        }

        private bool AuthorExists(int id)
        {
            return (_context.Authors.Any(e => e.AuthorId == id));
        }

        private static AuthorDTO AuthorToDTO(Author author)
        {
            return new AuthorDTO
            {
                AuthorId = author.AuthorId,
                FirstName = author.FirstName,
                LastName = author.LastName
            };
        }

        private static Author AuthorFromDTO(AuthorDTO authorDTO)
        {
            return new Author
            {
                AuthorId = authorDTO.AuthorId,
                FirstName = authorDTO.FirstName,
                LastName = authorDTO.LastName
            };
        }
    }
}
