﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using EasyAR;

public class TuoKaHandler : MonoBehaviour {

	private GameObject cameraDevice;
	private GameObject renderCamera;
	private GameObject augmenter;
	private GameObject switchBtn;
	public GameObject imgTarget;
	private GameObject scanLine;
	private Vector3 prefabOriginPosition;
	private Quaternion prefabOriginRotation;
	private Vector3 prefabOriginScale;

	public bool isOn = false;
	public float scaleRate = 0.25f;
//	public Vector2 moveRate = new Vector2(0.001f, 0.003f);

	private string scanId = "";
	private string oldScanId = "";

	void Awake () 
	{
		cameraDevice = GameObject.Find("CameraDevice");
		renderCamera = GameObject.Find("RenderCamera");
		augmenter = GameObject.Find ("Augmenter");
	}

	// Use this for initialization
	void Start () {
		scanLine = GameObject.FindGameObjectWithTag ("ScanLine");

		switchBtn = GameObject.FindGameObjectWithTag ("TuoKa");

//		switchBtn.GetComponent<RectTransform>().localPosition = new Vector3 (Screen.width / 2f - 40f, -120f, 0f);

//		AttachGyro();
	}
	
	// Update is called once per frame
	void Update () {
		scanId = cameraDevice.GetComponent<EasyBarCodeScanner> ().info.scanId;
		if (scanId != "" && scanId != oldScanId) {
			oldScanId = scanId;
			switchBtn.GetComponent<RectTransform>().sizeDelta = new Vector2 (80f, 80f);
			imgTarget = GameObject.FindGameObjectWithTag ("ImgTarget");
		} else if (scanId == "") {
			switchBtn.GetComponent<RectTransform>().sizeDelta = new Vector2 (0f, 0f);
		}
	}

	public void TuoKaBtnClicked() {
		isOn = !isOn;
		if (isOn) {
			switchBtn.GetComponent<UnityEngine.UI.Image> ().material = Resources.Load ("Materials/Off") as Material;
			TuoKa ();
		} else {
			switchBtn.GetComponent<UnityEngine.UI.Image> ().material = Resources.Load ("Materials/On") as Material;
			TuoKaBack ();
		}
	}

	void TuoKa() {
		GameObject.FindGameObjectWithTag("Scanner").transform.Find("ScaleView").gameObject.SetActive (true);
		scaleRate = 1f;
//		moveRate = new Vector2(0.001f, 0.003f);
		// target set active；hide scan line
//		augmenter.GetComponent<AugmenterBehaviour> ().WorldCenter = AugmenterBaseBehaviour.CenterMode.Augmenter;
		GameObject imageTargetRoot = GameObject.FindGameObjectWithTag("ImageTargetRoot");
		imgTarget = imageTargetRoot.transform.Find ("ImageTarget").gameObject;
		imgTarget.SetActive (true);
		GameObject prefab = GameObject.FindGameObjectWithTag ("OldPrefab");
		prefabOriginPosition = prefab.transform.localPosition;
		prefabOriginRotation = prefab.transform.localRotation;
		prefabOriginScale = prefab.transform.localScale;
		Vector3 newPosition = GameObject.FindGameObjectWithTag ("MainCamera").transform.localPosition + new Vector3 (0, -100, 0);
		prefab.transform.localPosition = newPosition;
		prefab.transform.localScale = new Vector3 (prefab.transform.localScale.x * 100, prefab.transform.localScale.y * 100, prefab.transform.localScale.x * 100);
		// 添加触摸事件的脚本
//		prefab.AddComponent<TouchEventHandler>();
		scanLine.SetActive (false);
		renderCamera.GetComponent<GyroHandler> ().AttachGyro ();
	}

	void TuoKaBack() {
		GameObject.FindGameObjectWithTag("ScaleView").SetActive (false);
		scaleRate = 0.2f;
//		moveRate = new Vector2 (0.1f, 0.3f);
		// target set deactive；
//		augmenter.GetComponent<AugmenterBehaviour> ().WorldCenter = AugmenterBaseBehaviour.CenterMode.Target;
		GameObject prefab = GameObject.FindGameObjectWithTag ("OldPrefab");
		prefab.transform.localPosition = prefabOriginPosition;
		prefab.transform.localRotation = prefabOriginRotation;
		prefab.transform.localScale = prefabOriginScale;
//		Destroy (prefab.GetComponent<TouchEventHandler> ());
		imgTarget.SetActive (false);
		scanLine.SetActive (true);
		renderCamera.GetComponent<GyroHandler> ().DetachGyro ();
		renderCamera.transform.rotation = new Quaternion (0, 0, 0, 0);
		renderCamera.transform.localPosition = new Vector3 (0, 0, 0);
	}
}
