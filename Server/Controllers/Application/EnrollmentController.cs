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

namespace SWARM.Server.Application.Enroll
{
    public class EnrollmentController : BaseController<Enrollment>, IBaseController<Enrollment>
    {

        public EnrollmentController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor)
            : base(context, httpContextAccessor)
        {

        }


        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get()
        {
            List<Enrollment> itmEnrollment = await _context.Enrollments.ToListAsync();
            return Ok(itmEnrollment);
        }

        [HttpGet]
        [Route("Get/{KeyValue}")]
        public Task<IActionResult> Get(int pKeyValue)
        {
            throw new NotImplementedException("Need more information");
        }

        [HttpGet]
        [Route("Get/{pStudentId}/{SectionId}")]
        public async Task<IActionResult> Get(int pStudentId, int pSectionId)
        {
            Enrollment itmEnrollment = await _context.Enrollments
                            .Where(x => x.StudentId == pStudentId && x.SectionId == pSectionId).FirstOrDefaultAsync();
            return Ok(itmEnrollment);
        }



        [HttpDelete]
        [Route("Delete/{KeyValue}")]
        Task<IActionResult> IBaseController<Enrollment>.Delete(int itemID)
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        [Route("Delete/{StudentId}/{SectionId}")]
        public async Task<IActionResult> Delete(int pStudentId, int pSectionId)
        {
            Enrollment itmEnrollment = await _context.Enrollments
                .Where(x => x.StudentId == pStudentId && x.SectionId == pSectionId).FirstOrDefaultAsync();
            _context.Remove(itmEnrollment);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Enrollment _Enrollment)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _Eroll = await _context.Enrollments.Where(x => x.StudentId == _Enrollment.StudentId && x.SectionId == _Enrollment.SectionId).FirstOrDefaultAsync();

                if (_Eroll != null)
                {
                    await Post(_Enrollment);
                    return Ok();
                }

                _Eroll = new Enrollment();
                _Eroll.StudentId = _Enrollment.StudentId;
                _Eroll.SectionId = _Enrollment.SectionId;
                _Eroll.EnrollDate = _Enrollment.EnrollDate;
                _Eroll.FinalGrade = _Enrollment.FinalGrade;
                _Eroll.SchoolId = _Enrollment.SchoolId;
                _context.Update(_Eroll);

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
        public async Task<IActionResult> Post([FromBody] Enrollment _Enrollment)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _Eroll = await _context.Enrollments.Where(x => x.StudentId == _Enrollment.StudentId && x.SectionId == _Enrollment.SectionId).FirstOrDefaultAsync();

                if (_Eroll != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Record Exists");
                }

                _Eroll = new Enrollment();
                _Eroll.StudentId = _Enrollment.StudentId;
                _Eroll.SectionId = _Enrollment.SectionId;
                _Eroll.EnrollDate = _Enrollment.EnrollDate;
                _Eroll.FinalGrade = _Enrollment.FinalGrade;
                _Eroll.SchoolId = _Enrollment.SchoolId;

                _context.Enrollments.Add(_Eroll);

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
    }
}
