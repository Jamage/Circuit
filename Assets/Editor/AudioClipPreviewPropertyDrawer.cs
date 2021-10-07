using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using System.Reflection;
using System;
#endif

namespace Clavian
{
	/*
	audio stop/play code from https://forum.unity.com/threads/reflected-audioutil-class-for-making-audio-based-editor-extensions.308133/

	waveform drawing adapted from https://answers.unity.com/questions/1603418/how-to-create-waveform-texture-from-audioclip.html
	//...but this ended up being unused. going to keep it here just in case it turns out to be more efficient. I'll keep it in here just to keep everything in one place!

	for Unity 2020, apparently some of the classes have changed: https://forum.unity.com/threads/reflected-audioutil-class-for-making-audio-based-editor-extensions.308133/#post-6459664
	//applied this but havent actually tested in unity 2020.2 yet.

	I also found this when I was 99% done. helped me with making just one button update in the inspector tho.
	https://www.reddit.com/r/Unity3D/comments/5n6ddx/audioclip_propertydrawer/
	*/
	
	/*
	//attribute, in case this code shouldn't apply to all audio clips. [AudioClipPreview]
	public class AudioClipPreview : PropertyAttribute
	{
		public AudioClipPreview()
		{

		}
	}
	*/
	#if UNITY_EDITOR
	//in case an attribute should be used instead:
	//[CustomPropertyDrawer(typeof(AudioClipPreview))]
	//attribute no longer needed if i just make this work with all audioclips.
	[CustomPropertyDrawer(typeof(AudioClip))]
	public class AudioClipDrawer : PropertyDrawer
	{

		private static string currentClipName;
		private bool isThisClipPlaying = false;
		private Rect fieldRect;
		private AudioClip targetClip;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{

			EditorGUI.BeginProperty(position, label, property);

			var target = property.objectReferenceValue;
			//label
			position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

			//rects
			var buttonRect = new Rect(position.x, position.y, 20, position.height);
			fieldRect = new Rect(position.x + 25, position.y, position.width - 25, position.height);

			if(target == null) //object field
			{
				EditorGUI.ObjectField(fieldRect, property, GUIContent.none);
			}
			else if(target.GetType() != typeof(AudioClip))
			{
				//hey wrong type!
				EditorGUI.LabelField(fieldRect, "Wrong type! Must be AudioClip.");
			}
			else
			{
				targetClip = (AudioClip)target;
				isThisClipPlaying = EditorSFX.IsClipPlaying(targetClip) && (property.propertyPath == currentClipName);
				//draw fields
				if(isThisClipPlaying)
				{
					if(GUI.Button(buttonRect, "■"))
					{
						currentClipName = string.Empty;
						EditorSFX.StopAllClips();
					}
				}
				else
				{
					if(GUI.Button(buttonRect, "▶"))
					{
						currentClipName = property.propertyPath;
						//Debug.Log(target.GetType().ToString() + " " + target.name);
						//play that sound.
						EditorSFX.StopAllClips();
						EditorSFX.PlayClip(targetClip, 0, false);
					}
				}
				//EditorGUI.PropertyField(fieldRect, property.FindPropertyRelative)
				EditorGUI.ObjectField(fieldRect, property, typeof(AudioClip), GUIContent.none);
				var myGUIStyle = new GUIStyle(GUIStyle.none);
				//THIS ONE WORKS: but
			//	var waveformTex = EditorSFX.PaintWaveformSpectrum(targetClip, 1f, 100, 20, new Color(1f, 0.55f, 0f, 0.3f), Color.clear);
				var waveformTex = AssetPreview.GetAssetPreview(property.objectReferenceValue);
				GUI.color = new Color(1f,1f,1f,0.3f);
				myGUIStyle.normal.background = waveformTex;
				EditorGUI.LabelField(fieldRect, GUIContent.none, myGUIStyle);
				GUI.color = Color.white;

				if(isThisClipPlaying)
				{
					//draw progress bar.
					
					var samplerRect = new Rect(fieldRect.x + ((float)EditorSFX.GetClipSamplePosition(targetClip) / (float)targetClip.samples * fieldRect.width), fieldRect.y, 1f, fieldRect.height);
					var samplerGUIStyle = new GUIStyle(GUIStyle.none);
					samplerGUIStyle.normal.background = Texture2D.whiteTexture;
					EditorGUI.LabelField(samplerRect, GUIContent.none, samplerGUIStyle);
				}
				
			}

			EditorGUI.EndProperty();
		}
	}

	public static class EditorSFX
	{
	
		public static void PlayClip(AudioClip clip , int startSample , bool loop) {
			Assembly unityEditorAssembly = typeof(AudioImporter).Assembly;
			Type audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
			MethodInfo method = audioUtilClass.GetMethod(
				#if UNITY_2020_2_OR_NEWER
				"PlayPreviewClip",
				#else
				"PlayClip",
				#endif
				BindingFlags.Static | BindingFlags.Public,
				null,
				new System.Type[] {
				typeof(AudioClip),
				typeof(Int32),
				typeof(Boolean)
			},
			null
			);
			method.Invoke(
				null,
				new object[] {
				clip,
				startSample,
				loop
			}
			);
		}
	
		public static void StopAllClips () {
			Assembly unityEditorAssembly = typeof(AudioImporter).Assembly;
			Type audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
			MethodInfo method = audioUtilClass.GetMethod(
				#if UNITY_2020_2_OR_NEWER
				"StopAllPreviewClips",
				#else
				"StopAllClips",
				#endif
				BindingFlags.Static | BindingFlags.Public
				);

			method.Invoke(
				null,
				null
				);
		}

		public static bool IsClipPlaying(AudioClip clip) {
			Assembly unityEditorAssembly = typeof(AudioImporter).Assembly;
			Type audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
			MethodInfo method = audioUtilClass.GetMethod(
				#if UNITY_2020_2_OR_NEWER
				"IsPreviewClipPlaying",
				#else
				"IsClipPlaying",
				#endif
				BindingFlags.Static | BindingFlags.Public
				);
			
			bool playing = (bool)method.Invoke(
				null,
				new object[] {
				clip,
			}
			);
			
			return playing;
		}

		public static float GetClipPosition(AudioClip clip) {
			Assembly unityEditorAssembly = typeof(AudioImporter).Assembly;
			Type audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
			MethodInfo method = audioUtilClass.GetMethod(
				"GetClipPosition",
				BindingFlags.Static | BindingFlags.Public
				);
			
			float position = (float)method.Invoke(
				null,
				new object[] {
				clip
			}
			);
			
			return position;
		}

		public static int GetClipSamplePosition(AudioClip clip) {
			Assembly unityEditorAssembly = typeof(AudioImporter).Assembly;
			Type audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
			MethodInfo method = audioUtilClass.GetMethod(
				"GetClipSamplePosition",
				BindingFlags.Static | BindingFlags.Public
				);
			
			int position = (int)method.Invoke(
				null,
				new object[] {
				clip
			}
			);
			
			return position;
		}
/*
		public static Texture2D PaintWaveformSpectrum(AudioClip audio, float saturation, int width, int height, Color col, Color bgCol) {
			Texture2D tex = new Texture2D(width, height, TextureFormat.RGBA32, false);
			if(audio.loadType != AudioClipLoadType.DecompressOnLoad)
			{
				//unable to render.

				for (int x = 0; x < width; x++) {
					for (int y = 0; y < height; y++) {
						tex.SetPixel(x, y, bgCol);
					}
				}

				tex.Apply();
			}
			else
			{
				float[] samples = new float[audio.samples * audio.channels];
				float[] waveform = new float[width];

				audio.GetData(samples, 0);
				float packSize = ((float)samples.Length / (float)width);
				int s = 0;
				for (float i = 0; Mathf.RoundToInt(i) < samples.Length && s < waveform.Length; i += packSize)
				{
					waveform[s] = Mathf.Abs(samples[Mathf.RoundToInt(i)]);
					s++;
				}
			
				for (int x = 0; x < width; x++) {
					for (int y = 0; y < height; y++) {
						tex.SetPixel(x, y, bgCol);
					}
				}
			
				for (int x = 0; x < waveform.Length; x++) {
					for (int y = 0; y <= waveform[x] * ((float)height * .75f); y++) {
						tex.SetPixel(x, ( height / 2 ) + y, col);
						tex.SetPixel(x, ( height / 2 ) - y, col);
					}
				}
				tex.Apply();
			}
			return tex;
     	}
		*/
	} 
	
	#endif


}