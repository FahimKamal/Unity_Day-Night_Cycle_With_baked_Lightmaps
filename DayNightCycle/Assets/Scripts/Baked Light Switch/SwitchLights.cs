using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Baked_Light_Switch{
    /// <summary>
    /// The class below stitch between 2 lightmap sets. Like switching from day to night.
    /// But does that in an instance. Like switching a button. What I want is a smooth
    /// transition between the two over time. Have to look for a way.
    ///
    /// More Research needed. 
    /// </summary>
    public class SwitchLights : MonoBehaviour{
        public static SwitchLights Instance{ get; private set; }
        [SerializeField] private List<Texture2D> darkLightmapDir;
        [SerializeField] private List<Texture2D> darkLightmapColor;
        [SerializeField] private List<Texture2D> brightLightmapDir;
        [SerializeField] private List<Texture2D> brightLightmapColor;
        [SerializeField] private GameObject dayPostProcess;
        [SerializeField] private GameObject nightPostProcess;
        // Duration of the transition in seconds
        [SerializeField] private float transitionDuration = 10f;
        
        [SerializeField] private Button dayBtn;
        [SerializeField] private Button nightBtn;

        public event EventHandler<bool> OnLightSwitch; 

        private LightmapData[] _darkLightMap;
        private LightmapData[] _brightLightMap;

        private void Awake(){
            Instance = this;
            dayBtn.onClick.AddListener(SwitchDay);
            nightBtn.onClick.AddListener(SwitchNight);
        }
        
        /// <summary>
        /// Get LightmapDir and LightmapColor and store them in LightmapData format.
        /// </summary>
        /// <param name="lightmapDir"></param>
        /// <param name="lightmapColor"></param>
        /// <returns>Returns a array of LightMapData.</returns>
        private LightmapData[] LightmapDataConverter(List<Texture2D> lightmapDir, List<Texture2D> lightmapColor){
            var lightmaps = new List<LightmapData>();
            for (var i = 0; i < lightmapDir.Count; i++){
                var lmData = new LightmapData{
                    lightmapDir = lightmapDir[i],
                    lightmapColor = lightmapColor[i]
                };
                lightmaps.Add(lmData);
            }
            return lightmaps.ToArray();
        }

        private void Start(){
            // Get darkLightmapDir and darkLightmapColor and store them in darkLightMap of LightmapData format.
            _darkLightMap = LightmapDataConverter(darkLightmapDir, darkLightmapColor);
        
            // Get brightLightmapDir and brightLightmapColor and store them in brightLightMap of LightmapData format.
            _brightLightMap = LightmapDataConverter(brightLightmapDir, brightLightmapColor);
        }

        private void SwitchDay(){
            LightmapSettings.lightmaps = _brightLightMap;
            // StartCoroutine(TransitionLightmaps(LightmapSettings.lightmaps, _brightLightMap));
            dayPostProcess.SetActive(true);
            nightPostProcess.SetActive(false);
            OnLightSwitch?.Invoke(this, true);
        }

        private void SwitchNight(){
            LightmapSettings.lightmaps = _darkLightMap; 
            // StartCoroutine(TransitionLightmaps(LightmapSettings.lightmaps, _darkLightMap));
            dayPostProcess.SetActive(false);
            nightPostProcess.SetActive(true);
            OnLightSwitch?.Invoke(this, false);
        }

        /*
         * Codes below works but very much CPU expensive. Have to do more research.  
         */
        
        #region Experimental code

        private IEnumerator TransitionLightmaps(LightmapData[] from, LightmapData[] to)
        {
            float elapsedTime = 0f;

            while (elapsedTime < transitionDuration)
            {
                float t = Mathf.Clamp01(elapsedTime / transitionDuration);

                // Interpolate between lightmaps
                LightmapSettings.lightmaps = InterpolateLightmaps(from, to, t);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Ensure that the final lightmap is set
            LightmapSettings.lightmaps = to;
        }

        private LightmapData[] InterpolateLightmaps(LightmapData[] from, LightmapData[] to, float t)
        {
            var interpolatedLightmaps = new LightmapData[from.Length];

            for (int i = 0; i < from.Length; i++)
            {
                var interpolatedData = new LightmapData
                {
                    lightmapDir = TextureLerp(from[i].lightmapDir, to[i].lightmapDir, t),
                    lightmapColor = TextureLerp(from[i].lightmapColor, to[i].lightmapColor, t)
                };

                interpolatedLightmaps[i] = interpolatedData;
            }

            return interpolatedLightmaps;
        }

        private Texture2D TextureLerp(Texture2D from, Texture2D to, float t)
        {
            Color[] fromPixels = GetPixelsSafe(from);
            Color[] toPixels = GetPixelsSafe(to);

            if (fromPixels.Length != toPixels.Length)
            {
                Debug.LogError("Texture sizes do not match for interpolation.");
                return null;
            }

            for (int i = 0; i < fromPixels.Length; i++)
            {
                fromPixels[i] = Color.Lerp(fromPixels[i], toPixels[i], t);
            }

            Texture2D result = new Texture2D(from.width, from.height);
            result.SetPixels(fromPixels);
            result.Apply();

            return result;
        }

        private Color[] GetPixelsSafe(Texture2D texture)
        {
            if (texture.isReadable)
            {
                return texture.GetPixels();
            }
            else
            {
                // Debug.LogWarning("Texture is not readable. Creating a temporary readable copy.");
                RenderTexture rt = RenderTexture.GetTemporary(texture.width, texture.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);
                Graphics.Blit(texture, rt);
                RenderTexture previous = RenderTexture.active;
                RenderTexture.active = rt;
                Texture2D tempTexture = new Texture2D(texture.width, texture.height);
                tempTexture.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
                tempTexture.Apply();
                RenderTexture.active = previous;
                RenderTexture.ReleaseTemporary(rt);
                return tempTexture.GetPixels();
            }
        }

        #endregion
        
        

    }
}
