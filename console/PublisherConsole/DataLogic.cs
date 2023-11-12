using PublisherData;
using PublisherDomain;

namespace PublisherConsole
{
    public class DataLogic
    {
        private PubContext _context;

        public DataLogic()
        {
            _context = new PubContext();
        }

        public DataLogic(PubContext context)
        {
            _context = context;
        }

        public int ImportAuthors(List<ImportAuthorDTO> authorList)
        {
            foreach (var author in authorList)
            {
                _context.Authors.Add(
                    new Author { FirstName = author.FirstName, LastName = author.LastName });
            }
            return _context.SaveChanges();
        }
    }
}
