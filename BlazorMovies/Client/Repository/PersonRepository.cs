using BlazorMovies.Client.Helpers;
using BlazorMovies.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorMovies.Client.Repository
{
    public class PersonRepository : IPersonRepository
    {
        private const string URL = "api/people";

        private readonly IHttpService _httpService;

        public PersonRepository(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<List<Person>> GetPeople()
        {
            var response = await _httpService.Get<List<Person>>(URL);
            if (!response.Success)
                throw new ApplicationException(await response.GetBody());

            return response.Response;
        }

        public async Task CreatePerson(Person person)
        {
            var response = await _httpService.Post(URL, person);

            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
        }

    }
}
