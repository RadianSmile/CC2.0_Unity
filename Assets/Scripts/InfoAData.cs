using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI ; 

public class InfoAData : MonoBehaviour {

	static Color dislikeColor = Lib.RColor(237,28,36,255) ; 
	static Color likeColor = Lib.RColor(0,113,188,255) ;



	public BuildingData currentBuildingData ; 

	public string bname ;
	public string description ;
	public string feature1 ;
	public string feature2 ;


	public Text NameField ; 
	public Text descriptionField ; 
	public Text feature1Field ; 
	public Text feature2Field ; 

	public List<Text> reviews ;

//	public Text review0 ; 
//	public Text review1 ; 
//	public Text review2 ; 



	// Use this for initialization
	void Start () {

		NameField.text = currentBuildingData.bname ; 
		descriptionField.text = currentBuildingData.description ;
		feature1Field.text = currentBuildingData.feature ;

		foreach (Text review in reviews){
			string[] commentArr =  currentBuildingData.nextComment() ;
			if (commentArr != null){
				review.color = getColor(commentArr) ;
				review.text = commentArr[1] ; 
			}else{
				break;
			}

		}


//		review0 =  ; 

	}

	Color getColor (string[] commentArr){
		if( commentArr[0] == "l" ){
			return likeColor ; 
		}
			return dislikeColor ; 
	}

	// Update is called once per frame
	void Update () {

	
	}

}
