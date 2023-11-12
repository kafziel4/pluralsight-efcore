namespace PublisherConsole
{
    public class ImportAuthorDTO
    {
        private string _firstName;
        private string _lastName;

        public string FirstName => _firstName;
        public string LastName => _lastName;

        public ImportAuthorDTO(string firstName, string lastName)
        {
            _firstName = firstName;
            _lastName = lastName;
        }
    }
}
