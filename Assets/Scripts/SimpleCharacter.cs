using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class SimpleCharacter : MonoBehaviour {
	[Header("Movement")]
	[SerializeField]
	private float characterSpeed = 3.0F;
	private CharacterController characterController;
	private Vector2 characterDirection;

	[Space(10)]
	[Header("Camera")]
	[SerializeField]
	private float cameraSensitivity = 2f;
	[SerializeField]
	private float cameraMaxLookAngle = 50f;
	private float cameraYaw = 0.0f;
	private float cameraPitch = 0.0f;
	private Camera playerCamera;

	private void Awake() {
		characterController = this.GetComponent<CharacterController>();
	}

	private void Start() {
		playerCamera = Camera.main;
	}

	void Update() {
		//Camera
		if (!Mouse.current.rightButton.isPressed) {
			cameraYaw = transform.localEulerAngles.y + Mouse.current.delta.ReadValue().x * cameraSensitivity;
			cameraPitch -= cameraSensitivity * Mouse.current.delta.ReadValue().y;
			cameraPitch = Mathf.Clamp(cameraPitch, -cameraMaxLookAngle, cameraMaxLookAngle);
			transform.localEulerAngles = new Vector3(0, cameraYaw, 0);
			playerCamera.transform.localEulerAngles = new Vector3(cameraPitch, 0, 0);
		}

		//Movement
		Vector3 forward = transform.TransformDirection(Vector3.forward) * characterDirection.y;
		Vector3 right = transform.TransformDirection(Vector3.right) * characterDirection.x;
		characterController.SimpleMove((forward + right) * characterSpeed);
	}

	public void OnMove(InputAction.CallbackContext context) {
		characterDirection = context.ReadValue<Vector2>();
	}
}