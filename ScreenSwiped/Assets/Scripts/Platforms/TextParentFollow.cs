﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextParentFollow : MonoBehaviour {
	public GameObject parent;
	public float xOffset;
	private Vector3 prevPosition;
	void Awake(){
		if(parent != null){
			prevPosition = parent.transform.position;
		} else{
			prevPosition = new Vector3(10, 10, 10);
		}
	}
	void Update(){
		if(parent!=null){
			Vector3 pos = parent.transform.position;
			int zIndex = 5;
			DragHandler script = parent.GetComponent<DragHandler>();
			if(script!=null && script.clicked)
				zIndex = 4;
			pos = new Vector3(pos.x - xOffset, pos.y, zIndex);
			if(pos != prevPosition){
				Vector3 point = Camera.main.WorldToScreenPoint(pos);
				transform.position = point;
				prevPosition = pos;
			}
		}
	}
}
