﻿using AutoMapper;
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

namespace SWARM.Server.Application.DvCode
{
    public class DeviceCodeController : BaseController<DeviceCode>, IBaseController<DeviceCode>
    {

        public DeviceCodeController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor)
            : base(context, httpContextAccessor)
        {

        }

        [HttpGet]
        [Route("GetDeciveCode")]
        public async Task<IActionResult> Get()
        {
            List<DeviceCode> itmDeviceCode = await _context.DeviceCodes.ToListAsync();
            return Ok(itmDeviceCode);
        }

        [HttpGet]
        [Route("GetDeviceCode/{pUserCode}")]
        public async Task<IActionResult> Get(int pUserCode)
        {
            DeviceCode itmDeviceCode = await _context.DeviceCodes.Where(x => x.UserCode == pUserCode.ToString()).FirstOrDefaultAsync();
            return Ok(itmDeviceCode);
        }

        [HttpDelete]
        [Route("Delete/{pUserCode}")]
        public async Task<IActionResult> Delete(int pUserCode)
        {
            DeviceCode itmDeviceCode = await _context.DeviceCodes.Where(x => x.UserCode == pUserCode.ToString()).FirstOrDefaultAsync();
            _context.Remove(itmDeviceCode);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] DeviceCode _DeviceCode)
        {
            bool bExists = false;

            var trans = _context.Database.BeginTransaction();
            try
            {
                var _DvCd = await _context.DeviceCodes.Where(x => x.UserCode == _DeviceCode.UserCode).FirstOrDefaultAsync();

                if (_DvCd == null)
                {
                    bExists = false;
                    _DvCd = new DeviceCode();
                }
                else
                    bExists = true;

                _DvCd.UserCode = _DeviceCode.UserCode;
                _DvCd.DeviceCode1 = _DeviceCode.DeviceCode1;
                _DvCd.SubjectId = _DeviceCode.SubjectId;
                _DvCd.SessionId = _DeviceCode.SessionId;
                _DvCd.ClientId = _DeviceCode.ClientId;
                _DvCd.Description = _DeviceCode.Description;
                _DvCd.CreationTime = _DeviceCode.CreationTime;
                _DvCd.Expiration = _DeviceCode.Expiration;
                _DvCd.Data = _DeviceCode.Data;
                _context.Update(_DeviceCode);
                await _context.SaveChangesAsync();

                if (bExists)
                {
                    _context.DeviceCodes.Update(_DeviceCode);
                }
                else
                    _context.DeviceCodes.Add(_DeviceCode);

                trans.Commit();

                return Ok(_DeviceCode.UserCode);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] DeviceCode _DeviceCode)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _DvCd = await _context.DeviceCodes.Where(x => x.UserCode == _DeviceCode.UserCode).FirstOrDefaultAsync();

                if (_DvCd != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Record Exists");
                }

                _DvCd = new DeviceCode();
                _DvCd.UserCode = _DeviceCode.UserCode;
                _DvCd.DeviceCode1 = _DeviceCode.DeviceCode1;
                _DvCd.SubjectId = _DeviceCode.SubjectId;
                _DvCd.SessionId = _DeviceCode.SessionId;
                _DvCd.ClientId = _DeviceCode.ClientId;
                _DvCd.Description = _DeviceCode.Description;
                _DvCd.CreationTime = _DeviceCode.CreationTime;
                _DvCd.Expiration = _DeviceCode.Expiration;
                _DvCd.Data = _DeviceCode.Data;
                _context.Update(_DeviceCode);
                await _context.SaveChangesAsync();
                _context.DeviceCodes.Add(_DvCd);

                trans.Commit();

                return Ok(_DeviceCode.UserCode);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }




    }
}