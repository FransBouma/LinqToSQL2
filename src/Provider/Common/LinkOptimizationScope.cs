using System.Collections.Generic;
using System.Data.Linq.Provider.NodeTypes;

namespace System.Data.Linq.Provider.Common
{
	internal class LinkOptimizationScope 
	{
		Dictionary<object, SqlExpression> map;
		LinkOptimizationScope previous;

		internal LinkOptimizationScope(LinkOptimizationScope previous) {
			this.previous = previous;
		}
		internal void Add(object linkId, SqlExpression expr) {
			if (this.map == null) {
				this.map = new Dictionary<object,SqlExpression>();
			}
			this.map.Add(linkId, expr);
		}
		internal bool TryGetValue(object linkId, out SqlExpression expr) {
			expr = null;
			return (this.map != null && this.map.TryGetValue(linkId, out expr)) ||
				   (this.previous != null && this.previous.TryGetValue(linkId, out expr));
		}
	}
}