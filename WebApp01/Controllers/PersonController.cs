using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp01.Shared.Domain;
using WebApp01.Shared.Interfaces;
using WebApp01.Web.DTOs;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApp01.Web.Controllers
{
    [Route("api/person")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly IPersonService _personService;
        private readonly IMapper _mapper;

        public PersonController(IPersonService personService, IMapper mapper)
        {
            _personService = personService;
            _mapper = mapper;
        }

        // GET: api/person
        /// <summary>
        /// Recupera todas as pessoas cadastradas
        /// </summary>
        /// <returns>Lista de pessoas cadastradas</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PersonDTO>>> Get()
        {
            var personList = await _personService.GetAll();
            return Ok(_mapper.Map<IEnumerable<PersonDTO>>(personList));
        }

        // GET api/person/{id}
        /// <summary>
        /// Recupera uma Pessoa utilizando o ID
        /// </summary>
        /// <param name="id">Indentificador da Pessoa</param>
        /// <returns>Retorna uma pessoa</returns>
        [HttpGet("{id}", Name ="GetPerson")]
        public async Task<ActionResult<PersonDTO>> Get(Guid id)
        {
            var person = await _personService.Get(id);
            if (person == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<PersonDTO>(person));
        }

        // POST api/person
        [HttpPost]
        public async Task<ActionResult<PersonDTO>> Post([FromBody] PersonForCreationDTO personDTO)
        {
            var person = _mapper.Map<Person>(personDTO);

            var personToReturn = await _personService.Add(person);
            var personDTOToReturn = _mapper.Map<PersonDTO>(personToReturn);
            
            return CreatedAtRoute("GetPerson",
                new { id = personDTOToReturn.Id },
                personDTOToReturn);
        }

        // PUT api/person/ID
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/person/
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
