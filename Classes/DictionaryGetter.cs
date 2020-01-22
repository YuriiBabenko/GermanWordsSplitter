namespace GermanWordsSplitter.Classes
{
	using System.Collections.Generic;
	using System.IO;
	using System.Text;
	using System.Threading.Tasks;

	#region Interface: IDictionaryGetter

	public interface IDictionaryGetter
	{
		#region Methods: public

		Task<IEnumerable<string>> GetDictionaryTask();

		#endregion
	}

	#endregion

	#region Class: DictionaryGetter

	public class DictionaryGetter : IDictionaryGetter
	{
		#region Constructors: public

		public DictionaryGetter(string path) {
			Path = path;
		}

		#endregion

		#region Properties: public

		public string Path { get; }

		#endregion

		#region Methods: protected

		protected virtual IEnumerable<string> GetDictionary() {
			using (var sr = new StreamReader(Path, Encoding.UTF8)) {
				var list = new List<string>();
				while (!sr.EndOfStream) {
					string line = sr.ReadLine();
					list.Add(line.ToLower());
				}
				return list;
			}
		}

		#endregion

		#region Methods: public

		public virtual Task<IEnumerable<string>> GetDictionaryTask() {
			return Task.Run(GetDictionary);
		}

		#endregion
	}

	#endregion
}
