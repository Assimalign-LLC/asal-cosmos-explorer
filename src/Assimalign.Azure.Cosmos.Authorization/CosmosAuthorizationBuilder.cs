using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Azure.Cosmos.Authorization
{
	public class CosmosAuthorizationBuilder<T>
		where T : class
	{
		private ICosmosAuthorizationPolicy<T> _policy;

		/// <summary>
		/// </summary>
		internal ICosmosAuthorizationPolicy<T> Build() => _policy;

		/// <summary>
		/// </summary>
		public CosmosAuthorizationBuilder<T> AddField<TProperty>(Expression<Func<T, TProperty>> expression)
		{
			if (expression.Body is MemberExpression member)
			{
				_policy.AddField(member.Member.Name, member.Member);
			}
			else
			{
				throw new Exception("");
			}

			return this;
		}
	}
}
