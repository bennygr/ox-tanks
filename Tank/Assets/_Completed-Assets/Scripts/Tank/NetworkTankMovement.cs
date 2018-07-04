using UnityEngine;
using UnityEngine.Networking;

namespace Complete {
	public class NetworkTankMovement : NetworkBehaviour {
		public int m_PlayerNumber = 1;              // Used to identify which tank belongs to which player.  This is set by this tank's manager.
		public float m_Speed = 12f;                 // How fast the tank moves forward and back.
		public float m_TurnSpeed = 180f;            // How fast the tank turns in degrees per second.
		public float m_PitchRange = 0.2f;           // The amount by which the pitch of the engine noises can vary.

		public AudioSource m_MovementAudio;         // Reference to the audio source used to play engine sounds. NB: different to the shooting audio source.
		public AudioClip m_EngineIdling;            // Audio to play when the tank isn't moving.
		public AudioClip m_EngineDriving;           // Audio to play when the tank is moving.

		public ParticleSystem m_LeftDustTrail;        // The particle system of dust that is kicked up from the left track.
		public ParticleSystem m_RightDustTrail;       // The particle system of dust that is kicked up from the rightt track.

		public Rigidbody m_Rigidbody;              // Reference used to move the tank.

		private string m_MovementAxisName;          // The name of the input axis for moving forward and back.
		private string m_TurnAxisName;              // The name of the input axis for turning.
		private float m_MovementInputValue;         // The current value of the movement input.
		private float m_TurnInputValue;             // The current value of the turn input.
		private float m_OriginalPitch;              // The pitch of the audio source at the start of the scene.

		/// <summary>
		/// Setup tank
		/// </summary>
		public void SetDefaults() {
			m_Rigidbody.velocity = Vector3.zero;
			m_Rigidbody.angularVelocity = Vector3.zero;

			m_MovementInputValue = 0f;
			m_TurnInputValue = 0f;

			m_LeftDustTrail.Clear();
			m_LeftDustTrail.Stop();

			m_RightDustTrail.Clear();
			m_RightDustTrail.Stop();
		}

		private void Awake() {
			m_Rigidbody = GetComponent<Rigidbody>();
		}

		public override void OnStartLocalPlayer() {
			Camera.main.GetComponent<ChaseCamera>().SetTarget(gameObject.transform);
		}


		private void OnEnable() {
			m_Rigidbody.constraints = m_OriginalConstrains;
		}


		//freeze the rigibody when the control is disabled to avoid the tank drifting
		protected RigidbodyConstraints m_OriginalConstrains;
		void OnDisable() {
			m_OriginalConstrains = m_Rigidbody.constraints;
			m_Rigidbody.constraints = RigidbodyConstraints.FreezeAll;
		}


		private void Start() {
			// The axes names are based on player number.
			m_MovementAxisName = "Vertical";
			m_TurnAxisName = "Horizontal";

			// Store the original pitch of the audio source.
			m_OriginalPitch = m_MovementAudio.pitch;
		}


		private void Update() {
			if (!isLocalPlayer) {
				return;
			}
			// Store the value of both input axes.
			m_MovementInputValue = Input.GetAxis(m_MovementAxisName);
			m_TurnInputValue = Input.GetAxis(m_TurnAxisName);

			EngineAudio();
		}


		private void EngineAudio() {
			// If there is no input (the tank is stationary)...
			if (Mathf.Abs(m_MovementInputValue) < 0.1f && Mathf.Abs(m_TurnInputValue) < 0.1f) {
				// ... and if the audio source is currently playing the driving clip...
				if (m_MovementAudio.clip == m_EngineDriving) {
					// ... change the clip to idling and play it.
					m_MovementAudio.clip = m_EngineIdling;
					m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
					m_MovementAudio.Play();
				}
			} else {
				// Otherwise if the tank is moving and if the idling clip is currently playing...
				if (m_MovementAudio.clip == m_EngineIdling) {
					// ... change the clip to driving and play.
					m_MovementAudio.clip = m_EngineDriving;
					m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
					m_MovementAudio.Play();
				}
			}
		}

		public void ReEnableParticles() {
			m_LeftDustTrail.Play();
			m_RightDustTrail.Play();
		}


		private void FixedUpdate() {
			if (!isLocalPlayer) {
				return;
			}
			// Adjust the rigidbodies position and orientation in FixedUpdate.
			Move();
			Turn();
		}


		private void Move() {
			// Create a vector in the direction the tank is facing with a magnitude based on the input, speed and the time between frames.
			Vector3 movement = transform.forward * m_MovementInputValue * m_Speed * Time.deltaTime;

			// Apply this movement to the rigidbody's position.
			m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
		}


		private void Turn() {
			// Determine the number of degrees to be turned based on the input, speed and time between frames.
			float turn = m_TurnInputValue * m_TurnSpeed * Time.deltaTime;

			// Make this into a rotation in the y axis.
			Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);

			// Apply this rotation to the rigidbody's rotation.
			m_Rigidbody.MoveRotation(m_Rigidbody.rotation * turnRotation);
		}
	}

}