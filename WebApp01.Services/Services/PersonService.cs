using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp01.Shared.Domain;
using WebApp01.Shared.Interfaces;

namespace WebApp01.Services.Services
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _personRepository;

        public PersonService(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public async Task<Person> Add(Person person)
        {
            person.Id = Guid.NewGuid();
            await _personRepository.Add(person);

            return person;
        }
        public async Task Update()
        {
            throw new NotImplementedException();
        }

        public async Task Delete()
        {
            throw new NotImplementedException();
        }

        public async Task<Person> Get(Guid id)
        {
            return await _personRepository.Get(id);
        }

        public async Task<IEnumerable<Person>> GetAll()
        {
            return await _personRepository.GetAll();
        }

        
    }
}
