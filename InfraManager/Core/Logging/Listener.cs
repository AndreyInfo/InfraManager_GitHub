using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;
using InfraManager.ComponentModel;

namespace InfraManager.Core.Logging
{
	public abstract class Listener : IActivatable
	{
		#region classes
		private class _StringWriter : StringWriter
		{
			#region method Reset
			public void Reset(int defaultCapacity, int maxCapacity)
			{
				var stringBuilder = base.GetStringBuilder();
				stringBuilder.Length = 0;
				if (stringBuilder.Capacity > maxCapacity)
					stringBuilder.Capacity = defaultCapacity;
			}
			#endregion

			#region override protected method Dispose
			protected override void Dispose(bool disposing)
			{ }
			#endregion
		}
		#endregion


		#region fields
		private const int __defaultBufferCapacity = 256;
		private const int __maxBufferCapacity = 1024;
		private SeverityLevel _severityLevel;
		private object _lock = new object();
		private _StringWriter _stringWriter;
		private bool _recursiveGuard;
		#endregion


		#region properties
		[XmlAttribute("name")]
		public string Name { get; set; }

		[XmlAttribute("severityLevel")]
		public string SeverityLevel { get; set; }

		[XmlArray("filters")]
		[XmlArrayItem(typeof(Filters.DenyFilter))]
		[XmlArrayItem(typeof(Filters.LoggerNameMatchFilter))]
		[XmlArrayItem(typeof(Filters.MessageMatchFilter))]
		[XmlArrayItem(typeof(Filters.PropertyMatchFilter))]
		[XmlArrayItem(typeof(Filters.SeverityLevelMatchFilter))]
		[XmlArrayItem(typeof(Filters.SeverityLevelRangeFilter))]
		public Filter[] Filters { get; set; }

		[XmlElement(typeof(Formatters.BasicFormatter))]
		[XmlElement(typeof(Formatters.ExceptionFormatter))]
		[XmlElement(typeof(Formatters.MessageFormatter))]
		[XmlElement(typeof(Formatters.TemplatedFormatter))]
		public Formatter Formatter { get; set; }
		#endregion


		#region interface IActivatable
		[XmlIgnore]
		public bool IsActivated { get; protected set; }

		[XmlIgnore]
		public bool IsSuccessful { get; protected set; }

		public virtual void Activate()
		{
			_severityLevel = InfraManager.Core.Logging.SeverityLevel.GetSeverityLevel(this.SeverityLevel);
			if (this.Filters != null)
				foreach (var filter in this.Filters)
					filter.Activate();
			this.Formatter.Activate();
			this.IsActivated = true;
			this.IsSuccessful = true;
		}
		#endregion


		#region method RegisterLogEvent
		public void RegisterLogEvent(LogEvent logEvent)
		{
			#region assertions
			Debug.Assert(this.IsActivated, "Listener is not activated.");
			#endregion
			//
			if (!this.IsSuccessful)
				return;
			//
			lock (_lock)
			{
				if (_recursiveGuard)
					return;
				try
				{
					_recursiveGuard = true;
					//
					PreRegisterAction();
					//
					if (FiltersAreSatisfied(logEvent))
						WriteLogEvent(logEvent);
					//
					PostRegisterAction();
				}
				catch
				{ }
				finally
				{
					_recursiveGuard = false;
				}
			}
		}
		#endregion

		#region abstract method WriteLogEvent
		public abstract void WriteLogEvent(LogEvent logEvent);
		#endregion

		#region protected virtual method PreRegisterAction
		protected virtual void PreRegisterAction()
		{ }
		#endregion

		#region protected virtual method PostRegisterAction
		protected virtual void PostRegisterAction()
		{ }
		#endregion

		#region protected method RenderLogEvent
		protected string RenderLogEvent(LogEvent logEvent)
		{
			if (_stringWriter == null)
				_stringWriter = new _StringWriter();
			//
			_stringWriter.Reset(__defaultBufferCapacity, __maxBufferCapacity);
			RenderLogEvent(_stringWriter, logEvent);
			return _stringWriter.ToString();
		}

		protected void RenderLogEvent(TextWriter textWriter, LogEvent logEvent)
		{
			#region assertions
			Debug.Assert(this.Formatter != null, "Formatter is null.");
			Debug.Assert(logEvent != null, "logEvent is null.");
			#endregion
			//
			this.Formatter.Format(textWriter, logEvent);
		}
		#endregion

		#region private method FiltersAreSatisfied
		private bool FiltersAreSatisfied(LogEvent logEvent)
		{
			if (_severityLevel != null && _severityLevel <= logEvent.SeverityLevel)
				return false;
			foreach (var filter in this.Filters)
				switch (filter.Filtrate(logEvent))
				{
					case FilterResult.Accept:
						return true;
					case FilterResult.Reject:
						return false;
					case FilterResult.Ignore:
						continue;
				}
			return true;
		}
		#endregion
	}
}
