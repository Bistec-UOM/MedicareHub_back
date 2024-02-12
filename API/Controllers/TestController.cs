﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly TestService _tmpl;

        public TestController(TestService tmpl)
        {
            _tmpl = tmpl;
        }

        //Get the list of all lab tests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Test>>> GetAllTests()
        {
            
            return Ok(await _tmpl.GetAllTests());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Test>> GetTest(int id)
        {
                var tst = await _tmpl.GetTest(id);
                return Ok(tst);
        }

        [HttpPost]
        public async Task<ActionResult<Test>> AddTest(Test item)
        {
            await _tmpl.Addtest(item);
            return CreatedAtAction(nameof(GetTest), new { id = item.TestId }, item);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTest(int id)
        {
            await _tmpl.DeleteTest(id);
            return NoContent();
        }

        //[HttpPut("{id}")]
        //public async Task<ActionResult> EditTest(int id, Test item)
        //{
        //    if (id != item.Id)
        //    {
        //        return BadRequest();
        //    }

        //    try
        //    {
        //        await _tmpl.EditTest(item);
        //        return Ok();
        //    }
        //    catch (KeyNotFoundException)
        //    {
        //        return NotFound();
        //    }
        //}
    }
 }
