using InfraManager.Core.Data;
using InfraManager.Core.Exceptions;
using InfraManager.Core.Logging;
using InfraManager.Web.BLL;
using InfraManager.Web.BLL.Assets.SoftwareDistributionCentres;
using System;
using Microsoft.AspNetCore.Mvc;

namespace InfraManager.Web.Controllers.IM
{
    public partial class AssetApiController
    {
        #region method GetSoftwareDistributionCentres

        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("assetApi/SoftwareDistributionCentres", Name = "GetSoftwareDistributionCentres")]
        public ResultData<ManagedAccessObjectList<SoftwareDistributionCentre, SoftwareDistributionCentreDetails>> GetSoftwareDistributionCentres(
            Guid? softwareModelId = null)
        {
            var user = CurrentUser;

            if (user == null)
                return new ResultData<ManagedAccessObjectList<SoftwareDistributionCentre, SoftwareDistributionCentreDetails>>(RequestResponceType.NullParamsError, null);

            Logger.Trace(
                "AssetApiController.GetSoftwareDistributionCentres userID={0}, userName={1}",
                user.Id, 
                user.UserName);

            try
            {
                return ResultData<ManagedAccessObjectList<SoftwareDistributionCentre, SoftwareDistributionCentreDetails>>.Create(
                    RequestResponceType.Success, 
                    SoftwareDistributionCentres.GetList(user.User, softwareModelId));
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка при получения списка ЦРПО.");
                return ResultData<ManagedAccessObjectList<SoftwareDistributionCentre, SoftwareDistributionCentreDetails>>.Create(RequestResponceType.GlobalError);
            }
        }
        
        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("assetApi/SoftwareDistributionCentresByUser", Name = "SoftwareDistributionCentresByUser")]
        public ResultData<ManagedAccessObjectList<SoftwareDistributionCentre, SoftwareDistributionCentreDetails>> SoftwareDistributionCentresByUser()
        {
            var user = CurrentUser;

            if (user == null)
                return new ResultData<ManagedAccessObjectList<SoftwareDistributionCentre, SoftwareDistributionCentreDetails>>(RequestResponceType.NullParamsError, null);

            Logger.Trace(
                "AssetApiController.GetSoftwareDistributionCentres userID={0}, userName={1}",
                user.Id, 
                user.UserName);

            try
            {
                return ResultData<ManagedAccessObjectList<SoftwareDistributionCentre, SoftwareDistributionCentreDetails>>.Create(
                    RequestResponceType.Success, 
                    SoftwareDistributionCentres.GetList(user.User));
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка при получения списка ЦРПО.");
                return ResultData<ManagedAccessObjectList<SoftwareDistributionCentre, SoftwareDistributionCentreDetails>>.Create(RequestResponceType.GlobalError);
            }
        }

        #endregion

        #region method GetSoftwareDistributionCentreDetails

        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("assetApi/SoftwareDistributionCentres/{id}", Name = "GetSoftwareDistributionCentreDetails")]
        public ResultData<ManagedAccessObject<SoftwareDistributionCentreDetails>> GetSoftwareDistributionCentreDetails(Guid id)
        {
            var user = CurrentUser;

            if (user == null)
                return new ResultData<ManagedAccessObject<SoftwareDistributionCentreDetails>>(RequestResponceType.NullParamsError, null);

            Logger.Trace(
                "AssetApiController.GetSoftwareDistributionCentreDetails userID={0}, userName={1}, id={2}",
                user.Id,
                user.UserName,
                id);

            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    return ResultData<ManagedAccessObject<SoftwareDistributionCentreDetails>>.Create(
                        RequestResponceType.Success,
                        SoftwareDistributionCentres.Get(user.User, dataSource, id));
                }
            }
            catch(Exception error)
            {
                Logger.Error(error, $"Ошибка получения ЦРПО (ID={id})");
                return ResultData<ManagedAccessObject<SoftwareDistributionCentreDetails>>.Create(RequestResponceType.GlobalError);
            }
        }

        #endregion

        #region method Create

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/SoftwareDistributionCentres", Name = "PostSoftwareDistributionCentre")]
        public InsertResultData<Guid> PostSoftwareDistributionCentre(
            [FromForm]SoftwareDistributionCentreDetails model)
        {
            var user = CurrentUser;

            if (user == null || model == null)
                return InsertResultData<Guid>.Failure(RequestResponceType.NullParamsError);

            if (string.IsNullOrWhiteSpace(model.Name) 
                || model.ResponsiblePerson == null)
            {
                return InsertResultData<Guid>.Failure(RequestResponceType.BadParamsError);
            }

            Logger.Trace(
                "AssetApiController.PostSoftwareDistributionCentre userID={0}, userName={1}, name={2}, responsiblePersonID={3}, responsiblePersonClassID={4}",
                user.Id,
                user.UserName,
                model.Name,
                model.ResponsiblePerson.ID,
                model.ResponsiblePerson.ClassID);

            try
            {
                return InsertResultData<Guid>.Success(
                    SoftwareDistributionCentres.Create(user.User, model));                
            }
            catch(ArgumentValidationException)
            {
                return InsertResultData<Guid>.Failure(RequestResponceType.HasDuplicates);
            }
            catch(Exception error)
            {
                Logger.Error(error, $"Ошибка создания ЦРПО");
                return InsertResultData<Guid>.Failure(RequestResponceType.GlobalError);
            }
        }

        #endregion

        #region method Update

        [HttpPut]
        [AcceptVerbs("PUT")]
        [Route("assetApi/SoftwareDistributionCentres/{id}", Name = "PutSoftwareDistributionCentre")]
        public ResultData<SoftwareDistributionCentreDetails> PutSoftwareDistributionCentre(
            Guid id,
            [FromForm] SoftwareDistributionCentreDetails model)
        {
            var user = CurrentUser;

            if (user == null || model == null)
                return new ResultData<SoftwareDistributionCentreDetails>(RequestResponceType.NullParamsError, null);

            if (string.IsNullOrWhiteSpace(model.Name)
                || model.ResponsiblePerson == null)
            {
                return new ResultData<SoftwareDistributionCentreDetails>(RequestResponceType.BadParamsError, null);
            }

            Logger.Trace(
                "AssetApiController.PutSoftwareDistributionCentre userID={0}, userName={1}, id={2}",
                user.Id,
                user.UserName,
                id);

            try
            {
                SoftwareDistributionCentres.Update(user.User, id, model);
                return ResultData<SoftwareDistributionCentreDetails>.Create(RequestResponceType.Success, model);
            }
            catch(ArgumentValidationException)
            {
                return ResultData<SoftwareDistributionCentreDetails>.Create(RequestResponceType.HasDuplicates);
            }
            catch(ObjectConcurrencyException)
            {
                return ResultData<SoftwareDistributionCentreDetails>.Create(RequestResponceType.ConcurrencyError);
            }
            catch (Exception error)
            {
                Logger.Error(error, $"Ошибка обновления ЦРПО (ID={id})");
                return ResultData<SoftwareDistributionCentreDetails>.Create(RequestResponceType.GlobalError);
            }
        }

        #endregion

        #region method Delete

        [HttpDelete]
        [AcceptVerbs("DELETE")]
        [Route("assetApi/SoftwareDistributionCentres/{id}", Name = "DeleteSoftwareDistributionCentre")]
        public ResultData<SoftwareDistributionCentreDetails> DeleteSoftwareDistributionCentre(Guid id)
        {
            var user = CurrentUser;

            if (user == null)
                return new ResultData<SoftwareDistributionCentreDetails>(RequestResponceType.NullParamsError, null);

            Logger.Trace(
                "AssetApiController.DeleteSoftwareDistributionCentre userID={0}, userName={1}, id={2}",
                user.Id,
                user.UserName,
                id);

            try
            {
                SoftwareDistributionCentres.Delete(user.User, id);
                return ResultData<SoftwareDistributionCentreDetails>.Create(RequestResponceType.Success, null);
            }
            catch (Exception error)
            {
                Logger.Error(error, $"Ошибка удаления ЦРПО (ID={id})");
                return ResultData<SoftwareDistributionCentreDetails>.Create(RequestResponceType.GlobalError);
            }
        }

        #endregion

        #region Responsible Person Details

        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("assetApi/ResponsiblePerson/{classId:int}/{id}", Name = "GetResponsiblePersonDetails")]
        public void GetResponsiblePersonDetails(int classId, Guid id)
        {

        }

        #endregion
    }
}
