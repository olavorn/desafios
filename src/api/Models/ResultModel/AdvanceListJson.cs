using api.Model;
using api.Models.EntityModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models.ResultModel
{
    public class AdvanceListJson : IActionResult
    {
        public AdvanceListJson() { }

        public AdvanceListJson(IEnumerable<Advance> advances, long count)
        {
            Advances = advances.Select(advance => new AdvanceJson(advance)).ToList();
            Count = count;
        }

        public IEnumerable<AdvanceJson> Advances { get; set; }
        public long Count { get; set; }

        public Task ExecuteResultAsync(ActionContext context)
        {
            return new JsonResult(this).ExecuteResultAsync(context);
        }
    }
}
