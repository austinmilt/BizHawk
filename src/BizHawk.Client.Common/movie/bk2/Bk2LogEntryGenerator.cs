﻿using System.Collections.Generic;
using System.Linq;
using System.Text;

using BizHawk.Emulation.Common;

namespace BizHawk.Client.Common
{
	// Designed to be able to last the lifetime of an IMovie
	internal class Bk2LogEntryGenerator : ILogEntryGenerator
	{
		private readonly string _systemId;
		private readonly IController _source;

		private readonly Dictionary<string, char> _mnemonics = new();
		private readonly List<IReadOnlyList<string>> _controlsOrdered;

		public Bk2LogEntryGenerator(string systemId, IController source)
		{
			_systemId = systemId;
			_source = source;
			_controlsOrdered = _source.Definition.ControlsOrdered.Where(static c => c.Count is not 0).ToList();
			foreach (var group in _controlsOrdered)
			{
				foreach (var button in group)
				{
					_mnemonics.Add(button, Bk2MnemonicLookup.Lookup(button, _systemId));
				}
			}
		}

		public bool IsEmpty => EmptyEntry == GenerateLogEntry();

		public string EmptyEntry => CreateLogEntry(createEmpty: true);

		public string GenerateLogEntry() => CreateLogEntry();

		public string GenerateLogKey()
		{
			var sb = new StringBuilder();
			sb.Append("LogKey:");

			foreach (var group in _source.Definition.ControlsOrdered.Where(static c => c.Count is not 0))
			{
				sb.Append('#');
				foreach (var button in group)
				{
					sb.Append(button).Append('|');
				}
			}

			return sb.ToString();
		}

		public IDictionary<string, string> Map()
		{
			var dict = new Dictionary<string, string>();
			foreach (var button in _source.Definition.OrderedControlsFlat)
			{
				if (_source.Definition.BoolButtons.Contains(button))
				{
					dict.Add(button, Bk2MnemonicLookup.Lookup(button, _systemId).ToString());
				}
				else if (_source.Definition.Axes.ContainsKey(button))
				{
					dict.Add(button, Bk2MnemonicLookup.LookupAxis(button, _systemId));
				}
			}

			return dict;
		}

		private string CreateLogEntry(bool createEmpty = false)
		{
			var sb = new StringBuilder();

			sb.Append('|');

			foreach (var group in _controlsOrdered)
			{
				foreach (var button in group)
				{
					if (_source.Definition.Axes.TryGetValue(button, out var range))
					{
						var val = createEmpty ? range.Neutral : _source.AxisValue(button);
						sb.Append(val.ToString().PadLeft(5, ' ')).Append(',');
					}
					else
					{
						sb.Append(!createEmpty && _source.IsPressed(button)
							? _mnemonics[button]
							: '.');
					}
				}
				sb.Append('|');
			}

			return sb.ToString();
		}
	}
}
