using Microsoft.AspNetCore.Mvc;

namespace PubAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly DataLogic _dataLogic;

        public AuthorsController(DataLogic dataLogic)
        {
            _dataLogic = dataLogic;
        }

        // GET: api/Authors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorDTO>>> GetAuthors()
        {
            var authorDTOList = await _dataLogic.GetAllAuthors();
            return Ok(authorDTOList);
        }

        // GET: api/Authors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorDTO>> GetAuthor(int id)
        {
            var authorDTO = await _dataLogic.GetAuthorById(id);

            if (authorDTO == null)
            {
                return NotFound();
            }

            return Ok(authorDTO);
        }

        // PUT: api/Authors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAuthor(int id, AuthorDTO authorDTO)
        {
            if (id != authorDTO.AuthorId)
            {
                return BadRequest();
            }

            var result = await _dataLogic.UpdateAuthor(authorDTO);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/Authors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AuthorDTO>> PostAuthor(AuthorDTO authorDTO)
        {
            var newAuthor = await _dataLogic.SaveNewAuthor(authorDTO);

            return CreatedAtAction("GetAuthor", new { id = newAuthor.AuthorId }, newAuthor);
        }

        // DELETE: api/Authors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var result = await _dataLogic.DeleteAuthor(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
