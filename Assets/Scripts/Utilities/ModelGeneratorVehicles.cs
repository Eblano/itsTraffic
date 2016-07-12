﻿using System.Collections.Generic;public class ModelGeneratorVehicles {

	public static string generate(string brand) {
		List<string> models;
		if (modelsForBrand.ContainsKey (brand)) {
			models = modelsForBrand [brand];
		} else {
			models = modelsForBrand [""];
		}
		return Misc.pickRandom (models);
	}

	private static Dictionary<string, List<string>> modelsForBrand = new Dictionary<string, List<string>> {
		{
			"Sportzcar", new List<string> {"Prancer", "3310", "Bolt", "Zolt", "ZX", "x86", "NES", "Oldsmobile", "Zpyder", "Camro"}
		}, {
			"Wolvo", new List<string> {"TI-81", "C64", "N64", "11-7", "X36.0", "X1", "X2000"}
		}, {
			"", new List<string> {""}
		}
	};
}