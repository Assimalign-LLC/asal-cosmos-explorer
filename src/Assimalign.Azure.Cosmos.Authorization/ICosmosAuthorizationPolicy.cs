using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Azure.Cosmos.Authorization
{
	public interface ICosmosAuthorizationPolicy
	{
		/// <summary>
		/// </summary>
		string Policy { get; }

		///<summary>
		/// Ths Roles required for this policy
		/// </summary>
		string[] Roles { get; }

		///<summary>
		/// 
		/// </summary>
		string[] Claims { get; }

		/// <summary>
		/// </summary>
		void AddField(string name, MemberInfo member);
	}

	public interface ICosmosAuthorizationPolicy<T> : ICosmosAuthorizationPolicy
		where T : class
	{
		IEnumerable<MemberInfo> Members { get; }
	}
}
