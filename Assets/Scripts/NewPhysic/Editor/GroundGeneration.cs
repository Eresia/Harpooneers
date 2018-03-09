using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class GroundGeneration {

	[MenuItem("DSea/Generate Sea %g")]
	public static void GenerateSea(){
		if(!ClearSea()){
			return ;
		}
		Ground sea = GameObject.FindObjectOfType<Ground>();
		
		Transform seaTransform = sea.GetComponent<Transform>();

		sea.points = new float[sea.lod.x * sea.lod.y];
		sea.coeffPoints = new float[sea.lod.x * sea.lod.y];

		for(int i = 0; i < sea.lod.x; i++){
			for(int j = 0; j < sea.lod.y; j++){
				sea.points[i*sea.lod.y + j] = seaTransform.position.y;
			}
		}

		// sea.points[5 * sea.lod.y + 8] = 1f;
		// sea.points[6 * sea.lod.y + 8] = 1f;
		// sea.points[5 * sea.lod.y + 7] = 1f;
		// sea.points[6 * sea.lod.y + 7] = 1f;

		EditorUtility.SetDirty(sea);
		EditorSceneManager.MarkAllScenesDirty();
	}

	[MenuItem("DSea/Clear Sea")]
	public static bool ClearSea(){
		Ground sea = GameObject.FindObjectOfType<Ground>();

		if(sea == null){
			Debug.Log("Need a Sea");
			return false;
		}

		sea.points = new float[0];

		EditorUtility.SetDirty(sea);
		EditorSceneManager.MarkAllScenesDirty();

		return true;
	}
}
