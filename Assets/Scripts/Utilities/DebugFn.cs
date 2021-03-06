using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class DebugFn
{
	private static bool enabled = true;
	private const float time = float.MaxValue;
//	private const float time = 10f;

	private static Color overrideColor;
	private static float overrideTime = -1f;

	private static Vector3 offsetZ = new Vector3 (0, 0, -0.1f);

    public static void print(Rect rect) {
        if (enabled) {
            Debug.Log("x: " + rect.x + ", y: " + rect.y + ", w: " + rect.width + ", h: " + rect.height);
        }
    }

	public static void print(Pos pos) {
		DebugFn.print (Game.getCameraPosition (pos));
	}

	public static void print(string message) {
		if (enabled) {
			Debug.Log (message);
		}
	}

	public static void print(Vector3 vector) {
		if (enabled) {
			Debug.Log (getVectorString(vector));
		}
	}

	public static void print(List<Vector3> vectors) {
		if (enabled) {
			Debug.Log ("[");
			foreach (Vector3 vector in vectors) {
				print (vector);
			}
			Debug.Log ("]");
		}
	}

	public static void print(Vector3[] vectors) {
		if (enabled) {
			print (vectors.ToList ());
		}
	}

    public static void print(List<DrivePath> drivePaths) {
        if (enabled) {
            Debug.Log("[");
            DrivePath prev = null;
            foreach (DrivePath curr in drivePaths) {
                if (prev == null) {
                    DebugFn.print(curr.startVector);
                }
				DebugFn.print(curr.endVector);
                prev = curr;
            }
            Debug.Log("]");
        }
    }

	public static void print(Vector2 vector) {
		if (enabled) {
			Debug.Log ("Vector2 (" + vector.x + ", " + vector.y + ")");
		}
	}
	
	public static void print(List<Vector2> vectors) {
		if (enabled) {
			Debug.Log ("[");
			foreach (Vector2 vector in vectors) {
				print (vector);
			}
			Debug.Log ("]");
		}
	}

	private static float X_LENGTH = 0.02f;

	public static void arrow(Vector3 start, Vector3 eulerAngles, Vector3 lengthVector) {
		Quaternion quaternion = Quaternion.Euler (eulerAngles);
		Vector3 move = quaternion * lengthVector;
		Vector3 end = start + move;
	
		arrowFromTo (start, end);
	}

	private static void arrowFromTo (Vector3 start, Vector3 end, float time = time) {
		if (enabled) {
			Debug.DrawLine (start + offsetZ + new Vector3 (-X_LENGTH, -X_LENGTH, 0f), start + offsetZ + new Vector3 (X_LENGTH, X_LENGTH, 0f), getColor(Color.yellow), getTime(time), false);
			Debug.DrawLine (start + offsetZ + new Vector3 (-X_LENGTH, X_LENGTH, 0f), start + offsetZ + new Vector3 (X_LENGTH, -X_LENGTH, 0f), getColor(Color.yellow), getTime(time), false);
			
			Debug.DrawLine (start + offsetZ, end + offsetZ, getColor(Color.yellow), getTime(time), false);
		}
	}

	public static void arrow(Pos start, Pos end) {
		arrow (Game.getCameraPosition (start), Game.getCameraPosition (end));
	}

    public static void arrows(List<Vector3> vectors) {
        for (int i = 1; i < vectors.Count; i++) {
            Vector3 prev = vectors[i-1];
            Vector3 curr = vectors[i];
            DebugFn.arrow(prev, curr);
        }
    }

	public static void arrow(Vector3 start, Vector3 end) {
		arrowFromTo (start, end);
	}

	public static void arrow(Vector2 start, Vector2 end) {
		arrowFromTo (new Vector3(start.x, start.y), new Vector3(end.x, end.y));
	}

	public static void square(Pos pos) {
		DebugFn.square (Game.getCameraPosition (pos));
	}

	public static void square(Vector3 pos, float time = time) {
		if (enabled) {
			Debug.DrawLine (pos + offsetZ + new Vector3 (-X_LENGTH, -X_LENGTH, 0f), pos + offsetZ + new Vector3 (X_LENGTH, -X_LENGTH, 0f), getColor(Color.yellow), getTime(time), false);
			Debug.DrawLine (pos + offsetZ + new Vector3 (X_LENGTH, -X_LENGTH, 0f), pos + offsetZ + new Vector3 (X_LENGTH, X_LENGTH, 0f), getColor(Color.yellow), getTime(time), false);
			Debug.DrawLine (pos + offsetZ + new Vector3 (X_LENGTH, X_LENGTH, 0f), pos + offsetZ + new Vector3 (-X_LENGTH, X_LENGTH, 0f), getColor(Color.yellow), getTime(time), false);
			Debug.DrawLine (pos + offsetZ + new Vector3 (-X_LENGTH, X_LENGTH, 0f), pos + offsetZ + new Vector3 (-X_LENGTH, -X_LENGTH, 0f), getColor(Color.yellow), getTime(time), false);

			Debug.DrawLine (pos + offsetZ + new Vector3 (-X_LENGTH, -X_LENGTH, 0f), pos + offsetZ + new Vector3 (X_LENGTH, X_LENGTH, 0f), getColor(Color.yellow), getTime(time), false);
			Debug.DrawLine (pos + offsetZ + new Vector3 (-X_LENGTH, X_LENGTH, 0f), pos + offsetZ + new Vector3 (X_LENGTH, -X_LENGTH, 0f), getColor(Color.yellow), getTime(time), false);
		}
	}

	public static void DrawBounds(Bounds bounds, float time = time) {
		if (enabled) {
			Debug.DrawLine (bounds.center - bounds.size / 2f, bounds.center + bounds.size / 2f, getColor(Color.green), getTime(time), false);
			Debug.DrawLine (bounds.center - new Vector3 (bounds.size.x, -bounds.size.y, bounds.size.z) / 2f, bounds.center + new Vector3 (bounds.size.x, -bounds.size.y, bounds.size.z) / 2f, getColor(Color.green), getTime(time), false);
		}
	}

	public static void DrawOutline (List<Vector2> points) {
		Vector2 prev = points.Last ();
		foreach (Vector2 curr in points) {
			arrow(prev, curr);
			prev = curr;
		}
	}

	public static void DebugPath (List<Pos> path) {
		Pos prev = null;
		foreach (Pos pos in path) {
			if (prev != null) {
				DebugFn.arrow (prev, pos);
			}
			prev = pos;
		}
	}

	public static void DebugPath (List<Vector3> path) {
		for (int i = 1; i < path.Count; i++) {
			Vector3 prev = path[i - 1];
			Vector3 pos = path [i];
			DebugFn.arrow (prev, pos);
		}
	}

	public static void temporaryOverride (Color color, float time = 0f) {
		if (time > 0f) {
			DebugFn.overrideColor = color;
			DebugFn.overrideTime = time;
		} else {
			DebugFn.overrideTime = 0f;
		}
	}

	private static float getTime (float time) {
		return DebugFn.overrideTime > 0f ? DebugFn.overrideTime : time;
	}

	static Color getColor (Color color) {
		return DebugFn.overrideTime > 0f ? DebugFn.overrideColor : color;
	}

	public static string getVectorString(Vector3 vector) {
		return "Vector3 (" + vector.x + ", " + vector.y + ", " + vector.z + ")";
	}
}
