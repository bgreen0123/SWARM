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

namespace SWARM.Server.Application.Grd
{
    public class GradeController : BaseController<Grade>, IBaseController<Grade>
    {

        public GradeController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor)
            : base(context, httpContextAccessor)
        {

        }


        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get()
        {
            List<Grade> lstGrade = await _context.Grades.ToListAsync();
            return Ok(lstGrade);
        }

        [HttpGet]
        [Route("{KeyValue")]
        Task<IActionResult> IBaseController<Grade>.Get(int itemID)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("Get/{SchoolId}/{StudentId}/{SectionId}/{GradeTypeCode}")]
        public async Task<IActionResult> Get(int pSchoolId, int pStudentId, int pSectionId, int pGradeTypeCode)
        {
            Grade itmGrade = await _context.Grades
                .Where(x => x.SchoolId == pSchoolId && x.StudentId == pStudentId && x.SectionId == pSectionId && x.GradeTypeCode == pGradeTypeCode.ToString()).FirstOrDefaultAsync();
            return Ok(itmGrade);
        }

        [HttpDelete]
        [Route("{KeyValue")]
        Task<IActionResult> IBaseController<Grade>.Delete(int itemID)
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        [Route("Delete/{SchoolId}/{StudentId}/{SectionId}/{GradeTypeCode}")]
        public async Task<IActionResult> Delete(int pSchoolId, int pStudentId, int pSectionId, string pGradeTypeCode)
        {
            Grade itmGrade = await _context.Grades
                .Where(x => x.SchoolId == pSchoolId && x.StudentId == pStudentId && x.SectionId == pSectionId && x.GradeTypeCode == pGradeTypeCode).FirstOrDefaultAsync();
            _context.Remove(itmGrade);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Grade _Grade)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _Grd = await _context.Grades
                    .Where(x => x.SchoolId == _Grade.SchoolId && x.StudentId == _Grade.StudentId && x.SectionId == _Grade.SectionId && x.GradeTypeCode == _Grade.GradeTypeCode).FirstOrDefaultAsync();

                if (_Grd == null)
                {
                    await Post(_Grade);
                    return Ok();
                }


                _Grd.SchoolId = _Grade.SchoolId;
                _Grd.StudentId = _Grade.StudentId;
                _Grd.SectionId = _Grade.SectionId;
                _Grd.GradeTypeCode = _Grade.GradeTypeCode;
                _Grd.GradeCodeOccurrence = _Grade.GradeCodeOccurrence;
                _Grd.NumericGrade = _Grade.NumericGrade;
                _Grd.Comments = _Grade.Comments;
                
                _context.Update(_Grd);
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
        public async Task<IActionResult> Post([FromBody] Grade _Grade)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _Grd = await _context.Grades
                    .Where(x => x.SchoolId == _Grade.SchoolId && x.StudentId == _Grade.StudentId && x.SectionId == _Grade.SectionId && x.GradeTypeCode == _Grade.GradeTypeCode).FirstOrDefaultAsync();

                if (_Grd != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Record Exists");
                }

                _Grd = new Grade();
                _Grd.SchoolId = _Grade.SchoolId;
                _Grd.StudentId = _Grade.StudentId;
                _Grd.SectionId = _Grade.SectionId;
                _Grd.GradeTypeCode = _Grade.GradeTypeCode;
                _Grd.GradeCodeOccurrence = _Grade.GradeCodeOccurrence;
                _Grd.NumericGrade = _Grade.NumericGrade;
                _Grd.Comments = _Grade.Comments;

                _context.Grades.Add(_Grd);
                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Grd);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }




    }
}
