using System;
using System.Collections.Generic;
using System.Text;

namespace Assimalign.Azure.Cosmos.Functions
{
    internal class CosmosLinqToSqlFunctions
    {
        // String Functions
        public const string ToLower         = "ToLower";            // Added 
        public const string ToUpper         = "ToUpper";            // Added
        public const string StartsWith      = "StartsWith";         // Added
        public const string EndsWith        = "EndsWith";           // Added
        public const string Contains        = "Contains";           // Added
        public const string SubString       = "Substring";          // Added
        public const string Replace         = "Replace";            // Added 
        public const string StringEquals    = "StringEquals";       // Added
        public const string Concat          = "Concat";             // Unsupported: This morphs the data and can cause complication on the query request
        public const string IndexOf         = "IndexOf";
        public const string StringCount     = "StringCount";        // Added
        public const string StringReverse   = "StringReverse";
        public const string TrimStart       = "TrimStart";          // Added
        public const string TrimEnd         = "TrimEnd";            // Added
        public const string Left            = "Left";               // Added   
        public const string Right           = "Right";              // Added
        public const string Trim            = "Trim";               // Added

        // Math Functions
        public const string Abs             = "Abs";                // Added
        public const string Acos            = "Acos";               // Added
        public const string Asin            = "Asin";               // Added
        public const string Atan            = "Atan";
        public const string Ceiling         = "Ceiling";
        public const string Cos             = "Cos";
        public const string Exp             = "Exp";
        public const string Floor           = "Floor";
        public const string Log             = "Log";
        public const string Log10           = "Log10";
        public const string Pow             = "Pow";
        public const string Round           = "Round";              // Added
        public const string Sign            = "Sign";
        public const string Sin             = "Sin";
        public const string Sqrt            = "Sqrt";
        public const string Tan             = "Tan";
        public const string Truncate        = "Truncate";


        // Array Functions
        public const string ArrayContains = "";


        // DateTime Functions
    }
}
