﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePlatform : MonoBehaviour {
	public GameObject[] platforms;
	// static platform after being placed at certain position
	public float[] cooldowns;
	// cooldowns of platform types
	public GameObject[] heldVersions;
	// version of platform that uses script to follow mouse
	private GameObject heldObj;
	// used to destroy instantiated object following mouse
	private bool holding = false;
	// are we holding the object
	private HUD hudScript;
	private int currPlatformNum;
	private Dictionary<KeyCode, int> keys = new Dictionary<KeyCode, int>();
	private bool colorSet = false;
	private Dictionary<int, bool> cooled = new Dictionary<int, bool>();
	void Start(){
		hudScript = Camera.main.GetComponent<HUD>();
		int num = 1;
		keys.Add(KeyCode.Alpha1, num);
		cooled.Add(num, true);
		num++;
		keys.Add(KeyCode.Alpha2, num);
		cooled.Add(num, true);
		num++;
		keys.Add(KeyCode.Alpha3, num);
		cooled.Add(num, true);
	}
	void Update(){
		Vector3 mouseLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mouseLocation.z = 0;
		bool buttonClicked = false;
		foreach(KeyValuePair<KeyCode, int> key in keys){
    		if(Input.GetKeyDown(key.Key) && !colorSet && cooled[key.Value]){
				buttonClicked = true;
				// initialize platform at mouse location
				if(heldObj){
					Destroy(heldObj);
				}
				currPlatformNum = key.Value;
				holding = true;
				heldObj = Instantiate(heldVersions[currPlatformNum - 1], mouseLocation, Quaternion.identity);
				hudScript.ChangeColor(currPlatformNum, true);
				colorSet = true;
				break;
			}
		}
		if(Input.GetKeyDown(KeyCode.Escape) && holding && !buttonClicked){
			// stop holding object and remove it from screen
			Destroy(heldObj);
			heldObj = null;
			holding = false;
			hudScript.ChangeColor(currPlatformNum, false);
			colorSet = false;
		} else if(Input.GetMouseButtonDown(0) && holding && !buttonClicked){
			// if holding, object follows mouse until we instantiate it with spacebar
			FollowMouse script = heldObj.GetComponent<FollowMouse>();
			float rotation = script.rotation;
			Color col = script.currColor;
			if(col == Color.red){
				// do not place platform if we are inside player
				return;
			}
			Destroy(heldObj);
			heldObj = null;
			GameObject currPlatform = Instantiate(platforms[currPlatformNum - 1], mouseLocation, Quaternion.identity);
			// instantiate actual static platform at location
			PlatformSet newScript = currPlatform.GetComponent<PlatformSet>();
			newScript.RotateTo(rotation);
			holding = false;
			colorSet = false;
			float cooldown = cooldowns[currPlatformNum - 1];
			cooled[currPlatformNum] = false;
			hudScript.CoolDown(currPlatformNum, cooldown, Time.time);
			StartCoroutine(CoolDown(cooldown, currPlatformNum));
		}
	}
	IEnumerator CoolDown(float cooldown, int currPlatformNum){
		yield return new WaitForSeconds(cooldown);
		// wait for platform cooldown, then allow player to use it again
		cooled[currPlatformNum] = true;
	}
}
