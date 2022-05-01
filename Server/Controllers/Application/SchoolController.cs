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

namespace SWARM.Server.Application.S
{
    public class SchoolController : BaseController<School>, IBaseController<School>
    {

        public SchoolController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor)
            : base(context, httpContextAccessor)
        {

        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get()
        {
            List<School> lstSchool = await _context.Schools.ToListAsync();
            return Ok(lstSchool);
        }

        [HttpGet]
        [Route("Get/{pSchoolId}")]
        public async Task<IActionResult> Get(int pSchoolId)
        {
            School itmSchool = await _context.Schools
                .Where(x => x.SchoolId == pSchoolId).FirstOrDefaultAsync();
            return Ok(itmSchool);
        }

        [HttpDelete]
        [Route("Delete/{KeyValue}")]
        Task<IActionResult> IBaseController<School>.Delete(int KeyValue)
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        [Route("Delete/{SchoolId}")]
        public async Task<IActionResult> Delete(int pSchoolId)
        {
            School itmSchool = await _context.Schools
                .Where(x => x.SchoolId == pSchoolId).FirstOrDefaultAsync();
            _context.Remove(itmSchool);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] School _School)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _s = await _context.Schools
                    .Where(x => x.SchoolId == _School.SchoolId).FirstOrDefaultAsync();

                if (_s == null)
                {
                    await Post(_School);
                    return Ok();
                }


                _s.SchoolId = _School.SchoolId;
                _s.SchoolName = _School.SchoolName;

                _context.Update(_s);
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
        public async Task<IActionResult> Post([FromBody] School _School)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _s = await _context.Schools
                    .Where(x => x.SchoolId == _School.SchoolId).FirstOrDefaultAsync();

                if (_s != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Record Exists");
                }

                _s = new School();
                _s.SchoolId = _School.SchoolId;
                _s.SchoolName = _School.SchoolName;

                _context.Schools.Add(_s);
                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_s);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}