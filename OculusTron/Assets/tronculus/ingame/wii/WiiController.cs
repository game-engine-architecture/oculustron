using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;
 
public class WiiController : MonoBehaviour {
	
	public enum NavDirection {LEFT, RIGHT, MIDDLE, NOTHING};
 
	[DllImport ("UniWii")]
	private static extern void wiimote_start();
 
	[DllImport ("UniWii")]
	private static extern void wiimote_stop();
 
	[DllImport ("UniWii")]
	private static extern int wiimote_count();
	
	[DllImport ("UniWii")]
	private static extern float wiimote_getPitch(int which);
	
	
	
	public bool wiimoteIsEnabled;
	private String display;
	private NavDirection localNavDirection;
	public NavDirection navDirection; 
	
	void Update () {	
		int c = wiimote_count();
	
		if (wiimoteIsEnabled){
			
			
        if (c>0) {
            	display = "";
            	for (int i=0; i<=c; i++) {
                	float p = Mathf.Round(wiimote_getPitch(i));
					if (i==0){
						NavDirection tempDirection = getDirection(p);
						if (tempDirection!=NavDirection.NOTHING){
							if ((localNavDirection==NavDirection.MIDDLE)&&(tempDirection!=NavDirection.MIDDLE)){
								navDirection = tempDirection;
								localNavDirection = tempDirection;
							}else if (tempDirection==NavDirection.MIDDLE){
								navDirection = tempDirection;
								localNavDirection = tempDirection;
							}
						}
						
						//display += "navdirection: " + navDirection.ToString() + " " + p;
					}
            	}
        	}else display = "Press the '1' and '2' buttons on your Wii Remote.";
		}
	}
	
	private NavDirection getDirection(float p){
		/*rechts -110 (<-90)
		53
		mitte  -57 (<-30-&&>-75)
		94
		links 37 (>0)
		*/
		if (p>0){
			return NavDirection.LEFT;
		}else if((p<-30)&&(p>-75)){
			return NavDirection.MIDDLE;
		}else if(p<-90){
			return NavDirection.RIGHT;
		}else {
			return NavDirection.NOTHING;
		}
	}
	
	void OnGUI() {
		GUI.Label(new Rect(10,Screen.height-100, 500, 100), display);
	}
 
	void Start ()
	{
		localNavDirection = NavDirection.MIDDLE;
		wiimoteIsEnabled = true;
		wiimote_start();
	}
 
	void OnApplicationQuit() {
		wiimote_stop();
	}
 
}