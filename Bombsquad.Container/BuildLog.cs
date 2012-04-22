using System;
using System.Collections.Generic;

namespace Bombsquad.Container
{
	internal class BuildLog
	{
		private readonly List<Entry> m_entries = new List<Entry>();
		private int m_indent;

		public void WriteLine( string format, params object[] args )
		{
			m_entries.Add( new Entry( m_indent, format, args ) );
		}

		public IDisposable BeginScope( string format, params object[] args )
		{
			WriteLine( format, args );
			m_indent++;
			return new Unindenter( this );
		}

		public override string ToString()
		{
			return string.Join( Environment.NewLine, m_entries );
		}

		private class Unindenter : IDisposable
		{
			private readonly BuildLog m_log;

			public Unindenter( BuildLog log )
			{
				m_log = log;
			}

			public void Dispose()
			{
				m_log.m_indent--;
			}
		}

		private class Entry
		{
			private readonly int m_indent;
			private readonly string m_format;
			private readonly object[] m_args;

			public Entry( int indent, string format, object[] args )
			{
				m_indent = indent;
				m_format = format;
				m_args = args;
			}

			public override string ToString()
			{
				return string.Concat( new string( '\t', m_indent ), string.Format( m_format, m_args ) );
			}
		}
	}
}