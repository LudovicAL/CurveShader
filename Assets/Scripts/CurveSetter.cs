using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class CurveSetter : MonoBehaviour {

	public Transform referenceTransform;

	[System.Serializable]
	public enum CurveMode : int {
		Exponent,
		Rotation
	};

	[SerializeField]
	private Material[] materials;
	[SerializeField]
	private CurveMode curveMode;
	private CurveMode oldCurveMode;

	[Space(10)]
	[Header("Exponent")]
	[SerializeField]
	private Vector2 exponentCurvature;
	private Vector2 oldExponentCurvature;
	[SerializeField]
	private Shader exponentCurvatureShader;

	[Space(10)]
	[Header("Rotation")]
	[SerializeField]
	private float rotationCurvature;
	private float oldRotationCurvature;
	[SerializeField]
	private bool rotationAxis;
	private bool oldRotationAxis;
	[SerializeField]
	private Shader rotationCurvatureShader;

	void Awake() {
		if (Application.isPlaying) {
			Shader.EnableKeyword("_ENABLED");
		} else {
			Shader.DisableKeyword("_ENABLED");
		}
		oldCurveMode = curveMode;
		oldExponentCurvature = exponentCurvature;
		oldRotationCurvature = rotationCurvature;
		SetShader();
	}

	// Start is called before the first frame update
	void Start() {
	}

    // Update is called once per frame
    void Update() {
		foreach (Material mat in materials) {
			mat.SetVector("_referencePosition", referenceTransform.position);
		}
		if (curveMode != oldCurveMode
			|| exponentCurvature != oldExponentCurvature
			|| rotationCurvature != oldRotationCurvature
			|| rotationAxis != oldRotationAxis) {
			oldCurveMode = curveMode;
			SetShader();
		}		
	}

	private void SetShader() {
		switch (curveMode) {
			case CurveMode.Exponent:
				SetExponentCurvature();
				break;
			case CurveMode.Rotation:
				SetRotationCurvature();
				break;
		}
	}

	private void SetExponentCurvature() {
		foreach (Material mat in materials) {
			mat.shader = exponentCurvatureShader;
			oldExponentCurvature = exponentCurvature;
			mat.SetVector("_curvature", exponentCurvature);
		}
	}

	private void SetRotationCurvature() {
		foreach (Material mat in materials) {
			mat.shader = rotationCurvatureShader;
			oldRotationCurvature = rotationCurvature;
			oldRotationAxis = rotationAxis;
			mat.SetFloat("_curvature", rotationCurvature);
			if (rotationAxis) {
				mat.EnableKeyword("_AXIS");
			} else {
				mat.DisableKeyword("_AXIS");
			}
		}
	}

	public void SetRotationCurvature(float value) {
		rotationCurvature = value;
	}

	public void ChangeRotationAxis() {
		rotationAxis = !rotationAxis;
	}

	public void SetExponentCurvature(Vector2 value) {
		exponentCurvature = value;
	}

	public CurveMode ChangeMode() {
		switch (curveMode) {
			case CurveMode.Exponent:
				curveMode = CurveMode.Rotation;
				break;
			case CurveMode.Rotation:
				curveMode = CurveMode.Exponent;
				break;
		}
		return curveMode;
	}
}
