using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingText : MonoBehaviour {

	[SerializeField]
	private Animator animator;
	private Text text;

	// Use this for initialization
	void Awake () {
		AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo (0);
		Destroy (gameObject, clipInfo[0].clip.length);
		text = animator.GetComponent<Text> ();
	}

	public void setText (string text) {
		this.text.text = text;
	}
}