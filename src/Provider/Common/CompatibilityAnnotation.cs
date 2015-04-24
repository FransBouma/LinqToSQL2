using System.Linq;

namespace System.Data.Linq.Provider.Common
{

	/// <summary>
	/// Annotation which indicates that the given node will cause a compatibility problem
	/// for the indicated set of provider modes.
	/// </summary>
	internal class CompatibilityAnnotation : SqlNodeAnnotation
	{
		private Enum[] _providerModes;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="message">The compatibility message.</param>
		/// <param name="providerModes">The set of providers this compatibility issue applies to.</param>
		internal CompatibilityAnnotation(string message, params Enum[] providerModes)
			: base(message)
		{
			_providerModes = providerModes;
		}


		/// <summary>
		/// Returns true if this annotation applies to the specified provider.
		/// </summary>
		internal bool AppliesTo(Enum provider)
		{
			return _providerModes.Any(p=>p.Equals(provider));
		}
	}
}
