using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Extensions;
using api.Model;
using api.Models.EntityModel;
using api.Models.IntegrationModel;
using api.Models.ResultModel;
using api.Models.ServiceModel;
using api.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace api.Controllers
{
    [Route("api/v2/admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly apiContext _context;
        private readonly IAcquirerApi _acquirerApi;
        private readonly IAccountApi _accountApi;
        private readonly ILogger _log;

        public AdminController(apiContext context, IAcquirerApi acquirerApi, IAccountApi accountApi, ILogger log)
        {
            _context = context;
            _acquirerApi = acquirerApi;
            _accountApi = accountApi;
            _log = log;
        }

        [HttpPost, Route("login")]
        public async Task<dynamic> Login([FromQuery]AccountLoginModel model)
        {
            var wai = await _context.Users.FirstOrDefaultAsync(q=> q.Email == model.Login);

            return new { Token = wai.MapToWhoAdminAmI().EncryptToken() };
        }

        [HttpGet, Route("list-advance-requests")]
        public async Task<AdvanceListJson> ListAdvanceRequests([FromQuery]AdvanceListModel model)
        {
            var wai = await _accountApi.WhoAdminAmI(model.AuthToken);
            var advances = _context.Advances.Include(q => q.Payments).Where(q => q.EvaluationDateStart == null || q.EvaluationBy == wai.AdminId).ToList();

            return new AdvanceListJson(advances, advances.Count());
        }

        [HttpGet, Route("advance-details")]
        public async Task<AdvanceJson> GetAdvanceDetails([FromQuery] AdvanceDetailsModel model)
        {
            var wai = await _accountApi.WhoAmI(model.AuthToken);
            var advance = _context.Advances.Include(q => q.Payments).SingleOrDefault(q => q.CustomerId == wai.CustomerId && q.Id == model.Id);

            return new AdvanceJson(advance);
        }

        [HttpPost, Route("advance-begin-evaluation")]
        public async Task<AdvanceJson> BeginAdvanceEvaluation([FromQuery]AdvanceEvaluationModel model)
        {
            var advProcessing = new AdvanceProcessing(_context, _acquirerApi, _accountApi, _log);
            var advance = model.Map();

            try
            {
                advance = await advProcessing.BeginEvaluation(advance, model.AuthToken);
            }
            catch (Exception ex)
            {
                return new AdvanceErrorJson(ex.Message);
            }

            await _context.SaveChangesAsync();
            return new AdvanceJson(advance);
        }

        [HttpPost, Route("advance-end-evaluation")]
        public async Task<AdvanceJson> EndAdvanceEvaluation([FromQuery]AdvanceEvaluationModel model)
        {
            var advProcessing = new AdvanceProcessing(_context, _acquirerApi, _accountApi, _log);
            var advance = model.Map();

            try
            {
                advance = await advProcessing.EndEvaluation(advance, model.IsApproved, model.AuthToken);
            }
            catch (Exception ex)
            {
                return new AdvanceErrorJson(ex.Message);
            }

            await _context.SaveChangesAsync();
            return new AdvanceJson(advance);
        }

    }
}
