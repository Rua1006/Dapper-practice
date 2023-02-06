using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using WebApplication2.Dapper.Models;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LanguageController : ControllerBase
    {
        private readonly IConfiguration _config;

        public LanguageController(IConfiguration config) 
        {
            _config = config;
        }

        [HttpGet]
        public async Task<ActionResult<List<Language>>> GetAllLanguage()
        {
            MySqlConnection connection = conn();
            IEnumerable<Language> languages = await SelectAllLanguages(connection);
            return Ok(languages);
        }

        [HttpGet("{language_id}")]
        public async Task<ActionResult<Language>> GetLanguage(int language_id)
        {
            MySqlConnection connection = conn();
            var language = await connection.QueryFirstAsync<Language>("select * from language where language_id=@Id",
                new {Id = language_id});
            return Ok(language);
        }

        [HttpPost]
        public async Task<ActionResult<List<Language>>> CreateLanguage(Language language)
        {
            MySqlConnection connection = conn();
            await connection.ExecuteAsync("insert into language (language_id, name, last_update) values (@language_id, @name, @last_update)", language);
            return Ok(await SelectAllLanguages(connection));
        }

        [HttpPut("{language_id}")]
        public async Task<ActionResult<List<Language>>> UpdateLanguage(Language language)
        {
            MySqlConnection connection = conn();
            await connection.ExecuteAsync("update language set name=@name, last_update=@last_update where language_id=@language_id", language);
            return Ok(await SelectAllLanguages(connection));
        }

        [HttpDelete("{language_id}")]
        public async Task<ActionResult<List<Language>>> DeleteLanguage(int language_id)
        {
            MySqlConnection connection = conn();
            await connection.ExecuteAsync("delete from language where language_id=@Id",
                new {Id = language_id});
            return Ok(await SelectAllLanguages(connection));
        }

        private static async Task<IEnumerable<Language>> SelectAllLanguages(MySqlConnection connection)
        {
            return await connection.QueryAsync<Language>("select * from language");
        }

        private MySqlConnection conn()
        {
            return new MySqlConnection(_config.GetConnectionString("DefaultConnection"));
        }
    }
}
