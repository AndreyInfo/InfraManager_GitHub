using System;
using System.Collections.Generic;

namespace InfraManager.DAL.Import.CSV
{
    public class UICSVIMFieldConcordance : UIIMFieldConcordance
    {
        public UICSVIMFieldConcordance(Guid cSVConfigurationID, long iMFieldID, string expression)
        {
            CSVConfigurationID = cSVConfigurationID;
            IMFieldID = iMFieldID;
            Expression = expression;
        }

        public Guid CSVConfigurationID { get; init; }
        public virtual UICSVConfiguration Configuration { get; set; }
    }
}
