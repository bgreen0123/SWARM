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
    public class GradeTypeController : BaseController<GradeType>, IBaseController<GradeType>
    {

        public GradeTypeController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor)
            : base(context, httpContextAccessor)
        {

        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get()
        {
            List<GradeType> lstGradeType = await _context.GradeTypes.ToListAsync();
            return Ok(lstGradeType);
        }

        [HttpGet]
        [Route("{KeyValue}")]
        Task<IActionResult> IBaseController<GradeType>.Get(int KeyValue)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("Get/{pSchoolId}/{pGradeTypeCode}")]
        public async Task<IActionResult> Get(int pSchoolId, int pGradeTypeCode)
        {
            GradeType itmGradeTypeCode = await _context.GradeTypes
                .Where(x => x.SchoolId == pSchoolId && x.GradeTypeCode == pGradeTypeCode.ToString()).FirstOrDefaultAsync();
            return Ok(itmGradeTypeCode);
        }

        [HttpDelete]
        [Route("{KeyValue}")]
        Task<IActionResult> IBaseController<GradeType>.Delete(int itemID)
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        [Route("Delete/{pSchoolId}/{pGradeTypeCode}")]
        public async Task<IActionResult> Delete(int pSchoolId, int pGradeTypeCode)
        {
            GradeType itmGradeTypeCode = await _context.GradeTypes
                .Where(x => x.SchoolId == pSchoolId && x.GradeTypeCode == pGradeTypeCode.ToString()).FirstOrDefaultAsync();
            _context.Remove(itmGradeTypeCode);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] GradeType _GradeType)
        {
            var _GrdType = await _context.GradeTypes
                .Where(x => x.SchoolId == _GradeType.SchoolId && x.GradeTypeCode == _GradeType.GradeTypeCode).FirstOrDefaultAsync();

            if (_GrdType == null)
            {
                await Post(_GradeType);
                return Ok();
            }

            var trans = _context.Database.BeginTransaction();
            try
            {
                _GrdType.SchoolId = _GradeType.SchoolId;
                _GrdType.GradeTypeCode = _GradeType.GradeTypeCode;
                _GrdType.Description = _GradeType.Description;
                _GrdType.School = _GradeType.School;
                _GrdType.GradeTypeWeights = _GradeType.GradeTypeWeights;

                _context.Update(_GrdType);
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
        public async Task<IActionResult> Post([FromBody] GradeType _GradeType)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _GrdType = await _context.GradeTypes
                    .Where(x => x.SchoolId == _GradeType.SchoolId && x.GradeTypeCode == _GradeType.GradeTypeCode).FirstOrDefaultAsync();

                if (_GrdType != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Record Exists");
                }

                _GrdType = new GradeType
                {
                    SchoolId = _GradeType.SchoolId,
                    GradeTypeCode = _GradeType.GradeTypeCode,
                    Description = _GradeType.Description,
                    School = _GradeType.School,
                    GradeTypeWeights = _GradeType.GradeTypeWeights
                };

                _context.GradeTypes.Add(_GrdType);
                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_GrdType);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }




    }
}