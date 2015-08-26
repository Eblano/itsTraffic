using UnityEngine;

using System.Collections.Generic;
using System.Linq;
using System.Xml;

public class TrafficLightIndex
{
	private static List<TrafficLightLogic> TrafficLights = new List<TrafficLightLogic>();
	private static Dictionary<TrafficLightLogic, Dictionary<TrafficLightIndex.RelationState, List<TrafficLightLogic>>> TrafficLightRelations = new Dictionary<TrafficLightLogic, Dictionary<TrafficLightIndex.RelationState, List<TrafficLightLogic>>> ();

	public static void AddTrafficLight (TrafficLightLogic trafficLight) {
		TrafficLights.Add (trafficLight);
	}

	public static void AutoInitTrafficLights () {
		// Set properties depending on rotation
		AutosetTrafficLightProperties ();
		// Index them to keep a map with each traffic light, and know which are in same crossing, oppose or on the sides
		BuildTrafficLightIndex ();
	}

	public static void ApplyConfig (XmlNode objectNode) {
		string id = objectNode.Attributes.GetNamedItem ("id").Value;
		TrafficLightLogic trafficLight = GetTrafficLightById (id);
		if (trafficLight) {
			foreach (XmlNode propertyNode in objectNode.ChildNodes) {
				switch (propertyNode.Name) {
					case "time": 
						float time = float.Parse (propertyNode.InnerText);
						ApplyTimeOnTrafficLight (trafficLight, time);
						break;
					case "state":
						TrafficLightLogic.State state = propertyNode.InnerText == "RED" ? TrafficLightLogic.State.RED : TrafficLightLogic.State.GREEN;
						ApplyStateOnTrafficLight (trafficLight, state);
						break;
				}
			}
		}
	}

	private static void AutosetTrafficLightProperties () {
		foreach (TrafficLightLogic trafficLight in TrafficLights) {
			if (trafficLight.getState () == TrafficLightLogic.State.NOT_INITIALISED) {
				// Get rotation, angles 45-135 & 225-315 should have same color, and the rest the other color
				float firstLightRotation = trafficLight.getRotation ();
				List<TrafficLightLogic> relatedTrafficLights = TrafficLights.Where (p => p.getPos () == trafficLight.getPos ()).ToList ();
				foreach (TrafficLightLogic relatedTrafficLight in relatedTrafficLights) {
					float lightRotation = Mathf.Abs((relatedTrafficLight.getRotation () - firstLightRotation) % 180f);
					TrafficLightLogic.State state = lightRotation > 45f && lightRotation <= 135f ? TrafficLightLogic.State.GREEN : TrafficLightLogic.State.RED;
					relatedTrafficLight.setState(state);
				}
			}
		}
	}

	private static void BuildTrafficLightIndex () {
		foreach (TrafficLightLogic trafficLight in TrafficLights) {
			foreach (TrafficLightLogic otherTrafficLight in TrafficLights) {
				if (trafficLight != otherTrafficLight && trafficLight.getPos ().Equals (otherTrafficLight.getPos())) {
					// This traffic light is either same light or opposite to it
					bool isSameDirection = trafficLight.getState () == otherTrafficLight.getState ();
					RelationState relationState = isSameDirection ? RelationState.SAME_DIRECTION : RelationState.CROSSING_DIRECTION;

					if (!TrafficLightRelations.ContainsKey (trafficLight)) {
						TrafficLightRelations.Add (trafficLight, new Dictionary<RelationState, List<TrafficLightLogic>> ());
					}
					Dictionary<RelationState, List<TrafficLightLogic>> trafficLightEntry = TrafficLightRelations [trafficLight];

					if (!trafficLightEntry.ContainsKey (relationState)) {
						trafficLightEntry.Add (relationState, new List<TrafficLightLogic>());
					}
					List<TrafficLightLogic> linkedTrafficLightsList = trafficLightEntry [relationState];

					linkedTrafficLightsList.Add (otherTrafficLight);
				}
			}
		}
	}

	private static TrafficLightLogic GetTrafficLightById (string id) {
		TrafficLightLogic trafficLight = null;
		foreach (TrafficLightLogic light in TrafficLights) {
			if (light.Id == id) {
				trafficLight = light;
				break;
			}
		}
		return trafficLight;
	}

	private static void ApplyTimeOnTrafficLight (TrafficLightLogic trafficLight, float time) {
		trafficLight.setTimeBetweenSwitches (time);
		Dictionary<RelationState, List<TrafficLightLogic>> trafficLightEntry = TrafficLightRelations [trafficLight];
		if (trafficLightEntry.ContainsKey (RelationState.SAME_DIRECTION)) {
			foreach (TrafficLightLogic sameDirectionLight in trafficLightEntry [RelationState.SAME_DIRECTION]) {
				sameDirectionLight.setTimeBetweenSwitches (time);
			}
			foreach (TrafficLightLogic otherDirectionLight in trafficLightEntry [RelationState.CROSSING_DIRECTION]) {
				otherDirectionLight.setTimeBetweenSwitches (time);
			}
		}
	}

	private static void ApplyStateOnTrafficLight (TrafficLightLogic trafficLight, TrafficLightLogic.State state) {
		TrafficLightLogic.State otherState = state == TrafficLightLogic.State.GREEN ? TrafficLightLogic.State.RED : TrafficLightLogic.State.GREEN;
		trafficLight.setState (state);
		Dictionary<RelationState, List<TrafficLightLogic>> trafficLightEntry = TrafficLightRelations [trafficLight];
		if (trafficLightEntry.ContainsKey (RelationState.SAME_DIRECTION)) {
			foreach (TrafficLightLogic sameDirectionLight in trafficLightEntry [RelationState.SAME_DIRECTION]) {
				sameDirectionLight.setState (state);
			}
			foreach (TrafficLightLogic otherDirectionLight in trafficLightEntry [RelationState.CROSSING_DIRECTION]) {
				otherDirectionLight.setState (otherState);
			}
		}
	}
	
	private enum RelationState {
		SAME_DIRECTION,
		CROSSING_DIRECTION
	}
}