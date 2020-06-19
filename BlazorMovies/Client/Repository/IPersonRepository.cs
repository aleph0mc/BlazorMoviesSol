using BlazorMovies.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorMovies.Client.Repository
{
    interface IPersonRepository
    {
        Task CreatePerson(Person person);
        Task DeletePerson(int id);
        Task<List<Person>> GetPeople();
        Task<List<Person>> GetPeopleByName(string name);
        Task<Person> GetPerson(int id);
        Task UpdatePerson(Person person);
    }
}
