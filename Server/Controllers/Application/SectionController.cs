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

        public async Task<IActionResult> Get(int pSectionNo)
        {
            Section itmSection = await _context.Sections.Where(x => x.SectionNo == pSectionNo).FirstOrDefaultAsync();
            return Ok(itmSection);
        }

        public Task<IActionResult> Delete(int itemID)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> Post([FromBody] Section _Item)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> Put([FromBody] Section _Item)
        {
            throw new NotImplementedException();
        }
    }
}
