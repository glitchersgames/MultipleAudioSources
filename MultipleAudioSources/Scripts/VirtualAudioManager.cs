using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Glitchers.MultipleAudioListeners
{
	public class VirtualAudioManager : MonoBehaviour
	{
		#region Variables
		public static bool simulateMultipleListeners = true; // set to false before setting up VirtualAudioSources to disable simulation
		private static bool s_IsSetup = false;
		private static List<VirtualAudioListener> s_Listeners;
		#endregion

		#region Setup
		private static void Setup()
		{
			s_Listeners = new List<VirtualAudioListener>();
			s_IsSetup = true;
		}

		public static void AddListener( VirtualAudioListener listener )
		{
			if( s_IsSetup == false )
			{
				Setup();
			}

			s_Listeners.Add( listener );
		}

		public static void RemoveListener( VirtualAudioListener listener )
		{
			if( s_IsSetup == false )
			{
				Setup();
			}

			s_Listeners.Remove( listener );
		}

		public static void RemoveAllListeners()
		{
			if( s_IsSetup )
			{
				s_Listeners.Clear();
			}
		}
		#endregion

		#region Methods
		public static float GetMinDistanceToListener( VirtualAudioSource virtualAudioSource )
		{
			Vector3 sourcePosition = virtualAudioSource.audioSourceTransform.position;

			if( s_IsSetup == false || s_Listeners == null || s_Listeners.Count == 0 )
			{
				return 0f;
			}

			// Set default value based on first listener
			float minDistanceSqr = Vector3.SqrMagnitude( s_Listeners[0].listenerTransform.position - sourcePosition );;
			int closestListenerIndex = 0;
			int listenerCount = s_Listeners.Count;

			// if we have multiple listeners then find the closest
			if( listenerCount > 1 )
			{
				for( int i = 1; i < listenerCount; i++ )
				{
					float distanceSqr = Vector3.SqrMagnitude( s_Listeners[i].listenerTransform.position - sourcePosition );
					if( distanceSqr < minDistanceSqr )
					{
						minDistanceSqr = distanceSqr;
						closestListenerIndex = i;
					}
				}
			}

			return Mathf.Sqrt( minDistanceSqr );
		}
		#endregion
	}
}