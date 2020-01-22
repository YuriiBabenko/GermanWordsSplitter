namespace GermanWordsSplitter.Classes
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	#region Interface: IWordsGetter

	public interface IWordsGetter
	{
		#region Methods: public

		Task<IEnumerable<string>> GetWordsTask();

		#endregion
	}

	#endregion

	#region Class: WordsGetter

	public class WordsGetter : IWordsGetter
	{
		#region Constructors: public

		public WordsGetter(string path) {
			Path = path;
		}

		#endregion

		#region Properties: public

		public string Path { get; }

		#endregion

		#region Methods: protected

		protected virtual IEnumerable<string> GetWords() {
			using (var sr = new StreamReader(Path, Encoding.UTF8)) {
				var list = new List<string>();
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
				return list.Distinct().ToArray();
			}
		}

		#endregion

		#region Methods: public

		public Task<IEnumerable<string>> GetWordsTask() {
			return Task.Run(() => GetWords());
		}

		#endregion
	}

	#endregion
}
