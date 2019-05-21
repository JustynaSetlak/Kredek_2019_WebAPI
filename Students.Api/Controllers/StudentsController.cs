using Microsoft.AspNetCore.Mvc;
using Students.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Students.Api.Controllers
{
    [Route("api/students")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private static Dictionary<Guid, Student> _students;

        static StudentsController()
        {
            _students = new Dictionary<Guid, Student>();
        }

        [HttpGet]
        public IActionResult Get()
        {
            var result = _students.ToList();

            return Ok(result);
        }

        [HttpGet("{id}", Name = "GetStudent")]
        public IActionResult Get(string id)
        {
            var parsingResult = Guid.TryParse(id, out Guid parsedId);

            if (!parsingResult)
            {
                ModelState.AddModelError("invalid_id", "Invalid format of identificator");
                return BadRequest(ModelState);
            }

            var searchingResult = _students.TryGetValue(parsedId, out Student result);

            if (!searchingResult)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Student student)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newIdentificator = Guid.NewGuid();

            _students.Add(newIdentificator, student);
            return CreatedAtRoute("GetStudent", new { id = newIdentificator.ToString() }, student);
        }

        [HttpPut("{id}")]
        public IActionResult Put(string id, [FromBody] Student student)
        {
            var parsingResult = Guid.TryParse(id, out Guid parsedId);

            if (!parsingResult)
            {
                ModelState.AddModelError("invalid_id", "Invalid format of identificator");
                return BadRequest(ModelState);
            }

            var searchingResult = _students.TryGetValue(parsedId, out Student existingStudent);

            if (!searchingResult)
            {
                return NotFound();
            }

            existingStudent.FirstName = student.FirstName;
            existingStudent.Surname = student.Surname;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var parsingResult = Guid.TryParse(id, out Guid parsedId);

            if (!parsingResult)
            {
                ModelState.AddModelError("invalid_id", "Invalid format of identificator");
                return BadRequest(ModelState);
            }

            var searchingResult = _students.TryGetValue(parsedId, out Student existingStudent);

            if (!searchingResult)
            {
                return NotFound();
            }

            _students.Remove(parsedId);

            return NoContent();
        }
    }
}
