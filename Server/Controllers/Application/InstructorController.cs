using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SWARM.EF.Data;
using SWARM.EF.Models;
using SWARM.Server.Controllers;
using SWARM.Server.Models;
using SWARM.Shared;
using SWARM.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
//using Telerik.DataSource;
//using Telerik.DataSource.Extensions;

namespace SWARM.Server.Application.Instruct
{
    public class InstructorController : BaseController<Instructor>, IBaseController<Instructor>
    {

        public InstructorController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor)
            : base(context, httpContextAccessor)
        {

        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get()
        {
            List<Instructor> lstInstructor = await _context.Instructors.ToListAsync();
            return Ok(lstInstructor);
        }

        [HttpGet]
        [Route("{KeyValue")]
        Task<IActionResult> IBaseController<Instructor>.Get(int itemID)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("Get/{SchoolId}/{InstructorId}")]
        public async Task<IActionResult> Get(int pSchoolId, int InstructorId)
        {
            Instructor itmInstructor = await _context.Instructors
                .Where(x => x.SchoolId == pSchoolId && x.InstructorId == InstructorId).FirstOrDefaultAsync();
            return Ok(itmInstructor);
        }

        [HttpDelete]
        [Route("{KeyValue")]
        Task<IActionResult> IBaseController<Instructor>.Delete(int itemID)
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        [Route("Delete/{SchoolId}/{InstructorId}")]
        public async Task<IActionResult> Delete(int pSchoolId, int pInstructor)
        {
            Instructor itmInstructor = await _context.Instructors
                .Where(x => x.SchoolId == pSchoolId && x.InstructorId == pInstructor).FirstOrDefaultAsync();
            _context.Remove(itmInstructor);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Instructor _Instructor)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _Instruct = await _context.Instructors
                    .Where(x => x.SchoolId == _Instructor.SchoolId && x.InstructorId == _Instructor.InstructorId).FirstOrDefaultAsync();

                if (_Instruct == null)
                {
                    await Post(_Instructor);
                    return Ok();
                }


                _Instruct.SchoolId = _Instructor.SchoolId;
                _Instruct.InstructorId = _Instructor.InstructorId;
                _Instruct.Salutation = _Instructor.Salutation;
                _Instruct.FirstName = _Instructor.FirstName;
                _Instruct.LastName = _Instructor.LastName;
                _Instruct.StreetAddress = _Instructor.StreetAddress;
                _Instruct.Zip = _Instructor.Zip;
                _Instruct.Phone = _Instructor.Phone;
                _Instruct.StreetAddress = _Instructor.StreetAddress;
                _Instruct.StreetAddress = _Instructor.StreetAddress;

                _context.Update(_Instruct);
                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok("Done");
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Instructor _Instructor)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _Instruct = await _context.Instructors
                    .Where(x => x.SchoolId == _Instructor.SchoolId && x.InstructorId == _Instructor.InstructorId).FirstOrDefaultAsync();

                if (_Instruct != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Record Exists");
                }

                _Instruct = new Instructor();
                _Instruct.SchoolId = _Instructor.SchoolId;
                _Instruct.InstructorId = _Instructor.InstructorId;
                _Instruct.Salutation = _Instructor.Salutation;
                _Instruct.FirstName = _Instructor.FirstName;
                _Instruct.LastName = _Instructor.LastName;
                _Instruct.StreetAddress = _Instructor.StreetAddress;
                _Instruct.Zip = _Instructor.Zip;
                _Instruct.Phone = _Instructor.Phone;
                _Instruct.StreetAddress = _Instructor.StreetAddress;
                _Instruct.StreetAddress = _Instructor.StreetAddress;

                _context.Instructors.Add(_Instruct);
                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Instruct);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}