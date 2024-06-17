using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace SamenessFinder
{
	internal class Program
	{
		public static void Main(string[] args)
		{
			string file = Console.ReadLine();

			string content = File.ReadAllText(file);
			JObject json = (JObject)JsonConvert.DeserializeObject(content);
			JToken items = json["Items"];

			Console.WriteLine($"Items {items.Count()}");
			JToken[] item = items.ToArray();
			List<SamenessTree> sameNames = new List<SamenessTree>();
			List<JToken> sameColors = new List<JToken>();
			foreach (JToken token in item)
			{
				JToken color = token["Color"];
				Color clr = GetColor(color);
				if (sameColors.All(s => GetColor(s) != clr))
				{
					sameNames.Add(new SamenessTree()
					{
						Main = token["Name"].ToString(), Branches = new List<string>()
					});
					sameColors.Add(color);
					Console.WriteLine($"Added {token["Name"]}");
				}
				else
				{
					JToken same = sameColors.First(s => GetColor(s) == clr);
					int index = Array.IndexOf(sameColors.ToArray(), same);

					sameNames[index].Branches.Add(token["Name"].ToString());
				}
			}

			Console.WriteLine($"Same {sameColors.Count}");
			foreach (SamenessTree sameName in sameNames)
			{
				Console.WriteLine($"Main {sameName.Main}, branches: {string.Join(", ", sameName.Branches)}");
			}

		}
		private static Color GetColor(JToken token)
		{
			return new Color
			{
				R = Convert.ToDouble(token["r"]), G = Convert.ToDouble(token["g"]), B = Convert.ToDouble(token["b"]), A = Convert.ToDouble(token["a"])
			};
		}
	}
}

public class SamenessTree
{
	public string Main;
	public List<string> Branches;
}

public struct Color
{
	public double R;
	public double G;
	public double B;
	public double A;

	private double[] Values => new[]
	{
		R, G,
		B, A
	};

	public static bool operator ==(Color c1, Color c2)
	{
		return c1.R == c2.R && c1.B == c2.B && c1.G == c2.G && c1.A == c2.A;
	}

	public static bool operator !=(Color c1, Color c2)
	{
		return !(c1 == c2);
	}
}