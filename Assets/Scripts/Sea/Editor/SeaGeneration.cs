using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class SeaGeneration {

	[MenuItem("DSea/Generate Sea")]
	public static void GenerateSea(){
		if(!ClearSea()){
			return ;
		}
		Sea sea = GameObject.FindObjectOfType<Sea>();
		
		Transform seaTransform = sea.GetComponent<Transform>();

		int size = sea.size;
		float colliderRadius = sea.colliderRadius;
		float colliderSpace = sea.colliderSpace;
		int layer = sea.layer;
		sea.wave = new Transform[size * size];

		for(int i = 0; i < size; i++){
			for(int j = 0; j < size; j++){
				GameObject newCollider = new GameObject();
				Transform colliderTransform = newCollider.GetComponent<Transform>();
				SphereCollider collider = newCollider.AddComponent<SphereCollider>();
				collider.radius = colliderRadius;
				float x = (((float) i) - (((float) size) / 2)) * colliderSpace + seaTransform.position.x;
				float y = seaTransform.position.y;
				float z = (((float) j) - (((float) size) / 2)) * colliderSpace + seaTransform.position.z;
				colliderTransform.position = new Vector3(x, y, z);
				colliderTransform.parent = seaTransform;
				newCollider.layer = layer;
				sea.wave[i*size + j] = colliderTransform;
				newCollider.name = "Wave " + i + ";" + j;
				EditorUtility.SetDirty(newCollider);
			}
		}

		EditorUtility.SetDirty(sea);
		EditorSceneManager.MarkAllScenesDirty();
	}

	[MenuItem("DSea/Clear Sea")]
	public static bool ClearSea(){
		Sea sea = GameObject.FindObjectOfType<Sea>();

		if(sea == null){
			Debug.Log("Need a Sea");
			return false;
		}

		Transform seaTransform = sea.GetComponent<Transform>();
		for(int i = seaTransform.childCount - 1; i >= 0; i--){
			GameObject.DestroyImmediate(seaTransform.GetChild(i).gameObject);
		}
		EditorUtility.SetDirty(sea);
		EditorSceneManager.MarkAllScenesDirty();

		return true;
	}
}
