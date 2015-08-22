using UnityEngine;
using System ; 
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class Main : MonoBehaviour {

	[Header("Develop Use")]
	public int setStatusNum = 0 ;

	[Header("Develop Read only")]
	public GameObject currentBuilding ;
	public BuildingData currentBuildingData ;
	public string tableStatus ; 
	public bool buildingChanged = false ; 
	public string _status = "" ;
	public GameObject currentActivePage ;

	[Header("Lights")]
	public Sun sun1 ;
	public Sun sun2 ;
	public Spot spot1 ;
	public Spot spot2 ;
	
	[Header("Obj Settings")]
	public CameraController Cam ; 
	public MapController mapController ;  
		
	[Header("Obj Settings - UI Page")]
	public GameObject Common ;

	public GameObject InfoA ;
	public InfoAData infoAData ;

	public GameObject Comments ;

	public GameObject CommentHint ;
	public GameObject CommentBox ;
	public CommentBox CommentBoxData ; 

	public GameObject Global ;

	




	public string status {
		set {
			if (value != _status){
				changed = true ; 
			}
			_status = value ; 
		}
		get {
			return _status ;
		}
	}

	bool changed = false ;
	// Use this for initialization

	void Start () {
		DB.init ();
		currentActivePage = Global; 
		status = "global";
		if (currentBuilding){
			currentBuildingData = currentBuilding.GetComponent<BuildingData>() ;
		}

	}

	private bool isInit ; 
	public void startSync (){
		StartCoroutine( keepUpdateTableStatus ());
	}


	IEnumerator keepUpdateTableStatus (){
		while (true) {
			StartCoroutine (DB.syncTableStatus());
			yield return new WaitForSeconds (1f) ; 
			GameObject tempB = GameObject.Find( DB.tableSelectBid );


			if ( !(status == "global" || status == "infoA") ) continue ;

			tableStatus = DB.tableStatus ;
			if (tableStatus == "select") status = "infoA" ; 

			if (tempB && (currentBuilding != tempB)){
				changed = true ; 
				Cam.targetChanged = true ; 
				currentBuilding = tempB ; 					
				currentBuildingData = currentBuilding.GetComponent<BuildingData>() ;
				Cam.target = currentBuilding.transform.transform;					
				 								
				Debug.Log ("currentBuildingData"+currentBuildingData);
			}else if (tempB == null){
				Debug.Log("no such building : " + DB.tableSelectBid ); 
			}else {

			}
		}

	}

	void Update(){
		// status controll

		if (!isInit && mapController.isPrepared ){
			isInit = true ;
			startSync() ;
		}
/////////// test part 

		if (setStatusNum == 1 ){
			status = "global" ; 

		}else if (setStatusNum == 2) {
			status = "infoA" ;

		}
///////////

		if (status == "infoA") {
			if (Input.GetKeyUp (KeyCode.L)) {
				Debug.Log("L") ; 
				StartCoroutine(DB.thumbBuilding(Cam.target.name , true)) ; 

				// this would be a problem because the remote building would change !  
				// maybe we can use a method that : we could use timestamp to prevent previous status .
//				DB.updateStatus("like");
				CommentBoxData.isLike = true ;
				status = "comment" ;
			}

			if (Input.GetKeyUp (KeyCode.D)) {
				Debug.Log("D") ; 
				StartCoroutine(DB.thumbBuilding(Cam.target.name , false )) ; 
				CommentBoxData.isLike = false ; 
//				DB.updateStatus("dislike");
				status = "comment" ; 
			}
		}else {
			Debug.Log (status) ;
		}


		if (changed) {
			Debug.Log ("status : " + status) ; 
			switch (status){
			
			case "global" : 
				toSpot(false);
				currentActivePage.SetActive(false);
				Global.SetActive(true);	
//				Global.GetComponent<Animator>().SetTrigger("fadeIn") ; 
				currentActivePage = Global ; 
				mapController.spoting(false);
				Cam.spoting = false ;
				break;
			case "infoA" :

				currentActivePage.SetActive(false);

				infoAData.currentBuildingData = currentBuildingData ;
				InfoA.SetActive(true);
				toSpot(true); 


				CommentHint.SetActive(false); 
				Comments.SetActive(true);
				currentActivePage = InfoA;

				mapController.spoting(currentBuilding) ;

				Cam.spoting = true ;

				break;
			
			case "comment" :
				DB.updateStatus("comment") ; 
				Comments.SetActive(false);
				CommentBox.SetActive(true);
				CommentHint.SetActive(true); 

				// other procedure is execuate in CommentBox script.
				break;

			case "commentDone":
				DB.updateStatus("commentDone") ;
				CommentBox.SetActive(false) ;
				status = "infoA" ; 
				break;
			case "idle" : 
				Global.GetComponent<Renderer>().material.DOFade(1,1);
				currentActivePage.GetComponent<Renderer>().material.DOFade(0,1);
				break ;			
			default :
				break;
			}
			changed = false; 
		}
	}

	void toSpot( bool isSpoting) {
		if (isSpoting) {
			sun1.fadeOut ();
			sun2.fadeOut ();
			spot1.fadeIn ();
			spot2.fadeIn ();	
		} else {
			sun1.fadeIn ();
			sun2.fadeIn ();
			spot1.fadeOut ();
			spot2.fadeOut ();
		}
	}
}


