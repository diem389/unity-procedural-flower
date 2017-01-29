﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace mattatz.ProceduralFlower {

	public class PFPart : MonoBehaviour {

		const string PROPERTY_BEND = "_Bend";
		const string PROPERTY_T = "_T";

		MeshRenderer renderer {
			get {
				if(_renderer == null) {
					_renderer = GetComponent<MeshRenderer>();
				}
				return _renderer;
			}
		}

		MaterialPropertyBlock block {
			get {
				if(_block == null) {
					_block = new MaterialPropertyBlock();
					renderer.GetPropertyBlock(_block);
				}
				return _block;
			}
		}

		MaterialPropertyBlock _block;
		MeshRenderer _renderer;
		List<PFAnimation> animations = new List<PFAnimation>();

		float multiplySpeed = 1f;
		float speed = 1f;

		bool substance = true;
		bool animating = false;
		float ticker = 0f;

		void Update () {
			if(animating) {
				ticker += Time.deltaTime * multiplySpeed * speed;
				Fade(ticker);
				animations.ForEach(anim => anim.Animate(speed, ticker));
				if(ticker > 1f) {
					animating = false;
				}
			}
		}

		public void HasSubstance (bool flag) {
			substance = flag;
		}

		public void Bend (float bend) {
			if(substance) {
				block.SetFloat(PROPERTY_BEND, bend);
				renderer.SetPropertyBlock(block);			
			}
		}

		public void Fade (float t) {
			if(substance) {
				block.SetFloat(PROPERTY_T, t);
				renderer.SetPropertyBlock(block);			
			}
		}

		public void SetSpeed (float m) {
			multiplySpeed = m;
		}

		public void Add (PFPart part, float ratio) {
			animations.Add(new PFAnimation(part, ratio));
		}

		public void Animate (float s = 1f) {
			speed = s;
			animating = true;
			ticker = 0f;
		}

		[System.Serializable]
		class PFAnimation {
			PFPart part;
			float ratio;

			bool animating = false;

			public PFAnimation (PFPart p, float r) {
				part = p;
				ratio = r;
			}

			public void Animate (float speed, float r) {
				if(!animating && r <= ratio) {
					part.Animate(speed);
				}
			}

		}

	}
		
}
