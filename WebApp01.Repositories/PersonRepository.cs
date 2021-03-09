using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp01.Shared.Interfaces;
using Dapper;
using WebApp01.Shared.Domain;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace WebApp01.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private readonly IConfiguration _configuration;

        public PersonRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task Add(Person person)
        {
            string sql = "INSERT INTO Person (Id,Name, DateOfBith, Nickname, Weight)" +
                " VALUES (@Id, @Name, @DateOfBith, @Nickname, @Weight)";
            
            using var connection = new SqlConnection(_configuration.GetConnectionString("WebApp01DataBase"));            
            await connection.ExecuteAsync(sql, person, commandType: System.Data.CommandType.Text);
        }
        public async Task Update(Person person)
        {
            throw new NotImplementedException();
        }
        public async Task Delete(Person person)
        {
            throw new NotImplementedException();
        }

        public async Task<Person> Get(Guid id)
        {
            string sql = "SELECT Id,Name, DateOfBith, Nickname, Weight FROM PERSON WHERE Id =@id";

            using var connection = new SqlConnection(_configuration.GetConnectionString("WebApp01DataBase"));
            return await connection.QueryFirstOrDefaultAsync<Person>(sql, new { id }, commandType: System.Data.CommandType.Text);
        }

        public async Task<IEnumerable<Person>> GetAll()
        {
            string sql = "SELECT Id,Name, DateOfBith, Nickname, Weight FROM PERSON ";

            using var connection = new SqlConnection(_configuration.GetConnectionString("WebApp01DataBase"));
            return await connection.QueryAsync<Person>(sql, commandType: System.Data.CommandType.Text);
        }

       
    }
}
