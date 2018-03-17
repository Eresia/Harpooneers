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
		
		// Transform seaTransform = sea.GetComponent<Transform>();

		// int lod = 32 * ((int) Mathf.Pow(2, sea.lodPower));

		// sea.points = new float[lod * lod];
		// sea.normales = new Vector3[lod * lod];

		// for(int i = 0; i < lod; i++){
		// 	for(int j = 0; j < lod; j++){
		// 		sea.points[i*lod + j] = seaTransform.position.y;
		// 		sea.normales[i*lod + j] = new Vector3(0f, 1f, 0f);
		// 	}
		// }

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

		// sea.points = new float[0];

		EditorUtility.SetDirty(sea);
		EditorSceneManager.MarkAllScenesDirty();

		return true;
	}
}
