using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CameraRendering : MonoBehaviour {

	private void OnEnable() {
		RenderPipelineManager.beginCameraRendering += OnBeginCameraRendering;
		RenderPipelineManager.endCameraRendering += OnEndCameraRendering;
	}

	private void OnDisable() {
		RenderPipelineManager.beginCameraRendering -= OnBeginCameraRendering;
		RenderPipelineManager.endCameraRendering -= OnEndCameraRendering;
	}

	// Start is called before the first frame update
	void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

	private void OnBeginCameraRendering(ScriptableRenderContext context, Camera camera) {
		camera.cullingMatrix = Matrix4x4.Ortho(-99, 99, -99, 99, 0.001f, 99) * camera.worldToCameraMatrix;
	}	

	private void OnEndCameraRendering(ScriptableRenderContext context, Camera camera) {
		camera.ResetCullingMatrix();
	}
}
