
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Rest.Models;
using Rest.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static MongoDB.Driver.WriteConcern;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Rest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenerationController : ControllerBase
    {
        private IRepository<Generation> _repository;
        public GenerationController()
        {
            _repository = new GenerationRepository(DBContext.DBContext.GetInstance());
        }

        // GET: api/<GenerationController>
        [HttpGet]
        public List<Generation> Get()
        {
            return _repository.GetAll();
        }

        // GET api/<GenerationController>/5
        [HttpGet("{id}")]
        public Generation Get(string id)
        {
            return _repository.GetById(id);
        }

        // POST api/<GenerationController>
        [HttpPost]
        public void Post([FromBody] Generation value)
        {
            if(value == null) {
                return;
            }

            _repository.Add(value);
        }

        // PUT api/<GenerationController>/5
        [HttpPut("{id}")]
        public void Put(string id, [FromBody] Generation value)
        {
            if (value == null)
            {
                return;
            }
            _repository.Modify(id, value);
        }

        // DELETE api/<GenerationController>/5
        [HttpDelete("{id}")]
        public void Delete(string id)
        {   

            _repository.RemoveById(id);
        }
    }
}
