using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Glitchers.MultipleAudioListeners
{
	[ExecuteInEditMode]
	public class VirtualAudioListener : MonoBehaviour
	{
		#region Serialized
		public Transform listenerTransform;
		#endregion

		#region Lifecycle
		private void OnEnable ()
		{
			#if UNITY_EDITOR
			if (Application.isPlaying == false)
			{
				if( listenerTransform == null )
				{
					UnityEditor.SerializedObject serializedObject = new UnityEditor.SerializedObject( this );
					var audioSourceProperty = serializedObject.FindProperty("listenerTransform");
					audioSourceProperty.objectReferenceValue = transform;
					serializedObject.ApplyModifiedPropertiesWithoutUndo();
				}
			}
			else
			#endif
			{
				VirtualAudioManager.AddListener( this );
			}
		}

		private void OnDisable ()
		{
			#if UNITY_EDITOR
			if (Application.isPlaying == false)
			{

			}
			else
			#endif
			{
				VirtualAudioManager.RemoveListener( this );
			}
		}

		#endregion

	}
}