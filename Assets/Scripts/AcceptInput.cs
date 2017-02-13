using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcceptInput : MonoBehaviour {
	public float initialX;
	public float initialZ;
	public Material selectShader;
	public GameObject cursorRing;
	public Material cursorShader;
	
	public GameObject selectionRange;
	private bool selecting = false;
	private float xScale = 0.0f;
	private float zScale = 0.0f;
	private float xScaleMax = 10.0f;
	private float zScaleMax = 10.0f;
	private float xScaleDelta = .5f;
	private float zScaleDelta = .5f;
	private GameObject myCursor;
	
	private Camera myCam;
	// Use this for initialization
	void Start () {
		selectionRange = Instantiate<GameObject>(selectionRange);
		selectionRange.name = "obj_selector";
		selectionRange.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
		selectionRange.GetComponent<Renderer>().material = selectShader;
		selectionRange.transform.localScale = new Vector3(initialX, .001f, initialZ);
		selectionRange.SetActive(false);
		myCam = GetComponentInParent<Camera>();
		cursorRing.GetComponent<Renderer>().material = cursorShader;
		
		myCursor = Instantiate(cursorRing);
	}
	
	// Update is called once per frame
	void Update () {
		
		/* Selection radius expansion */
		if (selecting) {
			myCursor.SetActive(false);
			if (selectionRange.activeSelf == false) {
				selectionRange.SetActive(true);
			}
			selectionRange.transform.localScale = new Vector3(xScale, .001f, zScale);
			if (xScale < xScaleMax) {
				xScale+=xScaleDelta;
			}
			else {
				xScale = xScaleMax;
			}
			if (zScale < zScaleMax) {
				zScale+=zScaleDelta;
			}
			else {
				zScale = zScaleMax;
			}
		}
		else {
			myCursor.SetActive(true);
			if (selectionRange.activeSelf) {
				xScale = 0.0f;
				zScale = 0.0f;
				selectionRange.transform.localScale = new Vector3(xScale, .001f, zScale);
				selectionRange.SetActive(false);
			}
		}
		
		//Need to raycast from mouse to a point on the map to determine where
		//the selection circle appears
		RaycastHit pointer;
		Vector3 cursor = new Vector3(0.0f, 0.0f, 0.0f);
		Ray pointerStart = Camera.main.ScreenPointToRay(Input.mousePosition);
		int layerMask = 1 << 8;
		if (Physics.Raycast(pointerStart, out pointer, Mathf.Infinity, layerMask)) {
			cursor = pointer.point; 
			myCursor.transform.position = cursor;
			//myCursor.transform.localRotation = Quaternion.identity;
			/* Accept User Input */
			if (Input.GetMouseButton(0)) {
				selecting = true;
				selectionRange.transform.position = cursor;
			}
			else {
				selecting = false;
			}
		}
	}
	
}
