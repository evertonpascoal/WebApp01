using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp01.Shared.Domain;

namespace WebApp01.Shared.Interfaces
{
    public interface IPersonRepository
    {
        Task<IEnumerable<Person>> GetAll();
        Task<Person> Get(Guid id);
        Task Add(Person person);
        Task Update(Person person);
        Task Delete(Person person);
    }
}
