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

namespace SWARM.Server.Application.GrdType
{
    public class GradeTypeWeightController : BaseController<GradeTypeWeight>, IBaseController<GradeTypeWeight>
    {

        public GradeTypeWeightController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor)
            : base(context, httpContextAccessor)
        {

        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get()
        {
            List<GradeTypeWeight> lstGradeTypeWeight = await _context.GradeTypeWeights.ToListAsync();
            return Ok(lstGradeTypeWeight);
        }

        [HttpGet]
        [Route("{KeyValue}")]
        Task<IActionResult> IBaseController<GradeTypeWeight>.Get(int KeyValue)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("Get/{pSchoolId}/{pSectionId}/{pGradeTypeCode}")]
        public async Task<IActionResult> Get(int pSchoolId, int pSectionId, int pGradeTypeCode)
        {
            GradeTypeWeight itmGradeTypeCodeWeight = await _context.GradeTypeWeights
                .Where(x => x.SchoolId == pSchoolId && x.SectionId == pSectionId && x.GradeTypeCode == pGradeTypeCode.ToString()).FirstOrDefaultAsync();
            return Ok(itmGradeTypeCodeWeight);
        }

        [HttpDelete]
        [Route("{KeyValue}")]
        Task<IActionResult> IBaseController<GradeTypeWeight>.Delete(int KeyValue)
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        [Route("Delete/{pSchoolId}/{pSectionId}/{pGradeTypeCode}")]
        public async Task<IActionResult> Delete(int pSchoolId, int pSectionId, int pGradeTypeCode)
        {
            GradeTypeWeight itmGradeTypeCodeWeight = await _context.GradeTypeWeights
                .Where(x => x.SchoolId == pSchoolId && x.SectionId == pSectionId && x.GradeTypeCode == pGradeTypeCode.ToString()).FirstOrDefaultAsync();
            _context.Remove(itmGradeTypeCodeWeight);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] GradeTypeWeight _GradeTypeWeight)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _GrdTypeWeight = await _context.GradeTypeWeights
                    .Where(x => x.SchoolId == _GradeTypeWeight.SchoolId && x.SectionId == _GradeTypeWeight.SectionId && x.GradeTypeCode == _GradeTypeWeight.GradeTypeCode).FirstOrDefaultAsync();

                if (_GrdTypeWeight == null)
                {
                    await Post(_GradeTypeWeight);
                    return Ok();
                }


                _GrdTypeWeight.SchoolId = _GradeTypeWeight.SchoolId;
                _GrdTypeWeight.SectionId = _GrdTypeWeight.SectionId;
                _GrdTypeWeight.GradeTypeCode = _GradeTypeWeight.GradeTypeCode;
                _GrdTypeWeight.NumberPerSection = _GradeTypeWeight.NumberPerSection;
                _GrdTypeWeight.PercentOfFinalGrade = _GradeTypeWeight.PercentOfFinalGrade;
                _GrdTypeWeight.DropLowest = _GradeTypeWeight.DropLowest;
                
                _context.Update(_GrdTypeWeight);
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
        public async Task<IActionResult> Post([FromBody] GradeTypeWeight _GradeTypeWeight)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _GrdTypeWeight = await _context.GradeTypeWeights
                    .Where(x => x.SchoolId == _GradeTypeWeight.SchoolId && x.SectionId == _GradeTypeWeight.SectionId && x.GradeTypeCode == _GradeTypeWeight.GradeTypeCode).FirstOrDefaultAsync();

                if (_GrdTypeWeight != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Record Exists");
                }

                _GrdTypeWeight = new GradeTypeWeight
                {
                    SchoolId = _GradeTypeWeight.SchoolId,
                    SectionId = _GrdTypeWeight.SectionId,
                    GradeTypeCode = _GradeTypeWeight.GradeTypeCode,
                    NumberPerSection = _GradeTypeWeight.NumberPerSection,
                    PercentOfFinalGrade = _GradeTypeWeight.PercentOfFinalGrade,
                    DropLowest = _GradeTypeWeight.DropLowest
                };

                _context.GradeTypeWeights.Add(_GrdTypeWeight);
                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_GrdTypeWeight);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }




    }
}