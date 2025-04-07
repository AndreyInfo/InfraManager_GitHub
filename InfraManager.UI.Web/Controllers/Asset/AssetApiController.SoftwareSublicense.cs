using InfraManager.Core.Logging;
using InfraManager.Web.BLL.Software;
using System;
using System.Data;
using Microsoft.AspNetCore.Mvc;

namespace InfraManager.Web.Controllers.IM
{
    public partial class AssetApiController
    {
        #region method GetSoftwareSublicenseDetails

        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("assetApi/SoftwareSublicenses/{id}", Name = "GetSoftwareSublicenseDetails")]
        public ResultData<SoftwareSublicenseDetails> GetSoftwareSublicenseDetails(Guid id)
        {
            var user = CurrentUser;

            if (user == null)
                return new ResultData<SoftwareSublicenseDetails>(RequestResponceType.NullParamsError, null);

            Logger.Trace(
                "AssetApiController.GetSoftwareSublicenseDetails userID={0}, userName={1}, id={2}",
                user.Id,
                user.UserName,
                id);

            try
            {
                var details = SoftwareSublicenseDetails.Get(id);

                return ResultData<SoftwareSublicenseDetails>.Create(
                    RequestResponceType.Success,
                    details);
            }
            catch(Exception error)
            {
                Logger.Error(error, $"Ошибка получения сублицензии (ID={id})");
                return ResultData<SoftwareSublicenseDetails>.Create(RequestResponceType.GlobalError);
            }
        }

        #endregion

        #region Transfer to SDC

        public class SoftwareSublicenseTransferData
        {
            public Guid SoftwareDistributionCentreID { get; set; }
            public int? Quantity { get; set; }
            public string RowVersion { get; set; }
        }

        public class SoftwareSublicenceTransferResult
        {
            public bool Removed { get; set; }
        }

        [HttpPut]
        [AcceptVerbs("PUT")]
        [Route("assetApi/SoftwareSublicenses/{id}/transfer", Name = "SoftwareSublicenseTransfer")]
        public ResultData<SoftwareSublicenceTransferResult> SoftwareSublicenseTransfer(Guid id, [FromForm]SoftwareSublicenseTransferData data)
        {
            if (data.SoftwareDistributionCentreID == Guid.Empty || string.IsNullOrWhiteSpace(data.RowVersion))
            {
                return new ResultData<SoftwareSublicenceTransferResult>(RequestResponceType.BadParamsError, null);
            }

            var user = CurrentUser;

            if (user == null)
                return new ResultData<SoftwareSublicenceTransferResult>(RequestResponceType.NullParamsError, null);

            Logger.Trace(
                "AssetApiController.SoftwareSublicenseTransfer userID={0}, userName={1}, id={2}, sdc={3}, qty={4}",
                user.Id,
                user.UserName,
                id,
                data.SoftwareDistributionCentreID,
                data.Quantity);

            try
            {
                var isRemoved = SoftwareSublicenseDetails.TransferTo(
                    id, 
                    data.SoftwareDistributionCentreID, 
                    data.Quantity, 
                    data.RowVersion);

                return new ResultData<SoftwareSublicenceTransferResult>(
                    RequestResponceType.Success, 
                    new SoftwareSublicenceTransferResult { Removed = isRemoved });
            }
            catch(DBConcurrencyException)
            {
                return new ResultData<SoftwareSublicenceTransferResult>(
                    RequestResponceType.ConcurrencyError,
                    null);
            }
            catch(Exception error)
            {
                Logger.Error(error);

                return new ResultData<SoftwareSublicenceTransferResult>(
                    RequestResponceType.GlobalError,
                    null);
            }
        }

        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("assetApi/SoftwareSublicencePool/{sdcId}/{softwareModelId}/{licenceScheme}/{licenceType}")]
        public ResultData<SoftwareSublicencePoolInfo> GetSoftwareSublicencePool(Guid sdcId, Guid softwareModelId, LicenceScheme licenceScheme, LicenceType licenceType)
        {
            if (sdcId == Guid.Empty
                || softwareModelId == Guid.Empty)
            {
                return new ResultData<SoftwareSublicencePoolInfo>(RequestResponceType.BadParamsError, null);
            }

            var user = CurrentUser;
            if (user == null)
                return new ResultData<SoftwareSublicencePoolInfo>(RequestResponceType.NullParamsError, null);

            try
            {
                var info = new SoftwareSublicencePool(sdcId, softwareModelId, licenceScheme, licenceType)
                    .GetInfo(user.User.ID);

                return new ResultData<SoftwareSublicencePoolInfo>(RequestResponceType.Success, info);
            }
            catch(Exception error)
            {
                Logger.Error(error);

                return new ResultData<SoftwareSublicencePoolInfo>(
                    RequestResponceType.GlobalError,
                    null);
            }
        }

        [HttpPut]
        [AcceptVerbs("PUT")]
        [Route("assetApi/SoftwareSublicencePool/{sdcId}/{softwareModelId}/{licenceScheme}/{licenceType}/tranfer")]
        public ResultData<SoftwareSublicenceTransferResult> SoftwareSublicencePoolTransfer(
            Guid sdcId,
            Guid softwareModelId,
            LicenceScheme licenceScheme,
            LicenceType licenceType,
            [FromForm] SoftwareSublicenseTransferData data)
        {
            if (sdcId == Guid.Empty 
                || softwareModelId == Guid.Empty
                || data.SoftwareDistributionCentreID == Guid.Empty)
            {
                return new ResultData<SoftwareSublicenceTransferResult>(RequestResponceType.BadParamsError, null);
            }

            var user = CurrentUser;

            if (user == null)
                return new ResultData<SoftwareSublicenceTransferResult>(RequestResponceType.NullParamsError, null);

            Logger.Trace(
                "AssetApiController.SoftwareSublicenseTransfer userID={0}, userName={1}, sdc={2}, model={3}, scheme={4}, type={5}, targetSDC={6}, qty={7}",
                user.Id,
                user.UserName,
                sdcId,
                softwareModelId,
                licenceScheme,
                licenceType,
                data.SoftwareDistributionCentreID,
                data.Quantity);

            try
            {
                new SoftwareSublicencePool(sdcId, softwareModelId, licenceScheme, licenceType)
                    .Transfer(user.User.ID, data.SoftwareDistributionCentreID, data.Quantity);

                return new ResultData<SoftwareSublicenceTransferResult>(
                    RequestResponceType.Success,
                    new SoftwareSublicenceTransferResult { Removed = false });
            }
            catch (Exception error)
            {
                Logger.Error(error);

                return new ResultData<SoftwareSublicenceTransferResult>(
                    RequestResponceType.GlobalError,
                    null);
            }
        }

        #endregion
    }
}
