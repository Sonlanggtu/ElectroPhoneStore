using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace eShopSolution.Utilities.Constants
{
	public class Helper
	{
		public static string RemoveDiacritics(string text)
		{
			if (string.IsNullOrEmpty(text))
				return text;

			var normalizedString = text.Normalize(NormalizationForm.FormD);
			var stringBuilder = new StringBuilder();

			foreach (var c in normalizedString)
			{
				var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);

				if (c == 'đ')
				{
					stringBuilder.Append('d');
				}
				else if (c == 'Đ')
				{
					stringBuilder.Append('D');
				}
				else if (unicodeCategory != UnicodeCategory.NonSpacingMark)
				{
					stringBuilder.Append(c);
				}
			}

			return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
		}


		

	}
}
