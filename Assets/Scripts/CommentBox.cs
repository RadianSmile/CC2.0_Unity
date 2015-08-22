using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CommentBox : MonoBehaviour {

	// Use this for initialization
	public Main mainController  ; 
	public InputField commentInput  ; 
	public bool isLike ; 

	bool waitingDB = false  ;

	void Start () {

	}

	public void commentCallback (){
		commentInput.text = "" ;
		mainController.status = "commentDone";
	}

	// Update is called once per frame
	void Update () {
		if (mainController.status == "comment"){

			if( (Input.GetKey(KeyCode.LeftControl) ||Input.GetKey(KeyCode.RightControl) ) && Input.GetKeyUp(KeyCode.Return) ) {
				string comment = commentInput.text;
				DB.ceateComment(mainController.currentBuilding.name , comment , isLike ) ; 
				waitingDB = true ; 
			}

			if (waitingDB && DB.commentDone){
				waitingDB = false ; 
				DB.commentDone = false ; 
				commentCallback () ;
			}
		}
	}






}
