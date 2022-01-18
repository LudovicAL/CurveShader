using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class SimpleCharacter : MonoBehaviour {
	[SerializeField]
	private float characterSpeed = 3.0F;
	[SerializeField]
	private float cameraSpeed = 3.0F;
	[SerializeField]
	private float rotateSpeed = 3.0F;
	private CharacterController characterController;
	private Transform cameraTransform;
	private Vector2 characterDirection;
	private float cameraDirection;

	private void Awake() {
		characterController = this.GetComponent<CharacterController>();
	}

	private void Start() {
		cameraTransform = Camera.main.transform;
	}

	void Update() {
		// Rotate around y - axis
		transform.Rotate(0, characterDirection.x * rotateSpeed, 0);

		// Move forward / backward
		Vector3 forward = transform.TransformDirection(Vector3.forward);
		float curSpeed = characterSpeed * characterDirection.y;
		characterController.SimpleMove(forward * curSpeed);

		// Move the camera up or down
		cameraTransform.Rotate(Vector3.right, cameraDirection * cameraSpeed * Time.deltaTime);
	}

	public void OnMove(InputAction.CallbackContext context) {
		characterDirection = context.ReadValue<Vector2>();
	}

	public void OnCameraMove(InputAction.CallbackContext context) {
		cameraDirection = context.ReadValue<Vector2>().y;
	}
}