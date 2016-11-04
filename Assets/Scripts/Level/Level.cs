﻿using System.Xml;

public class Level {

	public string id;
	public string name;
	public string fileUrl;
	public string iconUrl;
    public string countryCode;
    public float lon;
    public float lat;

	public string brief;
	public string randomSeedStr;
	public int randomSeed;
	public string timeOfDay;
	public string country;
	public string mapUrl;
	public string configUrl;

	public Objectives objectives;
	public PointCalculator pointCalculator;
    public VehicleColors vehicleColors;
	public Randomizer humanRandomizer;
	public Randomizer vehicleRandomizer;
	public Setup setup;

	public Level(XmlDocument xmlDoc) {
		XmlNode levelNode = xmlDoc.SelectSingleNode ("/level");
		extractLevelDetails (levelNode);

		XmlNode objectivesNode = xmlDoc.SelectSingleNode ("/level/objectives");
		objectives = new Objectives (objectivesNode);

        XmlNode pointCalculatorNode = xmlDoc.SelectSingleNode("/level/pointCalculator");
        pointCalculator = new PointCalculator(pointCalculatorNode);

		XmlNode vehicleColorsNode = xmlDoc.SelectSingleNode ("/level/vehicleColors");
        vehicleColors = new VehicleColors (vehicleColorsNode);

		XmlNode humanRandomizerNode = xmlDoc.SelectSingleNode ("/level/humanRandomizer");
		humanRandomizer = new Randomizer (humanRandomizerNode, "human");

		XmlNode vehicleRandomizerNode = xmlDoc.SelectSingleNode ("/level/vehicleRandomizer");
		vehicleRandomizer = new Randomizer (vehicleRandomizerNode, "vehicle");

		XmlNode setupNode = xmlDoc.SelectSingleNode ("/level/setup");
		setup = new Setup (setupNode);
	}

    public Level(XmlNode levelNode) {
        extractLevelDetails (levelNode);
    }

	private void extractLevelDetails(XmlNode levelNode) {
		XmlAttributeCollection levelAttributes = levelNode.Attributes;
		id = Misc.xmlString(levelAttributes.GetNamedItem ("id"));
		name = Misc.xmlString(levelAttributes.GetNamedItem ("name"));
        fileUrl = Misc.xmlString(levelAttributes.GetNamedItem ("fileUrl"));
        iconUrl = Misc.xmlString (levelAttributes.GetNamedItem ("iconUrl"));
        countryCode = Misc.xmlString (levelAttributes.GetNamedItem("countrycode"));
        lon = Misc.xmlFloat (levelAttributes.GetNamedItem("lon"));
        lat = Misc.xmlFloat (levelAttributes.GetNamedItem("lat"));

		brief = Misc.xmlString (levelAttributes.GetNamedItem ("brief"));
		randomSeedStr = Misc.xmlString(levelAttributes.GetNamedItem ("randomSeed"));
        if (randomSeedStr != null) {
            randomSeed = randomSeedStr.GetHashCode ();
//		    DebugFn.print ("Random seed: " + randomSeedStr + ", hash: " + randomSeed);
        }
		timeOfDay = Misc.xmlString(levelAttributes.GetNamedItem ("timeOfDay"));
		country = Misc.xmlString (levelAttributes.GetNamedItem ("country"));
		mapUrl = Misc.xmlString(levelAttributes.GetNamedItem ("mapUrl"));
		configUrl = Misc.xmlString(levelAttributes.GetNamedItem ("configUrl"));
	}
}
