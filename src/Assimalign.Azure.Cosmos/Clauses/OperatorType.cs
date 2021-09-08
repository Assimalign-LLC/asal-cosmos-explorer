
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Assimalign.Azure.Cosmos.Clauses
{
	using Assimalign.Azure.Cosmos.Serialization;

	[JsonConverter(typeof(CosmosOperatorConverter))]
	public enum OperatorType : int
	{
		None = 0,
		Equal = 1,
		NotEqual = 2,
		GreaterThan = 3,
		GreaterThanOrEqual = 4,
		LessThan = 5,
		LessThanOrEqual = 6,
	}
}
