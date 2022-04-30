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

namespace SWARM.Server.Application.GrdConvert
{
    public class GradeConversionController : BaseController<GradeConversion>, IBaseController<GradeConversion>
    {

        public GradeConversionController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor)
            : base(context, httpContextAccessor)
        {

        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get()
        {
            List<GradeConversion> lstGradeConversion = await _context.GradeConversions.ToListAsync();
            return Ok(lstGradeConversion);
        }

        [HttpGet]
        [Route("{KeyValue}")]
        Task<IActionResult> IBaseController<GradeConversion>.Get(int itemID)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("Get/{SchoolId}/{LetterGrade}")]
        public async Task<IActionResult> Get(int pSchoolId, int pLetterGrade)
        {
            GradeConversion itmGradeConversion = await _context.GradeConversions
                .Where(x => x.SchoolId == pSchoolId && x.LetterGrade == pLetterGrade.ToString()).FirstOrDefaultAsync();
            return Ok(itmGradeConversion);
        }

        [HttpDelete]
        [Route("{KeyValue}")]
        Task<IActionResult> IBaseController<GradeConversion>.Delete(int itemID)
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        [Route("Delete/{SchoolId}/{LetterGrade}")]
        public async Task<IActionResult> Delete(int pSchoolId, int pLetterGrade)
        {
            GradeConversion itmGradeCOnversion = await _context.GradeConversions
                .Where(x => x.SchoolId == pSchoolId && x.LetterGrade == pLetterGrade.ToString()).FirstOrDefaultAsync();
            _context.Remove(itmGradeCOnversion);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] GradeConversion _GradeConversion)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _GrdConvert = await _context.GradeConversions
                    .Where(x => x.SchoolId == _GradeConversion.SchoolId && x.LetterGrade == _GradeConversion.LetterGrade).FirstOrDefaultAsync();

                if (_GrdConvert == null)
                {
                    await Post(_GradeConversion);
                    return Ok();
                }


                _GrdConvert.SchoolId = _GradeConversion.SchoolId;
                _GrdConvert.LetterGrade = _GradeConversion.LetterGrade;
                _GrdConvert.GradePoint = _GradeConversion.GradePoint;
                _GrdConvert.MaxGrade = _GradeConversion.MaxGrade;
                _GrdConvert.MinGrade = _GradeConversion.MinGrade;
                _GrdConvert.School = _GradeConversion.School;

                _context.Update(_GrdConvert);
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
        public async Task<IActionResult> Post([FromBody] GradeConversion _GradeConversion)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _GrdConvert = await _context.GradeConversions
                    .Where(x => x.SchoolId == _GradeConversion.SchoolId && x.LetterGrade == _GradeConversion.LetterGrade).FirstOrDefaultAsync();

                if (_GrdConvert != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Record Exists");
                }

                _GrdConvert = new GradeConversion
                {
                    SchoolId = _GradeConversion.SchoolId,
                    LetterGrade = _GradeConversion.LetterGrade,
                    GradePoint = _GradeConversion.GradePoint,
                    MaxGrade = _GradeConversion.MaxGrade,
                    MinGrade = _GradeConversion.MinGrade,
                    School = _GradeConversion.School
                };

                _context.GradeConversions.Add(_GrdConvert);
                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_GrdConvert);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }




    }
}
