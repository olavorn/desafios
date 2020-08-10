using api.Models.ResultModel;
using api.Models.ServiceModel;

namespace api.Controllers
{
    public class AdvanceErrorJson : AdvanceJson
    {
        private string _errorMessage;

        public AdvanceErrorJson(string errorMessage)
        {
            this._errorMessage = errorMessage;
        }
    }
}