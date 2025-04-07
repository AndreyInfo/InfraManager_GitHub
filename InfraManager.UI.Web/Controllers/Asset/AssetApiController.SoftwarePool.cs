using System;
using Microsoft.AspNetCore.Mvc;
using InfraManager.Core.Logging;
using InfraManager.IM.BusinessLayer.Asset.History;
using InfraManager.Web.BLL.Software;
using InfraManager.Web.DTL.Software;

namespace InfraManager.Web.Controllers.IM
{
    public partial class AssetApiController
    {
        public class SoftwarePoolBalanceResult
        {
            public int? Balance { get; set; }
        }
        
        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("assetApi/GetSoftwarePoolBalance/{manufacturerID}/{sdcId}/{softwareModelId}/{softwareTypeID}/{licenceScheme}/{licenceType}/{operationType}")]
        public ResultData<SoftwarePoolBalanceResult> GetSoftwarePoolBalance(
            Guid? manufacturerID, 
            Guid sdcId, 
            Guid softwareModelId,
            Guid softwareTypeID,
            LicenceScheme licenceScheme, 
            LicenceType licenceType,
            OperationType operationType)
        {
            if (sdcId == Guid.Empty
                || softwareModelId == Guid.Empty)
            {
                return new ResultData<SoftwarePoolBalanceResult>(RequestResponceType.BadParamsError, null);
            }

            var user = CurrentUser;
            if (user == null)
                return new ResultData<SoftwarePoolBalanceResult>(RequestResponceType.NullParamsError, null);
            try
            {
                var info = new LicenceSoftwarePoolInfo(manufacturerID, 
                        sdcId, 
                        softwareModelId, 
                        softwareTypeID,
                        licenceScheme, 
                        licenceType,
                        operationType)
                    .GetBalance();

                return new ResultData<SoftwarePoolBalanceResult>(RequestResponceType.Success, new SoftwarePoolBalanceResult() {Balance = info});
            }
            catch(Exception error)
            {
                Logger.Error(error);

                return new ResultData<SoftwarePoolBalanceResult>(
                    RequestResponceType.GlobalError,
                    null);
            }
        }

    }
}