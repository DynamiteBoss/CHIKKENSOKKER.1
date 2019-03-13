using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#if PUN_2_OR_NEWER

#elif MIRROR
using Mirror;
using System.Linq;
#else
using UnityEngine.Networking;
#endif


#if UNITY_EDITOR
using UnityEditor;
#endif

#pragma warning disable CS0618 // UNET obsolete

namespace emotitron.Utilities.Networking
{

	/// <summary>
	/// Component that will replace NetworkManager, NetworkManagerHUD and NetworkIdentity with mirror version at runtime.
	/// </summary>
	public class MirrorCheck : MonoBehaviour
	{
#if PUN_2_OR_NEWER
		
#elif MIRROR

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
	public static void RunCheck()
	{
		var scene = SceneManager.GetActiveScene();

		//var objs = Resources.FindObjectsOfTypeAll<MirrorCheck>();
		var objs = Object.FindObjectsOfType<MirrorCheck>();

		for (int i = 0; i < objs.Length; i++)
			objs[i].Check();
	}

	
	private void Awake()
	{
		/// Remove the UNET NetIdentity if it exists on this object.
		var netidentity = GetComponent<UnityEngine.Networking.NetworkIdentity>();

		if (netidentity)
			DestroyImmediate(netidentity);
	}
	
	private void Check()
	{

		var unetNM = GetComponent<UnityEngine.Networking.NetworkManager>();

		/// If this has a UNET NM on it, replace it with Mirror, and copy the playerprefab over
		if (unetNM)
		{
			var transport = GetComponent<Transport>();
			if (!transport)
				transport = gameObject.AddComponent<TelepathyTransport>();

			var mirrorNM = GetComponent<Mirror.NetworkManager>();
			if (mirrorNM == null)
			{
				mirrorNM = gameObject.AddComponent<Mirror.NetworkManager>();
				NetworkManager.singleton = mirrorNM;
			}

			var mirrorHUD = GetComponent<Mirror.NetworkManagerHUD>();
			if (mirrorHUD == null)
				mirrorHUD = gameObject.AddComponent<Mirror.NetworkManagerHUD>();

#if MIRROR_1726_OR_NEWER

			/// Initialize some stuff Mirror doesn't on its own (at least when this was written)
			Transport.activeTransport = transport;
			Transport.activeTransport.OnServerDisconnected = new UnityEventInt();
			Transport.activeTransport.OnServerConnected = new UnityEventInt();
			Transport.activeTransport.OnServerDataReceived = new UnityEventIntByteArray();
			Transport.activeTransport.OnServerError = new UnityEventIntException();
			Transport.activeTransport.OnClientConnected = new UnityEngine.Events.UnityEvent();
			Transport.activeTransport.OnClientDataReceived = new UnityEventByteArray();
			Transport.activeTransport.OnClientError = new UnityEventException();
			Transport.activeTransport.OnClientDisconnected = new UnityEngine.Events.UnityEvent();

#else
			/// Initialize some stuff Mirror doesn't on its own (Fix this Mirror team)
			NetworkManager.singleton.transport = transport;
			NetworkManager.singleton.transport.OnServerDisconnected = new UnityEventInt();
			NetworkManager.singleton.transport.OnServerConnected = new UnityEventInt();
			NetworkManager.singleton.transport.OnServerDataReceived = new UnityEventIntByteArray();
			NetworkManager.singleton.transport.OnServerError = new UnityEventIntException();
			NetworkManager.singleton.transport.OnClientConnected = new UnityEngine.Events.UnityEvent();
			NetworkManager.singleton.transport.OnClientDataReceived = new UnityEventByteArray();
			NetworkManager.singleton.transport.OnClientError = new UnityEventException();
			NetworkManager.singleton.transport.OnClientDisconnected = new UnityEngine.Events.UnityEvent();
#endif

			/// Destroy the Unet HUD
			var unetHUD = mirrorNM.GetComponent<UnityEngine.Networking.NetworkManagerHUD>();
			if (unetHUD)
				Object.DestroyImmediate(unetHUD, true);

			/// Copy values from UNET NM to Mirror NM
			if (unetNM)
			{
				CopyPlayerPrefab(unetNM, mirrorNM);
				CopySpawnablePrefabs(unetNM, mirrorNM);
				Object.DestroyImmediate(unetNM, true);
			}
		}
	}

	public static void GiveMirrorNetworkIdentity(GameObject prefab)
	{
		var unetNI = prefab.GetComponent<UnityEngine.Networking.NetworkIdentity>();

		var mirrorNI = prefab.GetComponent<Mirror.NetworkIdentity>();

		if (!mirrorNI)
		{
			mirrorNI = prefab.AddComponent<Mirror.NetworkIdentity>();
		}

		if (unetNI)
		{
			mirrorNI.localPlayerAuthority = unetNI.localPlayerAuthority;
			mirrorNI.serverOnly = unetNI.serverOnly;
			//DestroyImmediate(unetni, true);
		}
	}

	public static void CopyPlayerPrefab(UnityEngine.Networking.NetworkManager src, Mirror.NetworkManager targ)
	{
		/// Make sure the player object is using mirror components
		if (src.playerPrefab)
		{
			GiveMirrorNetworkIdentity(src.playerPrefab);

			targ.autoCreatePlayer = src.autoCreatePlayer;
			targ.playerPrefab = src.playerPrefab;
		}
	}

	public static void CopySpawnablePrefabs(UnityEngine.Networking.NetworkManager src, Mirror.NetworkManager targ)
	{
		foreach (var obj in src.spawnPrefabs)
		{
			GiveMirrorNetworkIdentity(obj);

			if (!targ.spawnPrefabs.Contains(obj))
				targ.spawnPrefabs.Add(obj);
		}
	}

	

#endif

#if UNITY_EDITOR

		[CustomEditor(typeof(MirrorCheck))]
		[CanEditMultipleObjects]
		public class MirrorCheckEditor : Editor
		{
			public override void OnInspectorGUI()
			{
				base.OnInspectorGUI();
				EditorGUILayout.HelpBox("If 'MIRROR' exists as a define, this component will replace UNET components with Mirror versions at runtime.",
					MessageType.None);
			}
		}

#endif

	}
}

#pragma warning restore CS0618 // UNET obsolete
