using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BlazorMovies.Server.Data;
using BlazorMovies.Server.Helpers;
using BlazorMovies.Shared.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazorMovies.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IFileStorageService _fileStorageService;
        private readonly IMapper _mapper;

        public PeopleController(DataContext context, IFileStorageService fileStorageService, IMapper mapper)
        {
            _context = context;
            _fileStorageService = fileStorageService;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Person>> Get(int id)
        {
            var person = await _context.Persons.FirstOrDefaultAsync(p => id == p.Id);

            if (null == person)
                return NotFound();

            return person;
        }

        [HttpGet]
        public async Task<ActionResult<List<Person>>> Get()
        {
            return await _context.Persons.ToListAsync();
        }

        [HttpGet("search/{searchText}")]
        public async Task<ActionResult<List<Person>>> FilterByName(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
                return new List<Person>();

            return await _context.Persons.Where(p => p.Name.ToLower().Contains(searchText.ToLower()))
                .Take(5) //TODO: pagination
                .ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post(Person person)
        {
            if (!string.IsNullOrWhiteSpace(person.Picture))
            {
                var personPicture = Convert.FromBase64String(person.Picture);
                //The extension jpg works with *.gif and *png as well
                person.Picture = await _fileStorageService.SaveFile(personPicture, "jpg", "people");
            }

            _context.Add(person);
            await _context.SaveChangesAsync();
            return person.Id;
        }

        [HttpPut]
        public async Task<IActionResult> Put(Person person)
        {
            var personDb = await _context.Persons.FirstOrDefaultAsync(p => p.Id == person.Id);

            if (null == personDb)
                return NotFound();

            personDb = _mapper.Map(person, personDb);

            if (!string.IsNullOrWhiteSpace(person.Picture))
            {
                var personPicture = Convert.FromBase64String(person.Picture);
                person.Picture = await _fileStorageService.EditFile(personPicture, "jpg", "people", personDb.Picture);
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var peopleToDelete = await _context.Persons.FirstOrDefaultAsync(p => id == p.Id);
            if (null == peopleToDelete)
                return NotFound($"No person found with ID: {id}");


            _context.Persons.Remove(peopleToDelete);

            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
