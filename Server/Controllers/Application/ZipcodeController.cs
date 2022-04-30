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

namespace SWARM.Server.Application.Zip
{
    public class ZipcodeController : BaseController<Zipcode>, IBaseController<Zipcode>
    {

        public ZipcodeController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor)
            : base(context, httpContextAccessor)
        {

        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get()
        {
            List<Zipcode> lstZipcode = await _context.Zipcodes.ToListAsync();
            return Ok(lstZipcode);
        }

        [HttpGet]
        [Route("{KeyValue}")]
        Task<IActionResult> IBaseController<Zipcode>.Get(int itemID)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("Get/{Zip}")]
        public async Task<IActionResult> Get(int pZip)
        {
            Zipcode itmZip = await _context.Zipcodes
                .Where(x => x.Zip == pZip.ToString()).FirstOrDefaultAsync();
            return Ok(itmZip);
        }

        [HttpDelete]
        [Route("{KeyValue}")]
        Task<IActionResult> IBaseController<Zipcode>.Delete(int itemID)
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        [Route("Delete/{Zip}")]
        public async Task<IActionResult> Delete(int pZip)
        {
            Zipcode itmZip = await _context.Zipcodes
                .Where(x => x.Zip == pZip.ToString()).FirstOrDefaultAsync();
            _context.Remove(itmZip);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Zipcode _Zipcode)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _Zip = await _context.Zipcodes
                    .Where(x => x.Zip == _Zipcode.Zip).FirstOrDefaultAsync();

                if (_Zip == null)
                {
                    await Post(_Zipcode);
                    return Ok();
                }


                _Zip.Zip = _Zipcode.Zip;
                _Zip.City = _Zipcode.City;
                _Zip.State = _Zipcode.State;

                _context.Update(_Zip);
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
        public async Task<IActionResult> Post([FromBody] Zipcode _Zipcode)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _Zip = await _context.Zipcodes
                    .Where(x => x.Zip == _Zipcode.Zip).FirstOrDefaultAsync();

                if (_Zip != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Record Exists");
                }

                _Zip = new Zipcode();
                _Zip.Zip = _Zipcode.Zip;
                _Zip.City = _Zipcode.City;
                _Zip.State = _Zipcode.State;

                _context.Zipcodes.Add(_Zip);
                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Zip);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}