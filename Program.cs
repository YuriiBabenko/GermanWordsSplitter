namespace GermanWordsSplitter
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using GermanWordsSplitter.Classes;
	using Ninject;
	using Ninject.Parameters;

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
			InitObjectFactory();
			_stopwatch.Start();
			IDictionaryGetter dictionaryGetter = ObjectFactory.CurrentKernel.Get<IDictionaryGetter>(
				new ConstructorArgument("path", _dictionaryPath));
			IWordsGetter wordsGetter = ObjectFactory.CurrentKernel.Get<IWordsGetter>(
				new ConstructorArgument("path", _wordsPath));
			IDictionaryWordSplitter dictionaryWordSplitter = ObjectFactory.CurrentKernel.Get<IDictionaryWordSplitter>();
			IEnumerable<SplittedWordShell> splittedWordShells = dictionaryWordSplitter
				.SplitWordsByDictionary(dictionaryGetter, wordsGetter);
			_stopwatch.Stop();
			foreach (SplittedWordShell item in splittedWordShells) {
				Console.WriteLine($"{item.OriginalWord} - {item.SplittedWord}");
			}
			Console.WriteLine($"total ms: {_stopwatch.ElapsedMilliseconds}");
			Console.ReadKey();
		}

		static void InitObjectFactory() {
			ObjectFactory.CurrentKernel.Bind<IDictionaryGetter>().To<DictionaryGetter>();
			ObjectFactory.CurrentKernel.Bind<IWordsGetter>().To<WordsGetter>();
			ObjectFactory.CurrentKernel.Bind<IDictionaryWordSplitter>().To<DictionaryWordSplitter>();
		}

		#endregion
	}

	#endregion
}
