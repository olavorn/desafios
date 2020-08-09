using api.Models.ResultModel;
using api.Models.ServiceModel;

namespace api.Controllers
{
    public class AdvanceErrorJson : AdvanceJson
    {
        private AdvanceProcessing _advanceProcessing;

        public AdvanceErrorJson(AdvanceProcessing advanceProcessing)
        {
            this._advanceProcessing = advanceProcessing;
        }
    }
}