using System;
using Inframanager;

namespace InfraManager.DAL.ServiceDesk
{
	[ObjectClassMapping(ObjectClass.KBArticleFolder)]
	[OperationIdMapping(ObjectAction.Delete, OperationID.KBArticleFolder_Delete)]
	[OperationIdMapping(ObjectAction.Insert, OperationID.KBArticleFolder_Add)]
	[OperationIdMapping(ObjectAction.Update, OperationID.KBArticleFolder_Update)]
	[OperationIdMapping(ObjectAction.ViewDetails, OperationID.KBArticleFolder_Properties)]
	[OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.KBArticleFolder_Properties)]
	public class KBArticleFolder
	{
		public static int MaxNameLength = 250;
		public static int MaxNoteLength = 500;

		protected KBArticleFolder()
		{
			
		}
		
		public KBArticleFolder(string name, string note, Guid? parentID, bool visible)
		{
			ID = Guid.NewGuid();
			Name = name;
			Note = note ?? string.Empty;
			ParentID = parentID;
			Visible = visible;
		}

		public Guid ID { get; init; }

		public string Name { get; set; }

		public string Note { get; set; }

		public bool Visible { get; set; }

		public Guid? ParentID { get; set; }

		public int UpdatePeriod { get; set; }

		public int ExpertID { get; private set; }

		public byte[] RowVersion { get; init; }

		public virtual KBArticleFolder Parent { get; init; }

		private User _expert;
		public virtual User Expert
		{
			get => _expert;
			set
			{
				ExpertID = value?.ID ?? User.NullUserId;
				_expert = value;
			}
		}
	}
}
