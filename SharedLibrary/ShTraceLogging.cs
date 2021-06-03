#region using

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Security.AccessControl;
using System.Security.Policy;
using System.Text;
// using System.Text.Json;
using System.Xml.Serialization;

#endregion

// username: jeffs
// created:  5/8/2021 3:23:01 PM

namespace SharedLibrary
{
	public enum TraceLoggingStatus
	{
		INVALID = -1,
		CREATED = 0,
		CONFIGURED = 1,
		ACTIVATED = 2,
		RUNNING = 3
	}

/*
procedure
use the various write routines to set messages + message level throughout code

1. on startup - nothing - status == invalid
2. create instance - status == created
2a. make instance at code start so that all trace write routines will work
3. activate - provide file path and app name - status == configured 
3a. initially, status == configured
3b. trace file is setup / configured - if good - status == activated
3c. trace file is initially set to receive all messages but listener is off
4. start + level - status == running
5. get an instance if allowed (depends on status)

*/
	public class ShTraceLogging : INotifyPropertyChanged
	{
		private const string TRACE_FILE_PREFIX = "Trace Log";
		// private const string TRACE_LISTENER_NAME = "ShTraceListener01";

		private static readonly Lazy<ShTraceLogging> instance =
			new Lazy<ShTraceLogging>(() => new ShTraceLogging());

		private string traceSwitchName;

		private TraceSource traceSrc;

		private SourceLevels traceLevel = SourceLevels.Information;

		// private TextWriterTraceListener textListener;
		private TraceTextWriterListener textListener;

		private int traceFileCount = 0;

		private string appName;
		private string folderPath;
		private string filePath;
		private string filename;

		private bool traceActivated;

		private TraceLoggingStatus traceStatus = TraceLoggingStatus.INVALID;

	#region private fields

	#endregion

	#region ctor

		private ShTraceLogging()
		{
			TraceStatus = TraceLoggingStatus.CREATED;
		}

	#endregion

	#region public properties

		public static ShTraceLogging Instance => instance.Value;

		public SourceLevels TraceLevel => traceLevel;

		public string TraceFileName
		{
			get => filename;
			private set
			{
				filename = value;
				OnPropertyChanged();
			}
		}

		public string TraceFolderPath
		{
			get => folderPath;
			set
			{
				folderPath = value;
				OnPropertyChanged();

				CountPastTraceFiles();
			}
		}

		public string TraceFilePath
		{
			get => filePath;
			private set
			{
				filePath = value;
				OnPropertyChanged();
			}
		}

		public bool TraceActivated
		{
			get => traceActivated;
			private set
			{
				if (traceActivated == value) return;

				traceActivated = value;
				OnPropertyChanged();
			}
		}

		public TraceLoggingStatus TraceStatus
		{
			get => traceStatus;
			set
			{
				if (traceStatus == value) return;

				traceStatus = value;

				OnPropertyChanged();

				if (traceStatus >= TraceLoggingStatus.ACTIVATED)
				{
					TraceActivated = true;
				}
				else
				{
					TraceActivated = false;
				}
			}
		}

		public string TraceFileCountString
		{
			get
			{
				if (traceFileCount > 0) return $"{traceFileCount:D}";

				return "None";
			}
		}

		public int TraceFileCount
		{
			get => traceFileCount;
			set
			{
				traceFileCount = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(TraceFileCountString));
			}
		}

		public bool TraceFileExists => (File.Exists(filePath));

	#endregion

	#region private properties

	#endregion

	#region public methods

		public void UpdateProperties()
		{
			OnPropertyChanged("TraceFileExists");
		}

		public void Activate(string appname)
		{
			if (folderPath == null) return;

			appName = appname;
			// TraceFolderPath = folderpath;
			TraceFileName = $"{TRACE_FILE_PREFIX} - {appname} - ({DateTime.Now:yyyy-MM-dd hh_mm tt}).log";
			TraceFilePath = $"{TraceFolderPath}\\{TraceFileName}";

			CountPastTraceFiles();

			traceSwitchName = appName + "Switch";

			AssignTraceSource();
			
			TraceStatus = TraceLoggingStatus.CONFIGURED;

			if (!ActivateTraceFile()) return;

			TraceStatus = TraceLoggingStatus.ACTIVATED;


		}

		public List<string> PastTraceFiles(string folderPath)
		{
			if (folderPath == null) return null;

			List<string> files = new List<string>();

			foreach (string file in Directory.EnumerateFiles(folderPath, $"{TRACE_FILE_PREFIX}*"))
			{
				files.Add(file);
			}

			return files;
		}

		public string LastTraceFile(List<string> pastFiles)
		{
			if (pastFiles == null || pastFiles.Count == 0) return null;

			DateTime prior = DateTime.MinValue;
			DateTime dateTime;

			string result = null;

			foreach (string file in pastFiles)
			{
				string pastFile = Path.GetFileName(file);

				int pos1 = pastFile.IndexOf('(');
				if (pos1 < 0) continue;

				int pos2 = pastFile.IndexOf(')', pos1);
				if (pos2 < 0) continue;

				string date = pastFile.Substring(pos1 + 1, pos2 - pos1 - 1);

				date = date.Replace('_', ':');

				bool ok = DateTime.TryParse(date, out dateTime);

				if (!ok) continue;

				if (dateTime.CompareTo(prior) >= 0) result = file;
			}

			return result;
		}

		public List<string> DeletePastTraceFiles(List<string> pastFiles)
		{
			List<string> fails = new List<string>();

			foreach (string traceFile in pastFiles)
			{
				if (File.Exists(traceFile))
				{
					int count = 0;
					bool result = true;

					do
					{
						count++;
						try
						{
							File.Delete(traceFile);
						}
						catch
						{
							Trace.Close();
							if (count < 11) continue;

							fails.Add(traceFile);
						}

						result = false;
					}
					while (result);

				}
			}

			return fails;
		}

		public void Close()
		{
			if (TraceStatus != TraceLoggingStatus.RUNNING
				&& TraceStatus != TraceLoggingStatus.ACTIVATED
				) return;

			TraceStatus = TraceLoggingStatus.CREATED;

			traceSrc.Listeners[appName].Close();
			traceSrc.Listeners[appName].Dispose();
			traceSrc.Listeners.Remove(appName);
			// Trace.Flush();
			// Trace.Close();
			// AssignTraceSource();

			UpdateProperties();
		}

		public void Reset()
		{
			Trace.Close();

			if (traceStatus < TraceLoggingStatus.CONFIGURED) return;

			if (!ActivateTraceFile()) return;
		}

		public void Start(SourceLevels level)
		{
			if (TraceStatus != TraceLoggingStatus.ACTIVATED) return;

			// if (traceStatus < TraceLoggingStatus.ACTIVATED) return;

			traceLevel = level;

			textListener.Filter = new EventTypeFilter(traceLevel);

			TraceStatus = TraceLoggingStatus.RUNNING;

			textListener.WriteLine($"Trace Started| {DateTime.Now:F}");
			textListener.WriteLine($"Program| {appName}");
		}

		public void Stop()
		{
			CountPastTraceFiles();

			if (TraceStatus < TraceLoggingStatus.ACTIVATED) return;

			TraceStatus = TraceLoggingStatus.ACTIVATED;

			textListener.Filter = new EventTypeFilter(SourceLevels.Off);
		}

		public void TabUp(string location = null, string position = null, string msg = null)
		{
			if (!TraceActivated) return;

			// Trace.Indent();

			IndentMessage(location, position, msg);

			textListener.IndentLevel += 1;
		}

		public void TabDn(string location = null, string position = null, string msg = null)
		{
			if (!TraceActivated) return;

			// Trace.Unindent();

			textListener.IndentLevel -= 1;

			IndentMessage(location, position, msg);
		}

		public void TabReset(string location = null, string position = null, string msg = null)
		{
			textListener.IndentReset();

			IndentMessage(location, position, msg);
		}

		// public void WriteLine2(TraceEventType type, int id,
		// 	string location, string position, string msg)
		// {
		// 	if (!TraceActivated) return;
		// 	write(type, id, formatTraceString(location, position, msg));
		// }

		public void TraceEvent(TraceEventType type, int id,
			string location, string position, string msg)
		{
			if (!TraceActivated) return;

			traceSrc.TraceEvent(type, id, msg, location, position);
		}

		public void TraceEvent(TraceEventType type, int id, string msg)
		{
			if (!TraceActivated) return;

			traceSrc.TraceEvent(type, id, msg);
		}

		// public void TraceEvent(TraceEventType type, int id, 
		// 	string location, string position, string msg,
		// 	object obj)
		// {
		// 	JsonSerializerOptions o = new JsonSerializerOptions();
		// 	o.WriteIndented = true;
		// 	o.IgnoreNullValues = false;
		// 	
		// 	string jsonStr = JsonSerializer.Serialize(obj, o);
		//
		// 	XmlSerializer x = new XmlSerializer(obj.GetType());
		//
		// 	string xmlStr;
		//
		// 	using (StringWriter sw = new StringWriter())
		// 	{
		// 		x.Serialize(sw, obj);
		// 		xmlStr = sw.ToString();
		// 	}
		//
		// 	TraceEvent(type, id, location, position, msg);
		// 	textListener.WriteLine(jsonStr);
		//
		// 	TraceEvent(type, id, location, position, msg);
		// 	textListener.WriteLine(xmlStr);
		//
		// }

	#endregion

	#region private methods

		private void CountPastTraceFiles()
		{
			if (folderPath == null) return;

			TraceFileCount = PastTraceFiles(folderPath)?.Count ?? 0;
		}

		private void AssignTraceSource()
		{
			traceSrc = new TraceSource(appName);
		}

		private void IndentMessage(string location, string position, string msg)
		{
			if (msg != null)
				TraceEvent(TraceEventType.Information,
					0, location, position, msg);
		}

		private bool ActivateTraceFile()
		{
			if (TraceActivated) return false;

			traceSrc.Switch = new SourceSwitch(traceSwitchName, "All");
			traceSrc.Listeners.Remove("Default");

			textListener = new TraceTextWriterListener(TraceFilePath, appName);
			textListener.Filter = new EventTypeFilter(SourceLevels.Off);
			textListener.SourceName = "Trace Test";
			textListener.FormatStringHdr = "{0,-30}| {1,-5:D}";
			textListener.FormatStringMsg = "| {0,-16} | {1,-20} | {2}";

			traceSrc.Listeners.Add(textListener);

			Trace.AutoFlush = true;
			textListener.IndentSize = 2;

			return true;
		}

	#endregion

	#region event consuming

	#endregion

	#region event publishing

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "TraceLogging| FileName| " + (filename ?? "is null")
				+ " filePath| " + (filePath ?? "is null");
		}

	#endregion
	}

	class TraceTextWriterListener : TextWriterTraceListener
	{
		private string ident = "                    "; // 20 spaces

		private string sourceName;
		// private int indentSize = 0;

		public TraceTextWriterListener(string fileName, string name) : base(fileName, name)
		{
			SourceName = GetType().Assembly.GetName().Name;
		}

		public string SourceName
		{
			get => sourceName;
			set { sourceName = value; }
		}

		public string FormatStringHdr { get; set; } = "{0}: {1} : ";
		public string FormatStringMsg { get; set; } = "{0} {1} {2}";

		public void IndentReset()
		{
			IndentLevel = 0;
		}

		public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message, params object[] args)
		{
			if (this.Filter != null && !this.Filter.ShouldTrace(eventCache, sourceName, eventType, id, message, null, null, null))
				return;

			this.WriteHeader(sourceName, eventType, id);

			object[] strings = FormatArgs(message, args);

			string result = FormatMsg(strings);

			base.WriteLine(result);
		}

		private object[] FormatArgs(string msg, object[] args)
		{
			int i;

			object[] strings = new object[args.Length + 1];

			for (i = 0; i < strings.Length - 1; i++)
			{
				strings[i] = args[i];
			}

			strings[i] = Indent() + msg;

			return strings;
		}

		private string FormatMsg(params object[] args)
		{
			return string.Format(FormatStringMsg, args);
		}

		private string Indent()
		{
			if (IndentLevel == 0 || IndentSize == 0) return "";

			int indentsize = IndentSize <= 20 ? IndentSize : 20;

			string indent = ident.Substring(0, indentsize);

			if (IndentLevel == 1) return indent;

			StringBuilder sb = new StringBuilder();

			for (int i = 0; i < IndentLevel; i++)
			{
				sb.Append(indent);
			}

			return sb.ToString();
		}

		private void WriteHeader(string source, TraceEventType eventType, int id)
		{
			NeedIndent = false;
			string result = $"{source} {eventType.ToString()}";

			base.Write(string.Format((IFormatProvider) CultureInfo.InvariantCulture, FormatStringHdr,
				new object[2]
				{
					(object) result,
					(object) id.ToString((IFormatProvider) CultureInfo.InvariantCulture)
				}));
		}
	}
}