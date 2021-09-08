using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Security.Claims;

namespace Assimalign.Azure.Cosmos.Authorization
{
	public sealed class CosmosAuthorizationOptions<T>
		where T : class
	{
		// The Policy Collection
		private IDictionary<string, ICosmosAuthorizationPolicy> _policies = new Dictionary<string, ICosmosAuthorizationPolicy>();

		public CosmosAuthorizationOptions() { }

		/// <summary>
		/// </summary>
		public string Container { get; set; }

		/// <summary>
		/// </summary>
		public string Database { get; set; }

		/// <summary>
		/// </summary>
		public string Connection { get; set; }

		/// <summary>
		/// </summary>
		public IEnumerable<ICosmosAuthorizationPolicy<T>> GetPolicies(ClaimsPrincipal claimsPrincipal)
		{
			var policies = _policies.Where(a =>
			{
				var isMatch = true;

				if (!Array.TrueForAll(a.Value.Claims, b => claimsPrincipal.Claims.Any(c => c.Type == b)))
					isMatch = false;

				if (!Array.TrueForAll(a.Value.Roles, b => claimsPrincipal.Claims.Any(c => c.Type == "role" && c.Value == b)))
					isMatch = false;

				return isMatch;

			}).Select(x => x.Value as ICosmosAuthorizationPolicy<T>);


			return policies;
		}


		public ICosmosAuthorizationPolicy<T> GetPolicy(string name)
		{
			if (_policies.TryGetValue(name, out var policy))
			{
				return policy as ICosmosAuthorizationPolicy<T>;
			}
			else
			{
				throw new Exception("");
			}
		}

		public ICosmosAuthorizationPolicy<T> AddPolicy(string name, Action<CosmosAuthorizationBuilder<T>> builder)
		{
			var policyBuilder = new CosmosAuthorizationBuilder<T>();
			builder.Invoke(policyBuilder);
			var policy = policyBuilder.Build();
			_policies.Add(name, policy);
			return policy;
		}
	}
}
