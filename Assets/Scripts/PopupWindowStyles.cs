﻿using UnityEngine;
using System.Collections;

public class PopupWindowStyles : MonoBehaviour {

	protected static GUIStyle borderStyle;
	protected static GUIStyle windowStyle;
	protected static GUIStyle titleStyle;
	protected static GUIStyle subtitleStyle;
	protected static GUIStyle subtitleStyleRight;
	protected static GUIStyle textStyle;
	protected static GUIStyle textNoPointsStyle;
	protected static GUIStyle linkStyle;
	protected static GUIStyle textStyleRight;
	protected static GUIStyle textNoPointsStyleRight;

    public static GUIStyle VERTICAL_SCROLLBAR_VISIBLE;
    public static GUIStyle HORIZONTAL_SCROLLBAR_VISIBLE;
    public static GUIStyle SCROLLBAR_NOT_VISIBLE;

    protected static Texture2D starFilled;
    protected static Texture2D starOutlined;
    protected static Texture2D highscoreStamp;
    protected static Texture2D failedStamp;

    void Awake() {
        starFilled = Resources.Load<Texture2D>("Graphics/filled_star");
        starOutlined = Resources.Load<Texture2D>("Graphics/outlined_star");
        highscoreStamp = Resources.Load<Texture2D>("Graphics/highscore_stamp");
        failedStamp = Resources.Load<Texture2D>("Graphics/failed_stamp");
    }

    private bool isStyleSetupRequired = true;

    public void OnGUI() {
        if (isStyleSetupRequired) {
            isStyleSetupRequired = false;
			if (windowStyle == null) {
				windowStyle = new GUIStyle(GUI.skin.box);
				windowStyle.normal.background = Misc.MakeTex(2, 2, new Color(0.3f, 0.3f, 0.3f, 0.8f));
			}
			if (borderStyle == null) {
				borderStyle = new GUIStyle(GUI.skin.box);
				borderStyle.normal.background = Misc.MakeTex(2, 2, new Color(0.1f, 0.1f, 0.1f, 0.9f));
			}
			if (titleStyle == null) {
				titleStyle = new GUIStyle();
				titleStyle.fontStyle = FontStyle.Bold;
				titleStyle.fontSize = 24;
				titleStyle.normal.textColor = Color.white;
			}
			if (subtitleStyle == null) {
				subtitleStyle = new GUIStyle();
				subtitleStyle.fontStyle = FontStyle.Bold;
				subtitleStyle.fontSize = 18;
				subtitleStyle.normal.textColor = Color.white;
			}
			if (textStyle == null) {
				textStyle = new GUIStyle();
				textStyle.fontSize = 16;
				textStyle.normal.textColor = Color.white;
			}
			if (textNoPointsStyle == null) {
				textNoPointsStyle = new GUIStyle();
				textNoPointsStyle.fontSize = 16;
				textNoPointsStyle.normal.textColor = Color.gray;
			}
			if (linkStyle == null) {
				linkStyle = new GUIStyle();
				linkStyle.fontSize = 16;
				linkStyle.normal.textColor = new Color(0.77f, 0.77f, 1f);
			}
			if (textStyleRight == null) {
				textStyleRight = new GUIStyle();
				textStyleRight.fontSize = 16;
				textStyleRight.normal.textColor = Color.white;
				textStyleRight.alignment = TextAnchor.MiddleRight;
			}
			if (textNoPointsStyleRight == null) {
				textNoPointsStyleRight = new GUIStyle();
				textNoPointsStyleRight.fontSize = 16;
				textNoPointsStyleRight.normal.textColor = Color.grey;
				textNoPointsStyleRight.alignment = TextAnchor.MiddleRight;
			}
			if (subtitleStyleRight == null) {
				subtitleStyleRight = new GUIStyle();
				subtitleStyleRight.fontStyle = FontStyle.Bold;
				subtitleStyleRight.fontSize = 18;
				subtitleStyleRight.normal.textColor = Color.white;
				subtitleStyleRight.alignment = TextAnchor.MiddleRight;
			}
			if (VERTICAL_SCROLLBAR_VISIBLE == null) {
				VERTICAL_SCROLLBAR_VISIBLE = GUI.skin.verticalScrollbar;
			}
			if (HORIZONTAL_SCROLLBAR_VISIBLE == null) {
				HORIZONTAL_SCROLLBAR_VISIBLE = GUI.skin.horizontalScrollbar;
			}
			if (SCROLLBAR_NOT_VISIBLE == null) {
				SCROLLBAR_NOT_VISIBLE = GUIStyle.none;
			}
		}
	}

}
