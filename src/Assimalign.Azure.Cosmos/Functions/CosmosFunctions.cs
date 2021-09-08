using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Assimalign.Azure.Cosmos.Functions
{

	using Assimalign.Azure.Cosmos.Exceptions;

	/// <summary>
	/// 
	/// </summary>
	public abstract class CosmosFunctions
	{

		private ICosmosFunction nested;


		protected CosmosFunctions() { }


		/// <summary>
		/// Represents the 
		/// </summary>
		internal ICosmosFunction CurrentFunction
		{
			get => nested;
			set
			{
				if (nested is null)
				{
					nested = value;
				}
				else
				{
					var current = nested.GetType().Name;
					var change = value.GetType().Name;
					throw new CosmosInvalidMethodChainException(new[] { current, change });
				}
			}
		}


		/// <summary>
		/// Evaluates whether there is a chained function 
		/// </summary>
		[JsonIgnore]
		public bool IsFunctionAvailable => CurrentFunction != null;


		/// <summary>
		/// 
		/// </summary>
		[JsonPropertyName("$stringEquals")]
		public CosmosStringEquals StringEquals
		{
			get => CurrentFunction as CosmosStringEquals;
			set => CurrentFunction = value;
		}

		/// <summary>
		/// 
		/// </summary>
		[JsonPropertyName("$left")]
		public CosmosLeft Left
		{
			get => CurrentFunction as CosmosLeft;
			set => CurrentFunction = value;
		}

		/// <summary>
		/// 
		/// </summary>
		[JsonPropertyName("$startsWith")]
		public CosmosStartsWith StartsWith
		{
			get => CurrentFunction as CosmosStartsWith;
			set => CurrentFunction = value;
		}

		/// <summary>
		/// 
		/// </summary>
		[JsonPropertyName("$endsWith")]
		public CosmosEndsWith EndsWith
		{
			get => CurrentFunction as CosmosEndsWith;
			set => CurrentFunction = value;
		}

		/// <summary>
		/// 
		/// </summary>
		[JsonPropertyName("$substring")]
		public CosmosSubString SubString
		{
			get => CurrentFunction as CosmosSubString;
			set => CurrentFunction = value;
		}

		/// <summary>
		/// 
		/// </summary>
		[JsonPropertyName("$contains")]
		public CosmosContains Contains
		{
			get => CurrentFunction as CosmosContains;
			set => CurrentFunction = value;
		}

		/// <summary>
		/// 
		/// </summary>
		[JsonPropertyName("$replace")]
		public CosmosReplace Replace
		{
			get => CurrentFunction as CosmosReplace;
			set => CurrentFunction = value;
		}

		/// <summary>
		/// 
		/// </summary>
		[JsonPropertyName("$toLower")]
		public CosmosToLower ToLower
		{
			get => CurrentFunction as CosmosToLower;
			set => CurrentFunction = value;
		}

		/// <summary>
		/// 
		/// </summary>
		[JsonPropertyName("$toUpper")]
		public CosmosToUpper ToUpper
		{
			get => CurrentFunction as CosmosToUpper;
			set => CurrentFunction = value;
		}

		/// <summary>
		/// 
		/// </summary>
		[JsonPropertyName("$trim")]
		public CosmosTrim Trim
		{
			get => CurrentFunction as CosmosTrim;
			set => CurrentFunction = value;
		}

		/// <summary>
		/// 
		/// </summary>
		[JsonPropertyName("$trimStart")]
		public CosmosTrimStart TrimStart
		{
			get => CurrentFunction as CosmosTrimStart;
			set => CurrentFunction = value;
		}

		/// <summary>
		/// 
		/// </summary>
		[JsonPropertyName("$trimEnd")]
		public CosmosTrimEnd TrimEnd
		{
			get => CurrentFunction as CosmosTrimEnd;
			set => CurrentFunction = value;
		}

		/// <summary>
		/// 
		/// </summary>
		[JsonPropertyName("$stringIn")]
		public CosmosStringIn StringIn
		{
			get => CurrentFunction as CosmosStringIn;
			set => CurrentFunction = value;
		}

		/// <summary>
		/// 
		/// </summary>
		[JsonPropertyName("$length")]
		public CosmosLength Length
		{
			get => CurrentFunction as CosmosLength;
			set => CurrentFunction = value;
		}

		/// <summary>
		/// 
		/// </summary>
		[JsonPropertyName("$exists")]
		public CosmosArrayExists Exists
		{
			get => CurrentFunction as CosmosArrayExists;
			set => CurrentFunction = value;
		}

		/// <summary>
		/// 
		/// </summary>
		[JsonPropertyName("$round")]
		public CosmosRound Round
		{
			get => CurrentFunction as CosmosRound;
			set => CurrentFunction = value;
		}

		/// <summary>
		/// 
		/// </summary>
		[JsonPropertyName("$add")]
		public CosmosAdd Add
		{
			get => CurrentFunction as CosmosAdd;
			set => CurrentFunction = value;
		}

		/// <summary>
		/// 
		/// </summary>
		[JsonPropertyName("$subtract")]
		public CosmosSubtract Subtract
		{
			get => CurrentFunction as CosmosSubtract;
			set => CurrentFunction = value;
		}

		/// <summary>
		/// 
		/// </summary>
		[JsonPropertyName("$floor")]
		public CosmosFloor Floor
		{
			get => CurrentFunction as CosmosFloor;
			set => CurrentFunction = value;
		}

		/// <summary>
		/// 
		/// </summary>
		[JsonPropertyName("$ceiling")]
		public CosmosCeiling Ceiling
		{
			get => CurrentFunction as CosmosCeiling;
			set => CurrentFunction = value;
		}

		/// <summary>
		/// 
		/// </summary>
		[JsonPropertyName("$multiply")]
		public CosmosMultiply Multiply
		{
			get => CurrentFunction as CosmosMultiply;
			set => CurrentFunction = value;
		}

		/// <summary>
		/// 
		/// </summary>
		[JsonPropertyName("$divide")]
		public CosmosDivide Divide
		{
			get => CurrentFunction as CosmosDivide;
			set => CurrentFunction = value;
		}


		/// <summary>
		/// 
		/// </summary>
		[JsonPropertyName("$abs")]
		public CosmosAbs Abs
		{
			get => CurrentFunction as CosmosAbs;
			set => CurrentFunction = value;
		}


		/// <summary>
		/// 
		/// </summary>
		[JsonPropertyName("$acos")]
		public CosmosAcos Acos
		{
			get => CurrentFunction as CosmosAcos;
			set => CurrentFunction = value;
		}


		/// <summary>
		/// 
		/// </summary>
		[JsonPropertyName("$asin")]
		public CosmosAsin Asin
		{
			get => CurrentFunction as CosmosAsin;
			set => CurrentFunction = value;
		}
	}
}
