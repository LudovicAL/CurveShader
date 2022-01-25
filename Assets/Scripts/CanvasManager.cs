using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour {

	private Slider rotationSlider;
	private Slider exponentXSlider;
	private Slider exponentYSlider;
	private Slider inceptionDistanceSlider;
	private Slider inceptionAngleSlider;
	private Slider inceptionRadiusSlider;
	private GameObject panelExponent;
	private GameObject panelRotation;
	private GameObject panelInception;
	private CurveSetter curveSetter;

	private void Awake() {
		panelExponent = transform.Find("PanelExponent").gameObject;
		panelRotation = transform.Find("PanelRotation").gameObject;
		panelInception = transform.Find("PanelInception").gameObject;
		rotationSlider = transform.Find("PanelRotation").Find("Slider").GetComponent<Slider>();
		exponentXSlider = transform.Find("PanelExponent").Find("SliderX").GetComponent<Slider>();
		exponentYSlider = transform.Find("PanelExponent").Find("SliderY").GetComponent<Slider>();
		inceptionDistanceSlider = transform.Find("PanelInception").Find("SliderDistance").GetComponent<Slider>();
		inceptionAngleSlider = transform.Find("PanelInception").Find("SliderAngle").GetComponent<Slider>();
		inceptionRadiusSlider = transform.Find("PanelInception").Find("SliderRadius").GetComponent<Slider>();
		curveSetter = GameObject.Find("CurveSetter").GetComponent<CurveSetter>();
	}

	// Start is called before the first frame update
	void Start() {
		OnChangeModeButton();
	}

    // Update is called once per frame
    void Update() {
        
    }

	public void OnRotationSliderValueChanged() {
		curveSetter.SetRotationCurvature(rotationSlider.value);
	}

	public void OnRotationAxisButton() {
		curveSetter.ChangeRotationAxis();
	}

	public void OnExponentSliderValueChanged() {
		curveSetter.SetExponentCurvature(new Vector2(exponentXSlider.value, exponentYSlider.value));
	}

	public void OnInceptionSliderValueChanged() {
		curveSetter.SetInceptionAngle(new Vector3(inceptionDistanceSlider.value, inceptionAngleSlider.value, inceptionRadiusSlider.value));
	}

	public void OnChangeModeButton() {
		CurveSetter.CurveMode curveMode = curveSetter.ChangeMode();
		panelExponent.SetActive(false);
		panelRotation.SetActive(false);
		panelInception.SetActive(false);
		switch (curveMode) {
			case CurveSetter.CurveMode.Exponent:
				panelExponent.SetActive(true);
				exponentXSlider.Select();
				break;
			case CurveSetter.CurveMode.Rotation:
				panelRotation.SetActive(true);
				rotationSlider.Select();
				break;
			case CurveSetter.CurveMode.Inception:
				panelInception.SetActive(true);
				inceptionDistanceSlider.Select();
				break;
		}
	}
}
