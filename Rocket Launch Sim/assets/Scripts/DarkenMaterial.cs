using UnityEngine;
using System.Collections;

public class DarkenMaterial : MonoBehaviour {

	public Material DarkenSky;
	private float SkyColor = 0;
	private Transform myHeight;


	void Start(){
		myHeight = gameObject.GetComponent<Transform> ();
		SkyColor = 0;
	}

	void Update () {
		SkyColor = GetHeight ();
		DarkenSky.SetColor("_Color", new Vector4(0,0,0, SkyColor));
	}

	public float GetHeight(){
		float myHeight;
		myHeight = (gameObject.transform.position.y-19.77f)*.0005f;
		return myHeight;
	}
	void OnApplicationQuit(){
		DarkenSky.SetColor("_Color", new Vector4(0,0,0,0));
	}

}
