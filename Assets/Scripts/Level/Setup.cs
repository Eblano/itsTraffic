﻿using System.Collections;
using System.Xml;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Setup {

	public List<PersonSetup> referencePeople = new List<PersonSetup> ();
	public List<PersonSetup> people = new List<PersonSetup>();
	public List<VehicleSetup> vehicles = new List<VehicleSetup>();

	public Setup(XmlNode setupNode) {
        if (setupNode != null) {
			XmlNodeList personNodes = setupNode.SelectNodes("people/person");
			foreach (XmlNode personNode in personNodes) {
				PersonSetup person = createPerson (personNode);
				if (person.refOnly) {
					referencePeople.Add (person);
				} else {
					people.Add (person);
				}
			}

			XmlNodeList vehicleNodes = setupNode.SelectNodes("vehicles/vehicle");
			foreach (XmlNode vehicleNode in vehicleNodes) {
				vehicles.Add (createVehicle (vehicleNode));
			}

			referencePeople.Sort ((a, b) => a.id.CompareTo (b.id));
			people.Sort ((a, b) => a.time.CompareTo (b.time));
			vehicles.Sort ((a, b) => a.time.CompareTo (b.time));
        }
	}

	public Setup.PersonSetup getReferencePerson (long personId) {
		return referencePeople.Find (p => p.id == personId);
	}

	private PersonSetup createPerson (XmlNode personNode) {
		XmlAttributeCollection personAttributes = personNode.Attributes;
		long id = Misc.xmlLong(personAttributes.GetNamedItem ("id"));
		string name = Misc.xmlString(personAttributes.GetNamedItem ("name"));
		float time = Misc.xmlFloat(personAttributes.GetNamedItem ("time"));
		long startPos = Misc.xmlLong(personAttributes.GetNamedItem ("startPos"));
		long endPos = Misc.xmlLong(personAttributes.GetNamedItem ("endPos"));
		string startVector = Misc.xmlString (personAttributes.GetNamedItem ("startVector"));

		bool refOnly = Misc.xmlBool(personAttributes.GetNamedItem ("refOnly"));
		string dob = Misc.xmlString(personAttributes.GetNamedItem ("dob"));
		float money = Misc.xmlFloat(personAttributes.GetNamedItem ("money"));
		float speedFactor = Misc.xmlFloat (personAttributes.GetNamedItem ("speedFactor"));
		string shirtColor = Misc.xmlString (personAttributes.GetNamedItem ("shirtColor"));
		string skinColor = Misc.xmlString (personAttributes.GetNamedItem ("skinColor"));
		string country = Misc.xmlString (personAttributes.GetNamedItem ("country"));
        string specialIcon = Misc.xmlString( personAttributes.GetNamedItem( "specialIcon") );
        bool rerouteOK = Misc.xmlBool(personAttributes.GetNamedItem("rerouteOK"), true);

        List<long> wayPointIds = new List<long>();
        bool wayPointsLoop = parseWayPoints(personNode, wayPointIds);

        List<float> mood = Misc.parseFloats(Misc.xmlString(personAttributes.GetNamedItem("mood"), "1,0,1.5"));
        float angrySpeed = Misc.xmlFloat(personAttributes.GetNamedItem("angrySpeed"), 0.1f);
        float happySpeed = Misc.xmlFloat(personAttributes.GetNamedItem("happySpeed"), 0.05f);

        return new PersonSetup (id, name, time, startPos, endPos, startVector, rerouteOK, wayPointsLoop, wayPointIds, refOnly, dob, money, speedFactor, shirtColor, skinColor, country, specialIcon, mood, angrySpeed, happySpeed);
	}

	private VehicleSetup createVehicle (XmlNode vehicleNode) {
		XmlAttributeCollection vehicleAttributes = vehicleNode.Attributes;
		long id = Misc.xmlLong(vehicleAttributes.GetNamedItem ("id"));
		string name = Misc.xmlString(vehicleAttributes.GetNamedItem ("name"));
		float time = Misc.xmlFloat(vehicleAttributes.GetNamedItem ("time"));
		long startPos = Misc.xmlLong(vehicleAttributes.GetNamedItem ("startPos"));
		long endPos = Misc.xmlLong(vehicleAttributes.GetNamedItem ("endPos"));
		string startVector = Misc.xmlString (vehicleAttributes.GetNamedItem ("startVector"));

		string brand = Misc.xmlString(vehicleAttributes.GetNamedItem ("brand"));
		string model = Misc.xmlString(vehicleAttributes.GetNamedItem ("model"));
		string type = Misc.xmlString(vehicleAttributes.GetNamedItem ("type"));
		int year = Misc.xmlInt(vehicleAttributes.GetNamedItem ("year"));
		float distance = Misc.xmlFloat(vehicleAttributes.GetNamedItem ("distance"));
		float condition = Misc.xmlFloat(vehicleAttributes.GetNamedItem ("condition"), 1.0f);
		long driverId = Misc.xmlLong(vehicleAttributes.GetNamedItem ("driverId"));
		string passengerIdsStr = Misc.xmlString(vehicleAttributes.GetNamedItem ("passengerIds"));
		List<long> passengerIds = Misc.parseLongs (passengerIdsStr);
		float speedFactor = Misc.xmlFloat (vehicleAttributes.GetNamedItem ("speedFactor"));
		float acceleration = Misc.xmlFloat (vehicleAttributes.GetNamedItem ("acceleration"));
		float startSpeedFactor = Misc.xmlFloat (vehicleAttributes.GetNamedItem ("startSpeedFactor"));
		float impatientThresholdNonTrafficLight = Misc.xmlFloat (vehicleAttributes.GetNamedItem ("impatientThresholdNonTrafficLight"));
		float impatientThresholdTrafficLight = Misc.xmlFloat (vehicleAttributes.GetNamedItem ("impatientThresholdTrafficLight"));
		string color = Misc.xmlString (vehicleAttributes.GetNamedItem ("color"));
        bool rerouteOK = Misc.xmlBool(vehicleAttributes.GetNamedItem("rerouteOK"), true);
        string specialIcon = Misc.xmlString(vehicleAttributes.GetNamedItem( "specialIcon"));

        List<long> wayPointIds = new List<long>();
        bool wayPointsLoop = parseWayPoints(vehicleNode, wayPointIds);

        List<float> mood = Misc.parseFloats(Misc.xmlString(vehicleAttributes.GetNamedItem("mood"), "1,0,1.5"));
        float angrySpeed = Misc.xmlFloat(vehicleAttributes.GetNamedItem("angrySpeed"), 0.1f);
        float happySpeed = Misc.xmlFloat(vehicleAttributes.GetNamedItem("happySpeed"), 0.05f);

        bool startWithSiren = Misc.xmlBool(vehicleAttributes.GetNamedItem("startWithSiren"), false);
        bool fadeSiren = Misc.xmlBool(vehicleAttributes.GetNamedItem("fadeSiren"), false);

        return new VehicleSetup (id, name, time, startPos, endPos, startVector, rerouteOK, wayPointsLoop, wayPointIds, brand, model, type, year, distance, condition, driverId, passengerIds, speedFactor, acceleration, startSpeedFactor, impatientThresholdNonTrafficLight, impatientThresholdTrafficLight, color, specialIcon, mood, angrySpeed, happySpeed, startWithSiren, fadeSiren);
	}

    private bool parseWayPoints (XmlNode instanceNode, List<long> wayPointIds) {
        bool wayPointsLoop = false;
        XmlNode wayPointsNode = instanceNode.SelectSingleNode("wayPoints");
        if (wayPointsNode != null) {
            wayPointsLoop = Misc.xmlBool(wayPointsNode.Attributes.GetNamedItem("loop"), false);
            XmlNodeList singleWayPoints = wayPointsNode.SelectNodes("wayPoint");
            foreach (XmlNode wayPointNode in singleWayPoints) {
                wayPointIds.Add(Misc.xmlLong(wayPointNode.Attributes.GetNamedItem("id")));
            }
        }

        return wayPointsLoop;
    }

	public class InstanceSetup {
		public long id;
		public string name;
		public float time;
		public long startPos;
		public long endPos;
		public string startVector;
        public bool rerouteOK;
        public bool wayPointsLoop;
        public List<long> wayPoints;
        public string specialIcon;
        public List<float> mood;
        public float angrySpeed;
        public float happySpeed;

        private static List<float> DEFAULT_MOOD = new List<float>() {
            1f, 0f, 1.5f
        };

		public InstanceSetup(long id, string name, float time, long startPos, long endPos, string startVector, bool rerouteOK, bool wayPointsLoop, List<long> wayPoints, string specialIcon, List<float> mood, float angrySpeed, float happySpeed) {
			this.id = id;
			this.name = name;
			this.time = time;
			this.startPos = startPos;
			this.endPos = endPos;
			this.startVector = startVector;
            this.rerouteOK = rerouteOK;
            this.wayPointsLoop = wayPointsLoop;
            this.wayPoints = wayPoints;
			this.specialIcon = specialIcon;
            this.mood = mood;
            while (this.mood.Count != DEFAULT_MOOD.Count ) {
				this.mood.Add(DEFAULT_MOOD[this.mood.Count]);
			}
            this.angrySpeed = angrySpeed;
            this.happySpeed = happySpeed;
		}
	}

	public class PersonSetup : InstanceSetup {
		public bool refOnly;
		public string dob;
		public float money;
		public float speedFactor;
		public string shirtColor;
		public string skinColor;
		public string country;

		public PersonSetup(long id, string name, float time, long startPos, long endPos, string startVector, bool rerouteOK, bool wayPointsLoop, List<long> wayPoints, bool refOnly, string dob, float money, float speedFactor, string shirtColor, string skinColor, string country, string specialIcon, List<float> mood, float angrySpeed, float happySpeed) : base(id, name, time, startPos, endPos, startVector, rerouteOK, wayPointsLoop, wayPoints, specialIcon, mood, angrySpeed, happySpeed) {
			this.refOnly = refOnly;
			this.dob = dob;
			this.money = money;
			this.speedFactor = speedFactor;
			this.shirtColor = shirtColor;
			this.skinColor = skinColor;
			this.country = country;
		}
	}

	public class VehicleSetup : InstanceSetup {
		public string brand;
		public string model;
		public string type;
		public int year;
		public float distance;
		public float condition;
		public long driverId;
		public List<long> passengerIds;
		public float speedFactor;
		public float acceleration;
		public float startSpeedFactor;
		public float impatientThresholdNonTrafficLight;
		public float impatientThresholdTrafficLight;
		public string color;
        public bool startWithSiren;
        public bool fadeSiren;
        public long emergencyId = -1L;

		public VehicleSetup(long id, string name, float time, long startPos, long endPos, string startVector, bool rerouteOK, bool wayPointsLoop, List<long> wayPoints, string brand, string model, string type, int year, float distance, float condition, long driverId, List<long> passengerIds, float speedFactor, float acceleration, float startSpeedFactor, float impatientThresholdNonTrafficLight, float impatientThresholdTrafficLight, string color, string specialIcon, List<float> mood, float angrySpeed, float happySpeed, bool startWithSiren, bool fadeSiren) : base(id, name, time, startPos, endPos, startVector, rerouteOK, wayPointsLoop, wayPoints, specialIcon, mood, angrySpeed, happySpeed) {
			this.brand = brand;
			this.model = model;
			this.type = type;
			this.year = year;
			this.distance = distance;
			this.condition = condition;
			this.driverId = driverId;
			this.passengerIds = passengerIds;
			this.speedFactor = speedFactor;
			this.acceleration = acceleration;
			this.startSpeedFactor = startSpeedFactor;
			this.impatientThresholdNonTrafficLight = impatientThresholdNonTrafficLight;
			this.impatientThresholdTrafficLight = impatientThresholdTrafficLight;
			this.color = color;
            this.startWithSiren = startWithSiren;
            this.fadeSiren = fadeSiren;
		}
	}
}
