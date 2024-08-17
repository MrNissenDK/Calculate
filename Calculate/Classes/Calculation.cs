using System.Globalization;
using System.Text.RegularExpressions;

namespace Calculate.Classes
{
	public class Calculation
	{
		public string ToCalculate { get; set; }
		public List<string> Calculations { get; set; }
		public string Result { get; set; }
		public Calculation(string toCalculate)
		{
			ToCalculate = toCalculate;
		}

		public void Run()
		{
			ToCalculate = ToCalculate.Replace(" ", "").Replace(",", ".").Replace("−", "-").Replace("×", "*");
			Calculations ??= new List<string>();
			Calculations.Clear();
			Calculations.Add(ToCalculate);
			ToCalculate = Parentheses(ToCalculate);
			ToCalculate = Exponent(ToCalculate);
			ToCalculate = MultiplyDivide(ToCalculate);
			Result = AddSubtract(ToCalculate);
		}

		//PEMDAS
		private string Parentheses(string part)
		{
			var regex = new Regex(@"\([^()]*\)");
			while (regex.IsMatch(part))
			{
				var match = regex.Match(part);
				var startString = part.Substring(0, match.Index);
				var endString = part.Substring(match.Index + match.Length);
				var operate = match.Value.Substring(1, match.Value.Length - 2);
				operate = Exponent(operate, startString + "(", ")" + endString);
				operate = MultiplyDivide(operate, startString + "(", ")" + endString);
				operate = AddSubtract(operate, startString + "(", ")" + endString);
				part = startString + operate + endString;
			}
			return part;
		}

		private string Exponent(string part, string pre = "", string append = "")
		{
			var regex = new Regex(@"(-?[\d.]+)\^(-?[\d.]+)");
			while (regex.IsMatch(part))
			{
				var match = regex.Match(part);
				var value1 = double.Parse(match.Groups[1].Value);
				var value2 = double.Parse(match.Groups[2].Value);
				var result = Math.Pow(value1, value2);

				// Format the result to avoid scientific notation
				part = part.Substring(0, match.Index) + result.ToString("F", CultureInfo.InvariantCulture) + part.Substring(match.Index + match.Length);
				Calculations.Add(pre + part + append);
			}
			return part;
		}

		private string AddSubtract(string part, string pre = "", string append = "")
		{
			var regex = new Regex(@"(-?[\d.]+)([+-])(-?[\d.]+)");
			while (regex.IsMatch(part))
			{
				var match = regex.Match(part);
				var value1 = double.Parse(match.Groups[1].Value);
				var value2 = double.Parse(match.Groups[3].Value);
				var operation = match.Groups[2].Value;
				double result = 0;
				if (operation == "+")
				{
					result = value1 + value2;
				}
				else if (operation == "-")
				{
					result = value1 - value2;
				}

				// Format the result to avoid scientific notation
				part = part.Substring(0, match.Index) + result.ToString("F", CultureInfo.InvariantCulture) + part.Substring(match.Index + match.Length);
				Calculations.Add(pre + part + append);
			}
			return part;
		}

		private string MultiplyDivide(string part, string pre = "", string append = "")
		{
			var regex = new Regex(@"(-?[\d.]+)([\*\/])(-?[\d.]+)");
			while (regex.IsMatch(part))
			{
				var match = regex.Match(part);
				var value1 = double.Parse(match.Groups[1].Value);
				var value2 = double.Parse(match.Groups[3].Value);
				var operation = match.Groups[2].Value;
				double result = 0;
				if (operation == "*")
				{
					result = value1 * value2;
				}
				else if (operation == "/")
				{
					result = value1 / value2;
				}

				// Format the result to avoid scientific notation
				part = part.Substring(0, match.Index) + result.ToString("F", CultureInfo.InvariantCulture) + part.Substring(match.Index + match.Length);
				Calculations.Add(pre + part + append);
			}
			return part;
		}
	}
}
