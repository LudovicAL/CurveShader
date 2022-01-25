using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class CurveSetter : MonoBehaviour {

	public Transform referenceTransform;

	[System.Serializable]
	public enum CurveMode : int {
		Exponent,
		Rotation,
		Inception
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

	[Space(10)]
	[Header("Inception")]
	[SerializeField]
	private float inceptionDistance;
	private float oldInceptionDistance;
	[SerializeField]
	private float inceptionAngle;
	private float oldInceptionAngle;
	[SerializeField]
	private float inceptionRadius;
	private float oldInceptionRadius;
	[SerializeField]
	private Shader inceptionCurvatureShader;

	void Awake() {
		if (Application.isPlaying) {
			Shader.EnableKeyword("_ENABLED");
		} else {
			Shader.DisableKeyword("_ENABLED");
		}
		oldCurveMode = curveMode;
		oldExponentCurvature = exponentCurvature;
		oldRotationCurvature = rotationCurvature;
		oldRotationAxis = rotationAxis;
		oldInceptionDistance = inceptionDistance;
		oldInceptionAngle = inceptionAngle;
		oldInceptionRadius = inceptionRadius;
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
			|| rotationAxis != oldRotationAxis
			|| inceptionAngle != oldInceptionAngle
			|| inceptionDistance != oldInceptionDistance
			|| inceptionRadius != oldInceptionRadius) {
			oldCurveMode = curveMode;
			SetShader();
		}		
	}

	private void SetShader() {
		switch (curveMode) {
			case CurveMode.Exponent:
				SetExponent();
				break;
			case CurveMode.Rotation:
				SetRotation();
				break;
			case CurveMode.Inception:
				SetInception();
				break;
		}
	}

	private void SetExponent() {
		foreach (Material mat in materials) {
			mat.shader = exponentCurvatureShader;
			oldExponentCurvature = exponentCurvature;
			mat.SetVector("_curvature", exponentCurvature);
		}
	}

	private void SetRotation() {
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

	private void SetInception() {
		foreach (Material mat in materials) {
			mat.shader = inceptionCurvatureShader;
			oldInceptionDistance = inceptionDistance;
			oldInceptionAngle = inceptionAngle;
			oldInceptionRadius = inceptionRadius;
			mat.SetFloat("_distance", inceptionDistance);
			mat.SetFloat("_angle", inceptionAngle);
			mat.SetFloat("_radius", inceptionRadius);
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

	public void SetInceptionAngle(Vector3 value) {
		inceptionDistance = value.x;
		inceptionAngle = value.y;
		inceptionRadius = value.z;
	}

	public CurveMode ChangeMode() {
		switch (curveMode) {
			case CurveMode.Exponent:
				curveMode = CurveMode.Rotation;
				break;
			case CurveMode.Rotation:
				curveMode = CurveMode.Inception;
				break;
			case CurveMode.Inception:
				curveMode = CurveMode.Exponent;
				break;
		}
		return curveMode;
	}

	void OnDrawGizmos() {
		Vector3 arcCenter = new Vector3(referenceTransform.position.x + inceptionDistance, inceptionRadius, referenceTransform.position.z);
		Vector3 positionAtTheEndOfTheArc = new Vector3(Mathf.Cos((inceptionAngle - 90f) * Mathf.Deg2Rad) * inceptionRadius + arcCenter.x, Mathf.Sin((inceptionAngle - 90f) * Mathf.Deg2Rad) * inceptionRadius + arcCenter.y, referenceTransform.position.z);
		float arcCircumference = 2 * Mathf.PI * inceptionRadius;
		float arcLength = (inceptionAngle / 360f) * arcCircumference;
		Vector3 curveSectionBeginPosition = referenceTransform.position + (Vector3.right * inceptionDistance) - (referenceTransform.position.y * Vector3.up);
		Vector3 curveSectionEndPosition = curveSectionBeginPosition + (Vector3.right * arcLength);
		Vector3 tangentNormalVector = positionAtTheEndOfTheArc - arcCenter;

		//Sphere at the end of the arc
		Gizmos.color = Color.magenta;
		Gizmos.DrawSphere(positionAtTheEndOfTheArc, 1f);

		//Line from the center to the end of the arc
		Gizmos.color = Color.red;
		Gizmos.DrawRay(arcCenter, tangentNormalVector);

		//Line from the begin of the curve section to its end
		Gizmos.color = Color.blue;
		Gizmos.DrawRay(curveSectionBeginPosition + Vector3.up, Vector3.right * arcLength);

		//Sphere main
		Gizmos.color = new Color(0f, 1f, 1f, 0.3f);
		Gizmos.DrawSphere(arcCenter, inceptionRadius);
	}
}
