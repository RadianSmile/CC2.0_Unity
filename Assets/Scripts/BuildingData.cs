using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class BuildingData : MonoBehaviour{

	static Color likeColorAlbedo = RColor (0, 113, 188, 255) ;
	static Color likeColorEmission = RColor (0, 65, 108,255);
	static Color dislikeColorAlbedo =  RColor (237, 28, 36, 255) ;
	static Color dislikeColorEmission =  RColor (161, 22, 27, 255) ;
	static Color drawColorAlbedo = new Color (1f,1f,1f,1f); 
	static Color drawColorEmission = RColor (97,97,97,255);

	static Color inActiveAlbedo = RColor (37,37,37,255); 
	static Color inActiveEmission = RColor (67,67,67,255); 


	static float tweenSecond = 3f ; 


	public List<string[]> commentList =  new List<string[]>();
	public bool colorChange = false  ;

	public float _percentage = 0  ;
	public float percentage {
		set {
			if (value != _percentage) {
				colorChange = true; 
				_percentage = value; 
			} else {
				_percentage = value; 
			}
		}
		get {
			return _percentage ;
		}
	}
	public string _likeStatus ;
	public string likeStatus {
		set {
			if (value != _likeStatus) {
				colorChange = true; 
				_likeStatus = value; 
			} else {
				_likeStatus = value; 
			}
		}
		get {
			return _likeStatus ;
		} 
	}
	public int likeNum = 0 ; 
	public int dislikeNum = 0 ;
	public string bname ; 
	public string description ; 
	public string feature ; 
	public bool _active = true ;
	public bool active {
		set {
			if (value != _active) {
				colorChange = true; 
				_active = value; 
			} else {
				_active = value; 
			}
		}
		get {
			return _active ;
		} 
	}   

	public Renderer building ;


	void Start (){
		building = this.gameObject.GetComponentsInChildren<Renderer> () [0];
		building.material.EnableKeyword("_Emission");
		active = true ;     
	}

	void Update (){
		if (colorChange){
			colorChange = false ;
			if (!active) {
				building.material.DOColor(inActiveAlbedo,tweenSecond);
				building.material.DOColor(inActiveEmission,"_EmissionColor",tweenSecond);
			}else{
				if (likeStatus == "draw"){
					building.material.DOColor(drawColorAlbedo,tweenSecond);
					building.material.DOColor(drawColorEmission,"_EmissionColor",tweenSecond);
				}else if (likeStatus == "like"){
					building.material.DOColor(likeColorAlbedo,tweenSecond);
					building.material.DOColor(likeColorEmission,"_EmissionColor",tweenSecond);
				}else if (likeStatus == "dislike"){
					building.material.DOColor(dislikeColorAlbedo,tweenSecond);
					building.material.DOColor(dislikeColorEmission,"_EmissionColor",tweenSecond);
				}
			}
		}
	}

	private int commentIndex = 0 ;

//	public int commentListStatus (){
//		int status = 0 ; 
//		if ( commentList.Count == 0 ){
//			status = 0 ;  // 
//		}else if ( commentIndex == commentList.Count ){
//			status = 1 ;  // all shown
//		}
//	}
	public void resetCommentIndex (){
		commentIndex = 0 ; 
	}

	public string[] nextComment(){

		if ( commentIndex < commentList.Count ){ // 
			return commentList[commentIndex++] ; 
		}else if (commentIndex == commentList.Count && commentList.Count != 0 ){ // all looped
//			commentIndex = 0 ;
			// here we have to be care for the same comment show in the same round.
//			return commentList[commentIndex++] ;
		}
		return null ;
	}

	static Color RColor (int r , int g , int b , int a ){
		float rr = (float)r/255f;
		float gg = (float)g/255f;
		float bb = (float)b/255f;
		float aa = (float)a/255f;
		return new Color (rr,gg,bb,aa);
	}
}

