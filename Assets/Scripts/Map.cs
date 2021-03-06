using System.Collections.Generic;

public class Map {
	public static List<Pos> Nodes { set; get; }
	public static List<Way> Ways { set; get; }
	public static Dictionary<long, Way> WayIndex = new Dictionary<long, Way> ();

	static Map () {
		Ways = new List<Way> ();
	}

    public static void Clear() {
        Nodes.Clear();
        Ways.Clear();
        WayIndex.Clear();
    }
}
