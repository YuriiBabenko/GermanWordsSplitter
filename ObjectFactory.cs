namespace GermanWordsSplitter
{
	using Ninject;

	#region Class: ObjectFactory

	public static class ObjectFactory
	{
		#region Properties: Public

		private static IKernel _currentKernel;
		public static IKernel CurrentKernel {
			get {
				return _currentKernel ?? (_currentKernel = new StandardKernel());
			}
		}

		#endregion
	}

	#endregion
}
