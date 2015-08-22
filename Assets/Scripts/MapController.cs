using UnityEngine;
using System ; 
using Parse ; 
using System.Collections;
using System.Collections.Generic;
using Boomlagoon.JSON;



public class MapController : MonoBehaviour {
	public GameObject obj ;
	public bool isPrepared ; 
	// Use this for initialization
	public IEnumerable<BuildingData> buildings ; 
	void Start () {

		StartCoroutine( DB.getAllBuildingInfo (obj,buildingDataCallback));
		StartCoroutine("thermodynamic");

		buildings = this.gameObject.GetComponentsInChildren<BuildingData>() ;
	}

	// Update is called once per frame

	public void buildingDataCallback ( IEnumerable<ParseObject> allData , IEnumerable<ParseObject> allComment ){
		foreach (ParseObject b in allData) {
			string bid =  b.Get<int>("bid").ToString() ;
			GameObject building = GameObject.Find(bid) ; 
			if (building != null){
				BuildingData bb = building.GetComponent<BuildingData>();

				b.TryGetValue<string>("name" , out bb.bname) ;
				b.TryGetValue<string>("feature",out bb.feature);
				b.TryGetValue<string>("description",out bb.description);
			}else {
				Debug.LogWarning ("no such bid : " + bid) ; 
			}
		}

		foreach (ParseObject c in allComment) {
			string bid =  c.Get<int>("bid").ToString() ;
			GameObject building = GameObject.Find(bid) ; 
			if (building != null){
				BuildingData bb = building.GetComponent<BuildingData>();


				string[] commentArr = new string[2];
				c.TryGetValue<string>("type" , out commentArr[0] ); 
				c.TryGetValue<string>("comment" , out commentArr[1] ) ;

				bb.commentList.Add(commentArr) ; 

			}else {
				Debug.LogWarning ("comment couldnt find building : " + bid ) ; 
			}
		}
		isPrepared = true ;

	}
	public void spoting ( GameObject currentBuilding ){
		foreach( BuildingData b in buildings){
			if ( b.gameObject == currentBuilding ){
				b.active = true ;
			}else {
				b.active = false ;
			}
		}
	}

	public void spoting ( bool isSpoting = false ){
		if (!isSpoting){
			foreach( BuildingData b in buildings)
				b.active = true ;
		}
	}



	
	IEnumerator thermodynamic()
	{

		WWW www = new WWW("http://coconstructionv2.parseapp.com/thermodynamic");
		yield return www;


		if (www.error != null) {	
			Debug.Log ("WWW Error: " + www.error);
		} else {

			JSONArray jsonArray = JSONArray.Parse (www.text);
			foreach (JSONValue aa in jsonArray) {
				JSONObject statusJson;
				statusJson = JSONObject.Parse (aa.ToString ()); 
				string bid = statusJson.GetNumber ("bid").ToString ();
				GameObject building = GameObject.Find (bid);
//				Debug.Log (building);
				if (building != null) {

					BuildingData b = building.GetComponent<BuildingData> ();


				
					b.likeStatus = statusJson.GetString ("type");
					b.likeNum = (int)statusJson.GetNumber ("like");
					b.dislikeNum = (int)statusJson.GetNumber ("dislike");
					b.colorChange = true ;
				} else {
					Debug.LogWarning ("bid " + bid + " not found"); 
				}
			}
		}
	}
}
