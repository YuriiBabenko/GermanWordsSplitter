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
			HashSet<string> dictionaryWords = GetDictionary();
			HashSet<string> wordsToSplit = GetWordsToSplit();
			_stopwatch.Start();
			foreach (string word in wordsToSplit) {
				if (dictionaryWords.Contains(word)) {
					Console.WriteLine($"{word} - {{ {word} }}");
					continue;
				}
				IEnumerable<string> splitted = SplitWord(word, dictionaryWords);
				string result = splitted?.Any() == false
					? $"{{ {word} }}"
					: String.Join(" ", splitted.Select(w => $"{{ {w} }}"));
				Console.WriteLine($"{word} - {result}");
			}
			_stopwatch.Stop();
			Console.WriteLine($"total ms: {_stopwatch.ElapsedMilliseconds}");
			Console.ReadKey();
		}

		static IEnumerable<string> SplitWord(string word, HashSet<string> dictionary) {
			if (dictionary.Contains(word)) {
				return new[] { word };
			}
			var wordParts = new List<string>[word.Length + 1];
			wordParts[0] = new List<string>();
			for (int i = 0; i < word.Length; i++) {
				if (wordParts[i] == null) {
					continue;
				}
				for (int j = i + 1; j <= word.Length; j++) {
					string substring = word.Substring(i, j - i);
					if (dictionary.Contains(substring)) {
						if (wordParts[j] == null) {
							wordParts[j] = new List<string>();
						}
						wordParts[j].Add(substring);
					}
				}
			}
			if (wordParts[word.Length] == null) {
				return new List<string>();
			}
			var result = new List<string>();
			CombineParts(wordParts, result, String.Empty, word.Length);
			return result;
		}

		static void CombineParts(List<string>[] wordParts, List<string> result, string current, int index) {
			if (index == 0) {
				result.Add(current.Trim());
				return;
			}
			foreach (string wordPart in wordParts[index]) {
				string combined = $"{wordPart} {current}";
				CombineParts(wordParts, result, combined, index - wordPart.Length);
			}
		}

		static HashSet<string> GetDictionary() {
			using (var sr = new StreamReader(_dictionaryPath, Encoding.UTF8)) {
				var list = new HashSet<string>();
				while (!sr.EndOfStream) {
					string line = sr.ReadLine();
					list.Add(line.ToLower());
				}
				return list;
			}
		}

		static HashSet<string> GetWordsToSplit() {
			using (var sr = new StreamReader(_wordsPath, Encoding.UTF8)) {
				var list = new HashSet<string>();
				char[] splitChars = new[] { ' ', '\t' };
				while (!sr.EndOfStream) {
					string line = sr.ReadLine();
					if (!line.StartsWith("de")) {
						continue;
					}
					string word = line.Split(splitChars, 2).LastOrDefault() ?? String.Empty;
					if (!String.IsNullOrEmpty(word)) {
						list.Add(word.ToLower());
					}
				}
				return list;
			}
		}

		#endregion
	}

	#endregion
}
