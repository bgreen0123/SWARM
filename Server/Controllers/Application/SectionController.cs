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

namespace SWARM.Server.Application.Sec
{
    public class SectionController : BaseController<Section>, IBaseController<Section>
    {
        public SectionController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor)
            : base(context, httpContextAccessor)
        {

        }

        [HttpGet]
        [Route("GetSection")]
        public async Task<IActionResult> Get()
        {
            List<Section> itmSection = await _context.Sections.ToListAsync();
            return Ok(itmSection);
        }

        [HttpGet]
        [Route("GetSection/{pSectionNo}")]
        public async Task<IActionResult> Get(int pSectionNo)
        {
            Section itmSection = await _context.Sections.Where(x => x.SectionNo == pSectionNo).FirstOrDefaultAsync();
            return Ok(itmSection);
        }

        [HttpDelete]
        [Route("Delete/{pSectionNo}")]
        public async Task<IActionResult> Delete(int pSectionNo)
        {
            Section itmSection = await _context.Sections.Where(x => x.SectionNo == pSectionNo).FirstOrDefaultAsync();
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Section _Section)
        {
            bool bExists = false;

            var trans = _context.Database.BeginTransaction();
            try
            {
                var _Sec = await _context.Sections.Where(x => x.SectionNo == _Section.SectionNo).FirstOrDefaultAsync();

                if (_Sec == null)
                {
                    bExists = false;
                    _Sec = new Section();
                }
                else
                    bExists = true;

                _Sec.CourseNo = _Section.CourseNo;
                _Sec.SectionNo = _Section.SectionNo;
                _Sec.StartDateTime = _Section.StartDateTime;
                _Sec.Location = _Section.Location;
                _Sec.InstructorId = _Section.InstructorId;
                _Sec.Capacity = _Section.Capacity;
                _Sec.SchoolId = _Section.SchoolId;
                _Sec.Course = _Section.Course;
                _Sec.Instructor = _Section.Instructor;
                _Sec.School = _Section.School;
                _Sec.Enrollments = _Section.Enrollments;
                _Sec.GradeTypeWeights= _Section.GradeTypeWeights;
                _context.Update(_Sec);
                await _context.SaveChangesAsync();
                if (bExists)
                {
                    _context.Sections.Update(_Sec);
                }
                else
                    _context.Sections.Add(_Sec);

                trans.Commit();

                return Ok(_Section.SectionNo);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Section _Section)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _Sec = await _context.Sections.Where(x => x.SectionNo == _Section.SectionNo).FirstOrDefaultAsync();

                if (_Sec == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Record Exists");
                }

                _Sec = new Section();
                _Sec.SectionId = _Section.SectionId;
                _Sec.CourseNo = _Section.CourseNo;
                _Sec.SectionNo = _Section.SectionNo;
                _Sec.StartDateTime = _Section.StartDateTime;
                _Sec.Location = _Section.Location;
                _Sec.InstructorId = _Section.InstructorId;
                _Sec.Capacity = _Section.Capacity;
                _Sec.SchoolId = _Section.SchoolId;
                _Sec.Course = _Section.Course;
                _Sec.Instructor = _Section.Instructor;
                _Sec.School = _Section.School;
                _Sec.Enrollments = _Section.Enrollments;
                _Sec.GradeTypeWeights = _Section.GradeTypeWeights;
                _context.Update(_Sec);
                await _context.SaveChangesAsync();
                _context.Sections.Add(_Sec);

                trans.Commit();

                return Ok(_Section.SectionNo);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        
    }
}
