using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Azure.Cosmos.Authorization
{
	//public sealed class CosmosAuthorizationPolicy<T> : ICosmosAuthorizationPolicy<T>
	//	where T : class
	//{
	//	private IDictionary<string, MemberInfo> _members = new Dictionary<string, MemberInfo>();

	//	internal CosmosAuthorizationPolicy(string policy, string[] roles)
	//	{
	//		this.Policy = policy;
	//		this.Roles = roles;
	//	}

	//	/// <summary>
	//	/// </summary>
	//	public string Policy { get; }

	//	/// <summary>
	//	/// </summary>
	//	public string[] Roles { get; }

	//	/// <summary>
	//	/// </summary>
	//	public string[] Claims { get; }

	//	/// <summary>
	//	/// </summary>
	//	public IEnumerable<MemberInfo> Members => _members.Select(x => x.Value);

	//	/// <summary>
	//	/// </summary>
	//	public void AddField(string name, MemberInfo member) =>
	//		_members.Add(name, member);

	//}
}
