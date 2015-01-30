using System;
using System.Linq;
using System.Linq.Expressions;

namespace System.Data.Linq.Provider.Visitors
{
	internal static class Funcletizer
	{
		/// <summary>
		/// Expression handler which marks all expressions which solely refer to local elements, so these can be inlined and won't be
		/// converted to SQL fragments.
		/// </summary>
		/// <param name="expression"></param>
		/// <returns></returns>
		internal static Expression Funcletize(Expression expression)
		{
			return new Localizer(new LocalMapper().MapLocals(expression)).Localize(expression);
		}
	}
}
