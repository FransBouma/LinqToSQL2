using System.Text;

namespace System.Data.Linq.DbEngines.SqlServer
{
#warning [FB] BREAKING CHANGE. This namespace was in System.Data.Linq.SqlClient. Rename the new namespace or keep breaking change.

	/// <summary>
	/// Public helper methods to be used in queries. 
	/// </summary>
	public static class SqlHelpers
	{

		public static string GetStringContainsPattern(string text, char escape)
		{
			bool usedEscapeChar = false;
			return GetStringContainsPattern(text, escape, out usedEscapeChar);
		}

		internal static string GetStringContainsPattern(string text, char escape, out bool usedEscapeChar)
		{
			if(text == null)
			{
				throw Error.ArgumentNull("text");
			}
			return "%" + EscapeLikeText(text, escape, false, out usedEscapeChar) + "%";
		}

		internal static string GetStringContainsPatternForced(string text, char escape)
		{
			if(text == null)
			{
				throw Error.ArgumentNull("text");
			}
			bool usedEscapeChar = false;
			return "%" + EscapeLikeText(text, escape, true, out usedEscapeChar) + "%";
		}

		public static string GetStringStartsWithPattern(string text, char escape)
		{
			bool usedEscapeChar = false;
			return GetStringStartsWithPattern(text, escape, out usedEscapeChar);
		}

		internal static string GetStringStartsWithPattern(string text, char escape, out bool usedEscapeChar)
		{
			if(text == null)
			{
				throw Error.ArgumentNull("text");
			}
			return EscapeLikeText(text, escape, false, out usedEscapeChar) + "%";
		}

		internal static string GetStringStartsWithPatternForced(string text, char escape)
		{
			if(text == null)
			{
				throw Error.ArgumentNull("text");
			}
			bool usedEscapeChar = false;
			return EscapeLikeText(text, escape, true, out usedEscapeChar) + "%";
		}

		public static string GetStringEndsWithPattern(string text, char escape)
		{
			bool usedEscapeChar = false;
			return GetStringEndsWithPattern(text, escape, out usedEscapeChar);
		}

		internal static string GetStringEndsWithPattern(string text, char escape, out bool usedEscapeChar)
		{
			if(text == null)
			{
				throw Error.ArgumentNull("text");
			}
			return "%" + EscapeLikeText(text, escape, false, out usedEscapeChar);
		}

		internal static string GetStringEndsWithPatternForced(string text, char escape)
		{
			if(text == null)
			{
				throw Error.ArgumentNull("text");
			}
			bool usedEscapeChar = false;
			return "%" + EscapeLikeText(text, escape, true, out usedEscapeChar);
		}

		private static string EscapeLikeText(string text, char escape, bool forceEscaping, out bool usedEscapeChar)
		{
			usedEscapeChar = false;
			if(!(forceEscaping || text.Contains("%") || text.Contains("_") || text.Contains("[") || text.Contains("^")))
			{
				return text;
			}
			StringBuilder sb = new StringBuilder(text.Length);
			foreach(char c in text)
			{
				if(c == '%' || c == '_' || c == '[' || c == '^' || c == escape)
				{
					sb.Append(escape);
					usedEscapeChar = true;
				}
				sb.Append(c);
			}
			return sb.ToString();
		}

		public static string TranslateVBLikePattern(string pattern, char escape)
		{
			if(pattern == null)
			{
				throw Error.ArgumentNull("pattern");
			}
			const char vbMany = '*';
			const char sqlMany = '%';
			const char vbSingle = '?';
			const char sqlSingle = '_';
			const char vbDigit = '#';
			const string sqlDigit = "[0-9]";
			const char vbOpenBracket = '[';
			const char sqlOpenBracket = '[';
			const char vbCloseBracket = ']';
			const char sqlCloseBracket = ']';
			const char vbNotList = '!';
			const char sqlNotList = '^';
			const char vbCharRange = '-';
			const char sqlCharRange = '-';

			// walk the string, performing conversions
			StringBuilder result = new StringBuilder();
			bool bracketed = false;
			bool charRange = false;
			bool possibleNotList = false;
			int numBracketedCharacters = 0;

			foreach(char patternChar in pattern)
			{
				if(bracketed)
				{
					numBracketedCharacters++;

					// if we're in a possible NotList, anything other than a close bracket will confirm it
					if(possibleNotList)
					{
						if(patternChar != vbCloseBracket)
						{
							result.Append(sqlNotList);
							possibleNotList = false;
						}
						else
						{
							result.Append(vbNotList);
							possibleNotList = false;
						}
					}

					switch(patternChar)
					{
						case vbNotList:
						{
							// translate to SQL's NotList only if the first character in the group
							if(numBracketedCharacters == 1)
							{
								// latch this, and detect the next cycle
								possibleNotList = true;
							}
							else
							{
								result.Append(patternChar);
							}
							break;
						}
						case vbCloseBracket:
						{
							// close down the bracket group
							bracketed = false;
							possibleNotList = false;
							result.Append(sqlCloseBracket);
							break;
						}
						case vbCharRange:
						{
							if(charRange)
							{
								// we've seen the char range indicator already -- SQL 
								// doesn't support multiple ranges in the same group
								throw Error.VbLikeDoesNotSupportMultipleCharacterRanges();
							}
							else
							{
								// remember that we've seen this in the group
								charRange = true;
								result.Append(sqlCharRange);
								break;
							}
						}
						case sqlNotList:
						{
							if(numBracketedCharacters == 1)
							{
								// need to escape this one
								result.Append(escape);
							}
							result.Append(patternChar);
							break;
						}
						default:
						{
							if(patternChar == escape)
							{
								result.Append(escape);
								result.Append(escape);
							}
							else
							{
								result.Append(patternChar);
							}
							break;
						}
					}
				}
				else
				{
					switch(patternChar)
					{
						case vbMany:
						{
							result.Append(sqlMany);
							break;
						}
						case vbSingle:
						{
							result.Append(sqlSingle);
							break;
						}
						case vbDigit:
						{
							result.Append(sqlDigit);
							break;
						}
						case vbOpenBracket:
						{
							// we're openning a bracketed group, so reset the group state
							bracketed = true;
							charRange = false;
							numBracketedCharacters = 0;
							result.Append(sqlOpenBracket);
							break;
						}
						// SQL's special characters need to be escaped
						case sqlMany:
						case sqlSingle:
						{
							result.Append(escape);
							result.Append(patternChar);
							break;
						}
						default:
						{
							if(patternChar == escape)
							{
								result.Append(escape);
								result.Append(escape);
							}
							else
							{
								result.Append(patternChar);
							}
							break;
						}
					}
				}
			}

			if(bracketed)
			{
				throw Error.VbLikeUnclosedBracket();
			}

			return result.ToString();
		}
	}
}