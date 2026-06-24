using UnityEngine;
using System;

namespace PolyLabs
{
    /// <summary>
    /// Parse a numeric value into a short scale string representation.
    /// </summary>
    public static class ShortScale 
    {

        /*
        * Current Version : 1.4.0 (September 30, 2024)
        */
        
        /*
        * The ShortScaleString script parses either a double, float, or int value 
        * value type into a form that is easier to read. The list supports values up to
        * the largest currently accepted value(one centillion). values given larger than
        * 999 centillion will be represented as "1000 centillion" and so on.
        * It is important to be aware of the limitations of each data type. for intParse, numbers higher than 1 billion
        * will throw an overflow error. similarly a float value will also overflow after too large a number is reached.
        * for applications where the needed use is well over 1+E100 use parseDouble as its maximum output value tested is
        * 1000 centillion, that's 306 0's!  
        */

        /// <summary>
        /// The list containing all short scale values.
        /// </summary>
        public static readonly string[] ShortScaleReference = {"thousand","million","billion","trillion","quadrillion","quintillion",
        "sextillion","septillion","octillion","nonillion","decillion",
        "undecillion","duodecillion","tredecillion","quattuordecillion","quindecillion",
        "sexdecillion","septendecillion","octodecillion","novemdecillion","vigintillion",
        "unvigintillion", "duovigintillion","trevigintillion","quattuorvigintillion",
        "quinvigitillion","sexvigintillion","septenvigitillion","octovigintillion","novemvigitillion",
        "trigintillion","untrigintillion","duotrigintillion","tretrigintillion","quattuortrigintillion",
        "quintrigintillion","sextrigintillion","septentrigintillion","octotrigintillion",
        "novemtrigintillion","quadragintillion","unquadragintillion","duoquadragintillion",
        "trequadragintillion","quattuorquadragintillion","quinquadragintillion","sexquadragintillion",
        "septenquadragintillion","octoquadragintillion","novemquadragintillion","quinquagintillion","unquinquagintillion",
        "duoquinquagintillion","trequinquagintillion","quattuorquinquagintillion","quinquinquagintillion",
        "sexquinquagintillion","septenquinquagintillion","octoquinquagintillion","novemquinquagintillion","sexagintillion",
        "unsexagintillion","duosexagintillion","tresexagintillion","quattuorsexagintillion","quinsexagintillion",
        "sexsexagintillion","septensexagintillion","octosexagintillion","novemsexagintillion","septuagintillion",
        "unseptuagintillion","duoseptuagintillion","treseptuagintillion","quattuorseptuagintillion","quinseptuagintillion",
        "sexseptuagintillion","septenseptuagintillion","octoseptuagintillion","novemseptuagintillion",
        "octogintillion","unoctogintillion","duooctogintillion","treoctogintillion","quattuoroctogintillion",
        "quinoctogintillion","sexoctogintillion","septenoctogintillion","octooctogintillion","novemoctogintillion","nonagintillion",
        "unnonagintillion","duononagintillion","trenonagintillion","quattuornonagintillion","quinnonagintillion",
        "sexnonagintillion","septennonagintillion","octononagintillion","novemnonagintillion","centillion"};

        ///<summary>
        /// List containing short scale values in symbol form. Can be further expanded in future updates.
        /// </summary>
        public static readonly string[] ShortScaleSymbolReference = { "K", "M", "B", "T", "q", "Q", "s", "S", "O", "N", "D" };

        /// <summary>
        /// Parses the double value into short scale notation.
        /// </summary>
        /// <returns>The short scale string.</returns>
        /// <param name="value">The input value that will be parsed.</param>
        /// <param name="precision">(Optional) The decimal precision that should be represented (subject to data type round off).
        /// Default value is 3.</param> 
        /// <param name="startShortScale">(Optional) Set the value to begin parsing to short scale. Default value is 1 million.</param>
        /// <param name="useSymbol">
        /// (Optional) use the single symbol list for more shortened notation. currently supports only to Decillion. Default value is false.
        /// </param>
        /// <param name="simplifyPrecision">
        /// (Optional) Default true. When displaying the value, should trailing zeros be removed or forced to display the full precision digits.
        /// <code>
        /// ParseDouble(25.0d, startShortScale: 0, precision: 3, simplifyPrecision: true); // "25"
        /// ParseDouble(25.0d, startShortScale: 0, precision: 3, simplifyPrecision: false); // "25.000"
        /// </code>
        /// </param>
        public static string ParseDouble(
            double value,
            in int precision = 3,
            double startShortScale = 1000000,
            bool useSymbol = false,
            bool simplifyPrecision = true
        )
        {
            string symbol = "";
            string strVal = "";
            ParseDoubleInternal(value, ref strVal, ref symbol, precision, startShortScale, useSymbol, simplifyPrecision);
            return $"{strVal} {symbol}";
        }

        /// <summary>
        /// Parses the double value into short scale notation, splitting the data into the separate parts.
        /// </summary>
        /// <returns>A ShortScaleData object containing the value and symbol.</returns>
        /// <param name="value">The input value that will be parsed.</param>
        /// <param name="precision">(Optional) The decimal precision that should be represented (subject to data type round off).
        /// Default value is 3.</param> 
        /// <param name="startShortScale">(Optional) Set the value to begin parsing to short scale. Default value is 1 million.</param>
        /// <param name="useSymbol">
        /// (Optional) use the single symbol list for more shortened notation. currently supports only to Decillion. Default value is false.
        /// </param>
        /// <param name="simplifyPrecision">
        /// (Optional) Default true. When displaying the value, should trailing zeros be removed or forced to display the full precision digits.
        /// <code>
        /// ParseDoubleSplit(25.0d, startShortScale: 0, precision: 3, simplifyPrecision: true); // "25"
        /// ParseDoubleSplit(25.0d, startShortScale: 0, precision: 3, simplifyPrecision: false); // "25.000"
        /// </code>
        /// </param>
        public static ShortScaleData ParseDoubleSplit(
            double value,
            in int precision = 3,
            in double startShortScale = 1000000,
            in bool useSymbol = false,
            in bool simplifyPrecision = true
        )
        {
            var data = new ShortScaleData();
            ParseDoubleInternal(value, ref data.value, ref data.symbol, precision, startShortScale, useSymbol, simplifyPrecision);
            return data;
        }

        /// <summary>
        /// Parses a double value into short scale notation, splitting the data into the separate parts.
        /// </summary>
        /// <param name="value">The input value that will be parsed.</param>
        /// <param name="data">A reference to an existing data to reuse the same object.</param>
        /// <param name="precision">(Optional) The decimal precision that should be represented (subject to data type round off).
        /// Default value is 3.</param> 
        /// <param name="startShortScale">(Optional) Set the value to begin parsing to short scale. Default value is 1 million.</param>
        /// <param name="useSymbol">
        /// (Optional) use the single symbol list for more shortened notation. currently supports only to Decillion. Default value is false.
        /// </param>
        /// <param name="simplifyPrecision">
        /// (Optional) Default true. When displaying the value, should trailing zeros be removed or forced to display the full precision digits.
        /// <code>
        /// ParseDoubleSplit(25.0d, startShortScale: 0, precision: 3, simplifyPrecision: true); // "25"
        /// ParseDoubleSplit(25.0d, startShortScale: 0, precision: 3, simplifyPrecision: false); // "25.000"
        /// </code>
        /// </param>
        public static void ParseDoubleSplit(
            double value,
            ref ShortScaleData data,
            in int precision = 3,
            in double startShortScale = 1000000,
            in bool useSymbol = false,
            in bool simplifyPrecision = true
        )
        {
            ParseDoubleInternal(value, ref data.value, ref data.symbol, precision, startShortScale, useSymbol, simplifyPrecision);
        }

        /// <summary>
        /// Handles the internal calculation of the short scale string for double values.
        /// </summary>
        private static void ParseDoubleInternal(
            double value,
            ref string stringValue,
            ref string symbol,
            in int precision,
            double startShortScale,
            bool useSymbol,
            bool simplifyPrecision
        )
        {
            int index = -1;
            int isNegative = 1;
            string addPrecision = new string (simplifyPrecision ? '#' : '0', precision);
            double precisionValue = Mathf.Pow (10, precision);

            if (value < 0) 
            {
                isNegative = -1;
                value *= isNegative;
            } 
            else if (!(value > 0d)) 
            {
                stringValue = "0";
                symbol = "";
                return;
            }

            if (value < 1000d || value < startShortScale)
            {
                stringValue = (Math.Floor(value * isNegative * precisionValue) / precisionValue).ToString ("#,#." + addPrecision);
                symbol = "";
                return;
            }
            
            int maxIndex = useSymbol 
                ? ShortScaleSymbolReference.Length - 1 
                : ShortScaleReference.Length - 1;

            while (value >= 1000d && index < maxIndex) 
            {   
                value *= 0.001d;
                index++;
            }

            symbol = useSymbol 
                ? ShortScaleSymbolReference[index]
                : ShortScaleReference [index];
            
            stringValue = (Math.Floor(value * isNegative * precisionValue) / precisionValue)
                .ToString("#,#." + addPrecision);
        }

        /// <summary>
        /// Parses the float value into short scale notation.
        /// </summary>
        /// <returns>The short scale string.</returns>
        /// <param name="value">The input value that will be parsed.</param>
        /// <param name="precision">(Optional) The decimal precision that should be represented (subject to data type round off).
        /// Default value is 3.</param> 
        /// <param name="startShortScale">(Optional) Set the value to begin parsing to short scale. Default value is 1 million.</param>
        /// <param name="useSymbol">
        /// (Optional) use the single symbol list for more shortened notation. currently supports only to Decillion.
        /// </param>
        /// <param name="simplifyPrecision">
        /// (Optional) Default true. When displaying the value, should trailing zeros be removed or forced to display the full precision digits.
        /// <code>
        /// ParseFloat(25.0f, startShortScale: 0, precision: 3, simplifyPrecision: true); // "25"
        /// ParseFloat(25.0f, startShortScale: 0, precision: 3, simplifyPrecision: false); // "25.000"
        /// </code>
        /// </param>
        public static string ParseFloat(
            float value,
            in int precision = 3,
            in float startShortScale = 1000000,
            in bool useSymbol = false,
            in bool simplifyPrecision = true
        )
        {
            string symbol = "";
            string strVal = "";
            ParseFloatInternal(value, ref strVal, ref symbol, precision, startShortScale, useSymbol, simplifyPrecision);
            return $"{strVal} {symbol}";
        }

        /// <summary>
        /// Parses the float value into short scale notation, splitting the data into the separate parts.
        /// </summary>
        /// <returns>A ShortScaleData object containing the value and symbol.</returns>
        /// <param name="value">The input value that will be parsed.</param>
        /// <param name="precision">(Optional) The decimal precision that should be represented (subject to data type round off).
        /// Default value is 3.</param> 
        /// <param name="startShortScale">(Optional) Set the value to begin parsing to short scale. Default value is 1 million.</param>
        /// <param name="useSymbol">
        /// (Optional) use the single symbol list for more shortened notation. currently supports only to Decillion. Default value is false.
        /// </param>
        /// <param name="simplifyPrecision">
        /// (Optional) Default true. When displaying the value, should trailing zeros be removed or forced to display the full precision digits.
        /// <code>
        /// ParseFloatSplit(25.0f, startShortScale: 0, precision: 3, simplifyPrecision: true); // "25"
        /// ParseFloatSplit(25.0f, startShortScale: 0, precision: 3, simplifyPrecision: false); // "25.000"
        /// </code>
        /// </param>
        public static ShortScaleData ParseFloatSplit(
            float value,
            in int precision = 3,
            in float startShortScale = 1000000,
            in bool useSymbol = false,
            in bool simplifyPrecision = true
        )
        {
            var data = new ShortScaleData();
            ParseFloatInternal(value, ref data.value, ref data.symbol, precision, startShortScale, useSymbol, simplifyPrecision);

            return data;
        }

        /// <summary>
        /// Parses a float value into short scale notation, splitting the data into the separate parts.
        /// </summary>
        /// <param name="value">The input value that will be parsed.</param>
        /// <param name="data">A reference to an existing data to reuse the same object.</param>
        /// <param name="precision">(Optional) The decimal precision that should be represented (subject to data type round off).
        /// Default value is 3.</param> 
        /// <param name="startShortScale">(Optional) Set the value to begin parsing to short scale. Default value is 1 million.</param>
        /// <param name="useSymbol">
        /// (Optional) use the single symbol list for more shortened notation. currently supports only to Decillion. Default value is false.
        /// </param>
        /// <param name="simplifyPrecision">
        /// (Optional) Default true. When displaying the value, should trailing zeros be removed or forced to display the full precision digits.
        /// <code>
        /// ParseFloatSplit(25.0f, startShortScale: 0, precision: 3, simplifyPrecision: true); // "25"
        /// ParseFloatSplit(25.0f, startShortScale: 0, precision: 3, simplifyPrecision: false); // "25.000"
        /// </code>
        /// </param>
        public static void ParseFloatSplit(
            float value,
            ref ShortScaleData data,
            in int precision = 3,
            in float startShortScale = 1000000,
            in bool useSymbol = false,
            in bool simplifyPrecision = true
        )
        {
            ParseFloatInternal(value, ref data.value, ref data.symbol, precision, startShortScale, useSymbol, simplifyPrecision);
        }

        /// <summary>
        /// Handles the internal calculation of the short scale string for float values.
        /// </summary>
        private static void ParseFloatInternal(
            float value,
            ref string stringValue,
            ref string symbol,
            int precision = 3,
            float startShortScale = 1000000,
            bool useSymbol = false,
            bool simplifyPrecision = true
        )
        {
            int index = -1;
            int isNegative = 1;
            string addPrecision = new string (simplifyPrecision ? '#' : '0', precision);
            double precisionValue = Mathf.Pow (10, precision);

            if (value < 0) 
            {
                isNegative = -1;
                value *= isNegative;
            } 
            else if (!(value > 0))
            {
                stringValue = "0";
                symbol = "";
                return;
            }

            if (value < 1000 || value < startShortScale) 
            {
                stringValue = (Math.Floor(value * isNegative * precisionValue) / precisionValue).ToString ("#,#." + addPrecision);
                symbol = "";
                return;
            }

            int maxIndex = useSymbol 
                ? ShortScaleSymbolReference.Length - 1 
                : ShortScaleReference.Length - 1;

            while (value >= 1000.0f && index < maxIndex) 
            {   
                value *= 0.001f;
                index++;
            }

            stringValue = (Math.Floor(value * isNegative * precisionValue ) / precisionValue ).ToString ("#,#." + addPrecision);
            symbol = useSymbol 
                ? ShortScaleSymbolReference[index] 
                : ShortScaleReference[index];
        }

        /// <summary>
        /// Parses the int value into short scale notation.
        /// </summary>
        /// <returns>The short scale string.</returns>
        /// <param name="value">The input value that will be parsed.</param>
        /// <param name="precision">(Optional) The decimal precision that should be represented (subject to data type round off).
        /// Default value is 3.</param> 
        /// <param name="startShortScale">(Optional) Set the value to begin parsing to short scale. Default value is 1 million.</param>
        /// <param name="useSymbol">
        /// (Optional) use the single symbol list for more shortened notation. currently supports only to Decillion.
        /// </param>
        /// <param name="simplifyPrecision">
        /// (Optional) Default true. When displaying the value, should trailing zeros be removed or forced to display the full precision digits.
        /// <code>
        /// ParseInt(25, precision: 3, simplifyPrecision: true); // "25"
        /// ParseInt(25, precision: 3, simplifyPrecision: false); // "25.000"
        /// </code>
        /// </param>
        /// <remarks>
        /// Be aware that signed ints only range from -2.1 billion to 2.1 billion. If you are dealing with larger numbers,
        /// consider using either float or double.
        /// </remarks>
        public static string ParseInt(
            int value,
            in int precision = 3,
            in int startShortScale = 1000000,
            in bool useSymbol = false,
            in bool simplifyPrecision = true
        )
        {
            string strVal = "";
            string symbol = "";
            ParseIntInternal(value, ref strVal, ref symbol, precision, startShortScale, useSymbol, simplifyPrecision);
            return $"{strVal} {symbol}";
        }

        /// <summary>
        /// Parses an int value into short scale notation, splitting the data into the separate parts.
        /// </summary>
        /// <param name="value">The input value that will be parsed.</param>
        /// <param name="precision">(Optional) The decimal precision that should be represented (subject to data type round off).
        /// Default value is 3.</param> 
        /// <param name="startShortScale">(Optional) Set the value to begin parsing to short scale. Default value is 1 million.</param>
        /// <param name="useSymbol">
        /// (Optional) use the single symbol list for more shortened notation. currently supports only to Decillion. Default value is false.
        /// </param>
        /// <param name="simplifyPrecision">
        /// (Optional) Default true. When displaying the value, should trailing zeros be removed or forced to display the full precision digits.
        /// <code>
        /// ParseInt(25, startShortScale: 0, precision: 3, simplifyPrecision: true); // "25"
        /// ParseInt(25, startShortScale: 0, precision: 3, simplifyPrecision: false); // "25.000"
        /// </code>
        /// </param>
        /// <remarks>
        /// Be aware that signed ints only range from -2.1 billion to 2.1 billion. If you are dealing with larger numbers,
        /// consider using either float or double.
        /// </remarks>
        public static ShortScaleData ParseIntSplit(
            int value,
            in int precision = 3,
            in int startShortScale = 1000000,
            in bool useSymbol = false,
            in bool simplifyPrecision = true
        )
        {
            var data = new ShortScaleData();
            ParseIntInternal(value, ref data.value, ref data.symbol, precision, startShortScale, useSymbol, simplifyPrecision);
            return data;
        }

        /// <summary>
        /// Parses an int value into short scale notation, splitting the data into the separate parts.
        /// </summary>
        /// <param name="value">The input value that will be parsed.</param>
        /// <param name="data">A reference to an existing data to reuse the same object.</param>
        /// <param name="precision">(Optional) The decimal precision that should be represented (subject to data type round off).
        /// Default value is 3.</param> 
        /// <param name="startShortScale">(Optional) Set the value to begin parsing to short scale. Default value is 1 million.</param>
        /// <param name="useSymbol">
        /// (Optional) use the single symbol list for more shortened notation. currently supports only to Decillion. Default value is false.
        /// </param>
        /// <param name="simplifyPrecision">
        /// (Optional) Default true. When displaying the value, should trailing zeros be removed or forced to display the full precision digits.
        /// <code>
        /// ParseIntSplit(25, startShortScale: 0, precision: 3, simplifyPrecision: true); // "25"
        /// ParseIntSplit(25, startShortScale: 0, precision: 3, simplifyPrecision: false); // "25.000"
        /// </code>
        /// </param>
        /// <remarks>
        /// Be aware that signed ints only range from -2.1 billion to 2.1 billion. If you are dealing with larger numbers,
        /// consider using either float or double.
        /// </remarks>
        public static void ParseIntSplit(
            int value,
            ref ShortScaleData data,
            in int precision = 3,
            in int startShortScale = 1000000,
            in bool useSymbol = false,
            in bool simplifyPrecision = true
        )
        {
            ParseIntInternal(value, ref data.value, ref data.symbol, precision, startShortScale, useSymbol, simplifyPrecision);
        }

        private static void ParseIntInternal(
            int value,
            ref string stringValue,
            ref string symbol,
            in int precision = 3,
            in int startShortScale = 1000000,
            in bool useSymbol = false,
            in bool simplifyPrecision = true
        )
        {
            int index = -1;
            int isNegative = 1;
            string addPrecision = new string (simplifyPrecision ? '#' : '0', precision);

            if (value < 0) 
            {
                isNegative = -1;
                value *= isNegative;
            } 
            else if (!(value > 0))
            {
                stringValue = "0";
                symbol = "";
                return;
            }

            if (value < 1000 || value < startShortScale) 
            {
                stringValue = (value * isNegative).ToString ("#,#");
                symbol = "";
                return;
            }

            switch (value)
            {
                case < 1_000_000:
                    // Thousands
                    index = 0;
                    value = (int)(value * 1f / 1_000f);
                    break;
                case < 1_000_000_000:
                    // Millions
                    index = 1;
                    value = (int)(value * 1f / 1_000_000f);
                    break;
                default:
                    // Billions (Max signed int ~2.1 billion)
                    index = 2;
                    value = (int)(value * 1f / 1_000_000_000f);
                    break;
            }

            stringValue = (value * isNegative).ToString ("#,#." + addPrecision);
            symbol = useSymbol 
                ? ShortScaleSymbolReference[index] 
                : ShortScaleReference[index];
        }
    }

    /// <summary>
    /// Allows for the value to be separated from the symbol; both can be styled separately.
    /// </summary>
    public struct ShortScaleData
    {
        ///<summary>
        /// The leading value of the short scale i.e., "123.45" from "123.45 million"
        ///</summary>
        public string value;
        ///<summary>
        /// The trailing symbol of the short scale i.e., "million" from "123.45 million"
        ///</summary>
        public string symbol;
    }
}
