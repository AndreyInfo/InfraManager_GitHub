using InfraManager.Core.Logging;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.CrossPlatform.WebApi.Contracts.Common.Models;
using InfraManager.CrossPlatform.WebApi.Contracts.SoftwareLicenceScheme.Models;
using InfraManager.UI.Web;
using InfraManager.Web.BLL.Tables;
using InfraManager.Web.Controllers.Models.LicenceSchemes;
using InfraManager.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.Web.Controllers.LicenceSchemes
{
    public class SoftwareLicenceSchemeForTable : BaseForTable
    {
        private SoftwareLicenceSchemeListItem _schema;

        public SoftwareLicenceSchemeForTable(SoftwareLicenceSchemeListItem schema)
        {
            this._schema = schema;
        }

        public override Guid ID => _schema.ID;
        public string Name => _schema.Name;
        public string LicensingObjectTypeLabel => _schema.LicensingObjectTypeLabel;
        public string SchemeTypeLabel => _schema.SchemeTypeLabel;
        public bool IsLocationRestrictionApplicable => _schema.IsLocationRestrictionApplicable;

        public string CreatednDate => _schema.CreatedDate.ToString("dd.MM.yyyy");
        public string UpdatedDate => _schema.UpdatedDate.ToString("dd.MM.yyyy");
    }

    public class ProcessorModelForTable : BaseForTable
    {
        private CrossPlatform.WebApi.Contracts.Assets.ProcessorModelModel _item;

        public ProcessorModelForTable(CrossPlatform.WebApi.Contracts.Assets.ProcessorModelModel item)
        {
            _item = item;
        }

        public override Guid ID => _item.ID;
        public string Name => _item.ModelName;
        public String TypeName => _item.TypeName;
        public String ManufacturerName => _item.ManufactorName;
        public string Cores => _item.Cores;
        public int CoefficientValue => 1;

    }

    public class ExpressionValidationResult
    {
        public ExpressionValidationResult(ExpressionValidationResponse response)
        {
            IsSuccess = response.IsSuccess;

            if (!IsSuccess)
            {
                ErrorText = string.Format(
                    ResourcesArea.ResourceStringHelper.GetString($@"@Resources.{response.MessageKey}@"),
                    response.MessageArguments);
            }
        }

        public bool IsSuccess { get; set; }
        public string ErrorText { get; set; }
    }
}

