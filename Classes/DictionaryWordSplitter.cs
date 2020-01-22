namespace GermanWordsSplitter.Classes
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

	#region Interface: IDictionaryWordSplitter

	public interface IDictionaryWordSplitter
	{
		#region Methods: public

		IEnumerable<SplittedWordShell> SplitWordsByDictionary(IDictionaryGetter dictionaryGetter, IWordsGetter wordsGetter);

		#endregion
	}

	#endregion

	#region Class: DictionaryWordSplitter

	public class DictionaryWordSplitter : IDictionaryWordSplitter
	{
		#region Methods: protected

		protected virtual async Task<IEnumerable<SplittedWordShell>> SplitWordsByDictionaryAsync(IDictionaryGetter dictionaryGetter, IWordsGetter wordsGetter) {
			IEnumerable<string>[] taskResults = await Task.WhenAll(dictionaryGetter.GetDictionaryTask(), wordsGetter.GetWordsTask());
			IEnumerable<string> dictionaryWords = taskResults[0];
			IEnumerable<string> wordsToSplit = taskResults[1];
			return wordsToSplit.Select(word => {
				if (dictionaryWords.Contains(word)) {
					return new SplittedWordShell {
						OriginalWord = word,
						SplittedWord = word
					};
				}
				string result = word;
				foreach (string dictWord in dictionaryWords) {
					string tmpRes = result;
					if (tmpRes.Contains(dictWord)) {
						tmpRes = tmpRes.Replace(dictWord, $" {dictWord} ").Trim();
					}
					if (tmpRes != result && tmpRes.Split(' ').All(w => dictionaryWords.Contains(w))) {
						result = tmpRes.Trim();
					}
				}
				return new SplittedWordShell {
					OriginalWord = word,
					SplittedWord = result
				};
			});
		}

		#endregion

		#region Methods: public

		public IEnumerable<SplittedWordShell> SplitWordsByDictionary(IDictionaryGetter dictionaryGetter, IWordsGetter wordsGetter) {
			try {
				Task<IEnumerable<SplittedWordShell>> task = SplitWordsByDictionaryAsync(dictionaryGetter, wordsGetter);
				task.Wait();
				return task.Result;
			} catch (Exception e) {
				Console.WriteLine(e.Message);
				return null;
			}
		}

		#endregion
	}

	#endregion
}
