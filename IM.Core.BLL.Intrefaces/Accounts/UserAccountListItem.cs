using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.BLL.Settings.TableFilters.Attributes;
using InfraManager.BLL.Settings.UserFields;
using InfraManager.DAL;
using InfraManager.DAL.Accounts;
using InfraManager.ResourcesArea;
using System;
using System.Collections.Generic;

namespace InfraManager.BLL.Accounts
{
    [ListViewItem(ListView.UserAccounts)]
    public class UserAccountListItem
    {
        public ObjectClass ClassID => ObjectClass.UserAccount;

        public int ID { get; init; }

        //Тип - выбор из перечня: Общего назначения, Приложение, Windows, VMware, SSH, CIM, SNMP v2, SNMP v3
        [ColumnSettings(1)]
        [Label(nameof(Resources.Type))]
        public string TypeText { get; init; }

        //Название - Строка длиной не более 50 символов
        [ColumnSettings(2)]
        [Label(nameof(Resources.Name))]
        public string Name { get; init; }
    }
}
