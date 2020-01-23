namespace GermanWordsSplitter
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.IO;
	using System.Linq;
	using System.Text;

	#region Class: Program

	class Program
	{
		#region Fields: private

		private static readonly Stopwatch _stopwatch = new Stopwatch();

		#endregion

		#region Constants: private

		private const string _dictionaryPath = @"dict";
		private const string _wordsPath = @"de-test-words.tsv";

		#endregion

		#region Methods: private

		static void Main() {
			Console.ReadKey();
		}

		#endregion
	}

	#endregion
}
