using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class LerpMaterial : MonoBehaviour{
    [SerializeField] private AnimationCurve lightChangeCarve;
    [SerializeField] private Material dayMaterial;
    [SerializeField] private Material nightMaterial;

    [SerializeField] private Renderer targetRenderer;
    [SerializeField] private float duration;

    [SerializeField] private Button dayBtn;
    [SerializeField] private Button nightBtn;

    [SerializeField] private Volume dayPostProcessVolume;
    [SerializeField] private Volume nightPostProcessVolume;
    
    
    private bool _switchDay;
    private bool _switchNight;
    private float _timer;

    private void Awake(){
        // _timer = duration;
        dayBtn.onClick.AddListener(() => {
            _switchDay = true;
            _switchNight = false;
        });
        nightBtn.onClick.AddListener(() => {
            _switchDay = false;
            _switchNight = true;
        });
        
        
    }

    private void Update(){
        if (_switchDay){
            var lerp = _timer / duration;
            targetRenderer.material.Lerp (nightMaterial, dayMaterial, lerp);
            dayPostProcessVolume.weight = Mathf.Lerp(0, 1, lerp);
            nightPostProcessVolume.weight = Mathf.Lerp(1, 0, lerp);
            _timer += Time.deltaTime;
            if (_timer >= duration){
                _switchDay = false;
                _timer = 0;
            }
        }

        if (_switchNight){
            var lerp =  _timer / duration;
            targetRenderer.material.Lerp (dayMaterial, nightMaterial, lerp);
            dayPostProcessVolume.weight = Mathf.Lerp(1, 0, lerp);
            nightPostProcessVolume.weight = Mathf.Lerp(0, 1, lerp);
            _timer += Time.deltaTime;
            if (_timer >= duration){
                _switchNight = false;
                _timer = 0;
            }            
        }
    }

    private IEnumerator SwitchDay(){
        var timer = duration;
        while (timer > 0){
            timer -= Time.deltaTime;
            var lerp = duration / timer;
            targetRenderer.material.Lerp (dayMaterial, nightMaterial, lerp);
            yield return null;
        }

    }

    private IEnumerator SwitchNight(){
        var timer = duration;
        while (timer > 0){
            timer -= Time.deltaTime;
            var lerp = duration / timer;
            targetRenderer.material.Lerp(nightMaterial, dayMaterial, lerp);
            yield return null;
        }
    }
    
}
