using System;
using System.Collections.Generic;

namespace IM.Core.WF.BLL.Interfaces.Models
{
    public class WorkflowSchemeModel
    {
		public Guid ID { get; set; } // +

		public Guid? WorkflowSchemeFolderID { get; set; } // +

		public string Name { get; set; } // +

		public string Note { get; set; } // +

		public Guid? ParentID { get; set; }

		public ushort MajorVersion { get; set; } // +

		public ushort MinorVersion { get; set; } // +

		public byte Status { get; set; } //Created, Published, Overriden, Blocked // +

		public DateTime UtcDateModified { get; set; } // +

		public Guid ModifierID { get; set; } // +

		public DateTime? UtcDatePublished { get; set; } // +

		public Guid? PublisherID { get; set; } // +

		public int ObjectClassID { get; set; } //immutable // +

		public string Identifier { get; set; } //immutable // +
		
		public string VisualScheme { get; set; }
		
		public string LogicalScheme { get; set; }

		public bool TraceIsEnabled { get; set; } // +

		public Dictionary<int, string> StringTypes = new() //TODO переделать, временный вариант 
		{
			{ 119, "Задание" },
			{ 701, "Заявка" },
			{ 702, "Проблема" },
			{ 703, "Запрос на изменения" },
			{ 736, "Сообщение подсистемы мониторинга" },
			{ 737, "Сообщение подсистемы опроса оборудования" },
			{ 739, "Сводное сообщение опроса компьютеров" },
			{ 735, "Сообщение подсистемы электронной почты" },
			{ 741, "Сообщение подсистемы импорта оргструктуры" },
			{ 742, "Сводное сообщение импорта пользователей" },
			{ 823, "Массовый инцидент" }
		};
    }
}
