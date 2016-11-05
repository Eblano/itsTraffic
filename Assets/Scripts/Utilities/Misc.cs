﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Xml;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

public class Misc {

	public static System.Random random = new System.Random();

	public static byte[] GetBytes(string str)
	{
		byte[] bytes = new byte[str.Length * sizeof(char)];
		System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
		return bytes;
	}
	
	public static string GetString(byte[] bytes)
	{
		char[] chars = new char[bytes.Length / sizeof(char)];
		System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
		return new string(chars);
	}

	private static DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
	public static long currentTimeMillis()
	{
		return (long) ((DateTime.UtcNow - Jan1st1970).TotalMilliseconds);
	}

	public static List<T> CloneBaseNodeList<T> (List<T> list) where T: MonoBehaviour {
		return new List<T> (list);
	}

	public static List<GameObject> NameStartsWith(string start) {
		List<GameObject> matches = new List<GameObject> ();
		GameObject[] gameObjects = GameObject.FindObjectsOfType (typeof(GameObject)) as GameObject[];
		foreach (GameObject gameObject in gameObjects){
			if (gameObject.name.StartsWith(start)) {
				matches.Add (gameObject);
			}
		}
		return matches;
	}

	public static Vector3 getWorldPos (XmlNode xmlNode) {
		Pos pos = NodeIndex.nodes [Convert.ToInt64 (xmlNode.Value)];
		return Game.getCameraPosition (pos);
	} 

	public static bool isAngleAccepted (float angle1, float angle2, float acceptableAngleDiff, float fullAmountDegrees = 360f) {
		float angleDiff = Mathf.Abs (angle1 - angle2);
		return angleDiff <= acceptableAngleDiff || angleDiff >= fullAmountDegrees - acceptableAngleDiff;
	}

	public static float kmhToMps (float speedChangeKmh)
	{
		return speedChangeKmh * 1000f / 3600f;
	}

	public static T DeepClone<T>(T original)
	{
		// Construct a temporary memory stream
		MemoryStream stream = new MemoryStream();

		// Construct a serialization formatter that does all the hard work
		BinaryFormatter formatter = new BinaryFormatter();

		// This line is explained in the "Streaming Contexts" section
		formatter.Context = new StreamingContext(StreamingContextStates.Clone);

		// Serialize the object graph into the memory stream
		formatter.Serialize(stream, original);

		// Seek back to the start of the memory stream before deserializing
		stream.Position = 0;

		// Deserialize the graph into a new set of objects
		// and return the root of the graph (deep copy) to the caller
		return (T)(formatter.Deserialize(stream));
	}

	public static List<Vector3> posToVector3(List<Pos> positions) {
		List<Vector3> vectors = new List<Vector3> ();

		foreach (Pos pos in positions) {
			vectors.Add (Game.getCameraPosition (pos));
		}

		return vectors;
	}

	public static string pickRandom (List<string> strings) {
		return strings [Misc.randomRange (0, strings.Count-1)];
	}

	public static Texture2D MakeTex(int width, int height, Color col)
	{
		Color[] pix = new Color[width * height];
		for(int i = 0; i < pix.Length; ++i)
		{
			pix[i] = col;
		}
		Texture2D result = new Texture2D(width, height);
		result.SetPixels(pix);
		result.Apply();
		return result;
	}

	public static object getMoney (float money) {
		// TODO - Currency for current country - Taken from map info on country
		return "$" + Mathf.Round(money * 100f) / 100f;
	}

	public static object getDistance (float distance) {
		// TODO - Maybe in US weird mesurements if in USA
		return Mathf.FloorToInt(distance) + "m";
	}

	public static bool isInside (Vector2 pos, Rect rect) {
		return rect.Contains (pos);
	}

	public static long daysToTicks (int days) {
		return (long) days * 24 * 60 * 60 * 1000 * 1000 * 10;
	}

	public static float getDistance (Vector3 from, Vector3 to) {
		return (from - to).magnitude; 
	}

	//Breadth-first search
	public static Transform FindDeepChild(Transform aParent, string aName) {
		var result = aParent.Find(aName);
		if (result != null) {
			return result;
		}
		foreach(Transform child in aParent) {
			result = Misc.FindDeepChild(child, aName);
			if (result != null)
				return result;
		}
		return null;
	}

    // Get child to a GameObject with tag
    public static GameObject GetGameObjectWithMenuValue(Transform parent) {
        GameObject gameObjectWithTag = null;

        // Get all MenuValue GameObjects
        MenuValue[] menuValueObjects = Resources.FindObjectsOfTypeAll<MenuValue>();

        // Check which one has the sent in parent
        foreach (MenuValue menuValueObject in menuValueObjects) {
            GameObject gameObject = menuValueObject.gameObject;
            if (Misc.HasParent(gameObject, parent)) {
                gameObjectWithTag = gameObject;
                break;
            }
        }


        return gameObjectWithTag;
    }

    public static bool HasParent(GameObject gameObject, Transform parent) {
        Transform transform = gameObject.transform;
        while (transform != null) {
            if (transform == parent) {
                return true;
            }
            transform = transform.parent;
        }

		return false;
    }

	// Convert string with comma separated longs to list of longs
	public static List<long> parseLongs (string passengerIdsStr, char separator = ',') {
		List<long> ids = new List<long> ();
		if (passengerIdsStr != null) {
			string[] idStrings = passengerIdsStr.Split (separator);
			foreach (string id in idStrings) {
				ids.Add (Convert.ToInt64 (id));
			}
		}
		return ids;
	}

	public static List<List<long>> parseLongMultiList (string intStrings, char listSeparator, char itemSeparator) {
		List<List<long>> multiList = new List<List<long>> ();
		string[] lists = intStrings.Split (listSeparator);
		foreach (string list in lists) {
			multiList.Add (Misc.parseLongs(list, itemSeparator));
		}

		return multiList;
	}

	public static string xmlString(XmlNode attributeNode, string defaultValue = null) {
		if (attributeNode != null) {
			return attributeNode.Value;
		}
		return defaultValue;
	}

	public static bool xmlBool(XmlNode attributeNode, bool defaultValue = false) {
		string strVal = Misc.xmlString (attributeNode);
		return strVal == "true" ? true : defaultValue;
	}

	public static int xmlInt(XmlNode attributeNode, int defaultValue = 0) {
		string strVal = Misc.xmlString (attributeNode);
		if (strVal != null) {
			return Convert.ToInt32 (strVal);
		}
		return defaultValue;
	}

	public static float xmlFloat(XmlNode attributeNode, float defaultValue = 0f) {
		string strVal = Misc.xmlString (attributeNode);
		if (strVal != null) {
			return Convert.ToSingle (strVal);
		}
		return defaultValue;
	}

	public static long xmlLong(XmlNode attributeNode, long defaultValue = 0L) {
		string strVal = Misc.xmlString (attributeNode);
		if (strVal != null) {
			return Convert.ToInt64 (strVal);
		}
		return defaultValue;
	}
		
	public static void setRandomSeed (int randomSeed) {
		Misc.random = new System.Random (randomSeed);
	}

	public static float randomRange(float min, float max) {
		double value = Misc.random.NextDouble ();
		return min + (max - min) * (float)value;
	}

	public static int randomRange(int min, int max) {
		return Misc.random.Next (min, max);
	}

	public static object randomTime () {
		return Misc.randomRange (0, 23) + ":" + Misc.randomRange (0, 59);
	}

	public static DateTime parseDate (string dob) {
		string[] dateParts = dob.Split ('-');
		return new DateTime (Convert.ToInt32 (dateParts [0]), Convert.ToInt32 (dateParts [1]), dateParts.Length > 2 ? Convert.ToInt32 (dateParts [2]) : 1);
	}

	public static Color parseColor (string color) {
		string[] colorParts = color.Split (',');
		float r = Convert.ToInt32 (colorParts [0]) / 255f;
		float g = Convert.ToInt32 (colorParts [1]) / 255f;
		float b = Convert.ToInt32 (colorParts [2]) / 255f;

		return new Color (r, g, b);
	}

	public static Vector3 parseVector (string startVector) {
		string[] xy = startVector.Split (',');
		return new Vector3 (Convert.ToSingle(xy[0]), Convert.ToSingle(xy[1]), 0);
	}

	public static Tuple2<float, float> getOffsetPctFromCenter (Vector3 zoomPoint) {
		float centerX = Screen.width / 2f;
		float centerY = Screen.height / 2f;

		float offsetX = zoomPoint.x - centerX;
		float offsetY = zoomPoint.y - centerY;

		return new Tuple2<float, float> (offsetX / centerX, offsetY / centerY);
	}

	public static AudioListenerHolder FindAudioListenerHolder(GameObject gameObject) {
        AudioListenerHolder audioListenerHolder = gameObject.GetComponent<AudioListenerHolder> ();
        if (audioListenerHolder != null) {
            return audioListenerHolder;
        }
        for (int child = 0; child < gameObject.transform.childCount; child++) {
            audioListenerHolder = Misc.FindAudioListenerHolder(gameObject.transform.GetChild(child).gameObject);
            if (audioListenerHolder != null) {
                return audioListenerHolder;
            }
        }
        return null;
	}

	public static IEnumerator _WaitForRealSeconds(float aTime) {
		while (aTime > 0f) {
			aTime -= Mathf.Clamp (Time.unscaledDeltaTime, 0, 0.2f);
            yield return null;
		}
	}

	public static Coroutine WaitForRealSeconds(float aTime) {
		Game gameSingleton = Singleton<Game>.Instance;
        return gameSingleton.StartCoroutine (_WaitForRealSeconds (aTime));
	}

    public static Vector3 getWorldPos(Transform transform) {
        Vector3 worldPosition = transform.localPosition;
        while (transform.parent != null) {
            transform = transform.parent;
            worldPosition += transform.localPosition;
        }
        return worldPosition;
    }

    public static Quaternion getWorldRotation(Transform transform) {
        return transform.rotation;
//        Quaternion worldRotation = transform.localRotation;
//        while (transform.parent != null) {
//            transform = transform.parent;
//            worldRotation += transform.localRotation;
//        }
//        return worldRotation;
    }

    public static string maxDecimals(float value, int decimals = 2) {
        if (value != 0f) {
            return value.ToString("#." + getDecimalSpots(decimals));
        } else {
            return value.ToString("0." + getDecimalSpots(decimals));
        }
    }

    private static string getDecimalSpots(int decimals) {
        string decimalChars = "";
        for (int i = 0; i < decimals; i++) {
            decimalChars += "#";
        }
        return decimalChars;
    }

    public class Size {
        public int width;
        public int height;
    }

	public static Size getImageSize(int width, int height, int targetWidth, int targetHeight) {
        Size size = new Size ();
		float ratioX = (float) width / targetWidth;
		float ratioY = (float) height/ targetHeight;
        if (ratioX > 1f || ratioY > 1f) {
			float scaleFactor = Mathf.Max(ratioX, ratioY);
            size.width = Mathf.RoundToInt(targetWidth / scaleFactor);
            size.height = Mathf.RoundToInt(targetHeight / scaleFactor);
        } else {
            size.width = width;
            size.height = height;
        }
		return size;
	}

	public static AudioListener getAudioListener() {
        AudioListener[] audioListener = Resources.FindObjectsOfTypeAll<AudioListener>();
        if (audioListener != null && audioListener.Length > 0) {
            return audioListener[0];
        }
        return null;
	}

	public static MeshFilter[] FilterCarWays(MeshFilter[] allWayFilters) {
        List<MeshFilter> filtered = new List<MeshFilter> ();
        foreach (MeshFilter wayFilter in allWayFilters) {
            if (!(wayFilter.name.StartsWith("CarWay (") || wayFilter.name.StartsWith("NonCarWay ("))) {
                filtered.Add(wayFilter);
            }
        }
		return filtered.ToArray();
	}

    public static float ToRadians(float degrees) {
        return (Mathf.PI / 180f) * degrees;
    }

    public static float ToDegrees(float radians) {
        return 180f * radians / Mathf.PI;
    }

    public static float getDistanceBetweenEarthCoordinates (float lon1, float lat1, float lon2, float lat2) {
        float R = 6371e3f; // metres
		float φ1 = ToRadians (lat1);
		float φ2 = ToRadians (lat2);
		float Δφ = ToRadians (lat2 - lat1);
		float Δλ = ToRadians (lon2 - lon1);

		float a = Mathf.Sin (Δφ / 2f) * Mathf.Sin (Δφ / 2f) +
			Mathf.Cos (φ1) * Mathf.Cos (φ2) *
			Mathf.Sin (Δλ / 2f) * Mathf.Sin (Δλ / 2f);
        float c = 2f * Mathf.Atan2(Mathf.Sqrt(a), Mathf.Sqrt(1f-a));

        return R * c;
    }

	public static List<int> splitInts(string str) {
		return str.Split (',').Select<string, int>(int.Parse).ToList<int>();
	}

	public static Texture getCountryFlag(string countryCode) {
        return Resources.Load("Graphics/flags/" + countryCode) as Texture;
	}

    // Readable in format "1s", "5m 30s" (always number + suffix grouped, parts separated with spaces)
	public static long getTsForReadable(string readable) {
        long ms = 0;
        string[] parts = readable.Split(' ');

		foreach (string part in parts) {
            if (part.EndsWith("ms")) {
                ms += long.Parse(part.Substring(0, part.Length - 2));
            } else if (part.EndsWith("s")) {
                ms += long.Parse(part.Substring(0, part.Length - 1)) * 1000;
            } else if (part.EndsWith("m")) {
                ms += long.Parse(part.Substring(0, part.Length - 1)) * 1000 * 60;
            } else if (part.EndsWith("h")) {
                ms += long.Parse(part.Substring(0, part.Length - 1)) * 1000 * 60 * 60;
            } else if (part.EndsWith("d")) {
                ms += long.Parse(part.Substring(0, part.Length - 1)) * 1000 * 60 * 60 * 24;
            }
        }

        return ms;
	}

//	https://github.com/fiorix/freegeoip/releases (need https://golang.org?)
    public static IEnumerator getGeoLocation() {
		WWW www = CacheWWW.Get(Game.instance.endpointBaseUrl + Game.instance.getLocationRelativeUrl, Misc.getTsForReadable("5m"));
        yield return www;

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(www.text);

        Game.instance.lat = Convert.ToSingle (xmlDoc.SelectSingleNode ("/geoData/lat").InnerText);
        Game.instance.lon = Convert.ToSingle (xmlDoc.SelectSingleNode ("/geoData/lon").InnerText);
    }
}
