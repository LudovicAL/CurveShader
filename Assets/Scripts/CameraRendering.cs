using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CameraRendering : MonoBehaviour {

	// Start is called before the first frame update
	void Start() {
		object[] obj = FindObjectsOfType(typeof(MeshRenderer));
		foreach (object o in obj) {
			MeshRenderer meshRenderer = (MeshRenderer)o;
			if (meshRenderer) {
				meshRenderer.bounds = new Bounds(meshRenderer.transform.position, new Vector3(999f, 999f, 999f));
			}
		}
	}

    // Update is called once per frame
    void Update() {
    }
}
