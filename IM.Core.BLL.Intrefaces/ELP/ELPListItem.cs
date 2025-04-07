using System;
using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.ResourcesArea;

namespace InfraManager.BLL.ELP
{
    [ListViewItem(ListView.ELPList)]
    public sealed class ELPListItem
    {
        /// <summary>
        /// Идентификатор связи
        /// </summary>
        public Guid ID { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        [ColumnSettings(1)]
        [Label(nameof(Resources.Name))]
        public string Name { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        [ColumnSettings(2)]
        [Label(nameof(Resources.Description))]
        public string Description { get; set; }

        /// <summary>
        /// Производитель
        /// </summary>
        public string VendorName { get; set; }

        /// <summary>
        /// Производитель, ID
        /// </summary>
        public Guid? VendorID { get; set; }

        /// <summary>
        /// Описание расписания
        /// </summary>
        [ColumnSettings(3)]
        [Label(nameof(Resources.ELPListScheduleLabel))]
        public string ScheduleLabel { get; set; }

        /// <summary>
        /// Описание состояния
        /// </summary>
        [ColumnSettings(4)]
        [Label(nameof(Resources.ELPListState))]
        public string StateLabel { get; set; }

        /// <summary>
        /// Следующий запуск
        /// </summary>
        [ColumnSettings(5)]
        [Label(nameof(Resources.NextRunAt))]
        public DateTime? NextRun { get; set; }

        /// <summary>
        /// Последний запуск
        /// </summary>
        [ColumnSettings(6)]
        [Label(nameof(Resources.FinishRunAt))]
        public DateTime? LastRun { get; set; }
    }
}