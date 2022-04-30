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

namespace SWARM.Server.Application.Stu
{
    public class StudentController : BaseController<Student>, IBaseController<Student>
    {

        public StudentController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor)
            : base(context, httpContextAccessor)
        {

        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get()
        {
            List<Student> lstStudent = await _context.Students.ToListAsync();
            return Ok(lstStudent);
        }

        [HttpGet]
        [Route("{KeyValue}")]
        Task<IActionResult> IBaseController<Student>.Get(int itemID)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("Get/{SchoolId}")]
        public async Task<IActionResult> Get(int pStudentId)
        {
            Student itmStudent = await _context.Students
                .Where(x => x.StudentId == pStudentId).FirstOrDefaultAsync();
            return Ok(itmStudent);
        }

        [HttpDelete]
        [Route("{KeyValue}")]
        Task<IActionResult> IBaseController<Student>.Delete(int itemID)
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        [Route("Delete/{StudentId}")]
        public async Task<IActionResult> Delete(int pStudentId)
        {
            Student itmSchool = await _context.Students
                .Where(x => x.StudentId == pStudentId).FirstOrDefaultAsync();
            _context.Remove(itmSchool);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Student _Student)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _Stu = await _context.Students
                    .Where(x => x.SchoolId == _Student.StudentId).FirstOrDefaultAsync();

                if (_Stu == null)
                {
                    await Post(_Student);
                    return Ok();
                }


                _Stu.StudentId = _Student.StudentId;
                _Stu.Salutation = _Student.Salutation;
                _Stu.FirstName = _Student.FirstName;
                _Stu.StreetAddress = _Student.StreetAddress;
                _Stu.Zip = _Student.Zip;
                _Stu.Phone = _Student.Phone;
                _Stu.Employer = _Student.Employer;
                _Stu.RegistrationDate = _Student.RegistrationDate;

                _context.Update(_Stu);
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
        public async Task<IActionResult> Post([FromBody] Student _Student)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _Stu = await _context.Students
                    .Where(x => x.StudentId == _Student.StudentId).FirstOrDefaultAsync();

                if (_Stu != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Record Exists");
                }

                _Stu = new Student();
                _Stu.StudentId = _Student.StudentId;
                _Stu.Salutation = _Student.Salutation;
                _Stu.FirstName = _Student.FirstName;
                _Stu.StreetAddress = _Student.StreetAddress;
                _Stu.Zip = _Student.Zip;
                _Stu.Phone = _Student.Phone;
                _Stu.Employer = _Student.Employer;
                _Stu.RegistrationDate = _Student.RegistrationDate;

                _context.Students.Add(_Stu);
                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Stu);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}