using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DoorScript
{
	[RequireComponent(typeof(AudioSource))]


	public class Door : MonoBehaviour
	{
		public bool open;
		public float smooth = 1.0f;
		float DoorOpenAngle = -90.0f;
		float DoorCloseAngle = 0.0f;
		public AudioSource asource;
		public AudioClip openDoor, closeDoor;

		public bool sci_fi;
        private Animator anim;
        private bool triggerOpen;
        private bool triggerClose;

        // Use this for initialization
        void Start()
		{
			asource = GetComponent<AudioSource>();
		}

		// Update is called once per frame
		void Update()
		{
			if (sci_fi)
			{
				if (anim == null)
				{
					anim = gameObject.GetComponent<Animator>();
				}
			}

			if (open)
			{
				if (!sci_fi)
				{
					var target = Quaternion.Euler(0, DoorOpenAngle, 0);
					transform.localRotation = Quaternion.Slerp(transform.localRotation, target, Time.deltaTime * 5 * smooth);
				}
				else
				{
					if (!triggerOpen)
					{
						triggerOpen = true;

						if (anim != null)
						{
							anim.SetBool("isOpen", true);
						}

					}
				}

			}
			else
			{
				if (!sci_fi)
				{
					var target1 = Quaternion.Euler(0, DoorCloseAngle, 0);
					transform.localRotation = Quaternion.Slerp(transform.localRotation, target1, Time.deltaTime * 5 * smooth);
				}
				else
				{
					if (!triggerClose)
					{
						triggerClose = true;

						if (anim != null)
						{
							anim.SetBool("isOpen", false);
						}

					}
				}
			}
		}

		public void OpenDoor()
		{
			open = !open;
			asource.clip = open ? openDoor : closeDoor;
			asource.Play();

			if (open)
			{
				triggerOpen = false;
			}
			else
			{
				triggerClose = false;
			}
		}
	}
}