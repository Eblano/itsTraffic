﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLightToggle : IPubSub {

	private static TrafficLightToggle instance = null;
	
	private static TrafficLightToggle getInstance() {
		if (instance == null) {
			instance = new TrafficLightToggle(); 
		}
		return instance;
	}

	public static void Start () {
		PubSub.subscribe ("Click", getInstance(), 110);
	}
	
	public static void Add (long posId, Vector2 center, float radius) {
		getInstance().toggles.Add (new CircleTouchWithPosId(posId, center, radius));
	}


	List<CircleTouchWithPosId> toggles = new List<CircleTouchWithPosId>();

	public PROPAGATION onMessage(string message, System.Object obj) {
//        Debug.Log("Click TrafficLightToggle");
        if (message == "Click") {
			Vector2 clickPos = Game.instance.screenToWorldPosInBasePlane((Vector3) obj);
			CircleTouchWithPosId touchArea = toggles.Find(i => i.isInside(clickPos));
			if (touchArea != null) {
				TrafficLightIndex.toggleLightsForPos(touchArea.posId);
//                return PROPAGATION.STOP_AFTER_SAME_TYPE;
                return PROPAGATION.DEFAULT;
			}
		}
        return PROPAGATION.CONTINUE_WITH_OTHER_TYPES;
	}

	private class CircleTouchWithPosId : CircleTouch {
		public long posId;

		public CircleTouchWithPosId(long posId, Vector2 center, float radius) : base(center, radius) {
			this.posId = posId;
		}
	}

    public static void Clear() {
        PubSub.unsubscribeAllForSubscriber (instance);
    }

}
