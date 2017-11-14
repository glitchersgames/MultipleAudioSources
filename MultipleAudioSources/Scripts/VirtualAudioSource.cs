using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Glitchers.MultipleAudioListeners
{
	[RequireComponent(typeof(AudioSource))]
	[ExecuteInEditMode]
	public class VirtualAudioSource : MonoBehaviour
	{
		#region Serialized
		public AudioSource audioSource;
		public Transform audioSourceTransform;
		#endregion

		#region Lifecycle
		private void OnEnable ()
		{
			#if UNITY_EDITOR
			if (Application.isPlaying == false)
			{
				if( audioSource == null )
				{
					UnityEditor.SerializedObject serializedObject = new UnityEditor.SerializedObject( this );
					var audioSourceProperty = serializedObject.FindProperty("audioSource");
					audioSourceProperty.objectReferenceValue = GetComponent<AudioSource>();
					serializedObject.ApplyModifiedPropertiesWithoutUndo();
				}

				if( audioSourceTransform == null )
				{
					UnityEditor.SerializedObject serializedObject = new UnityEditor.SerializedObject( this );
					var audioSourceProperty = serializedObject.FindProperty("audioSourceTransform");
					audioSourceProperty.objectReferenceValue = transform;
					serializedObject.ApplyModifiedPropertiesWithoutUndo();
				}

			} else
			#endif
			{
				if( VirtualAudioManager.simulateMultipleListeners )
				{
					/*
					 * Force 2D for all sound effects.
					 * 
					 * This is our cheat around spatial audio.
					 * If you are playing split-screen you don't need spatial audio.
					 * 
					 */
					audioSource.spatialBlend = 0f;
					audioSource.bypassReverbZones = true;
				}
				else
				{
					// If we don't need to simulate multiple listeners (only 1 player) then disable this script from calling LateUpdate
					enabled = false;
				}
			}
		}

		private void LateUpdate()
		{
			#if UNITY_EDITOR
			if( Application.isPlaying == false )
			{
				return;
			}
			#endif

			float distanceToListener = VirtualAudioManager.GetMinDistanceToListener( this );

			float distance01 = Mathf.Clamp01(Mathf.InverseLerp( audioSource.minDistance, audioSource.maxDistance, distanceToListener ));

			switch( audioSource.rolloffMode )
			{
			case AudioRolloffMode.Logarithmic:
				{
					audioSource.volume = Mathf.Log( 1f / distance01 );
					break;
				}

			case AudioRolloffMode.Linear:
				{
					audioSource.volume = 1f - distance01;
					break;
				}

			case AudioRolloffMode.Custom:
				{
					audioSource.volume = audioSource.GetCustomCurve( AudioSourceCurveType.CustomRolloff ).Evaluate( distance01 );
					break;
				}
			}
		}
		#endregion

	}
}
