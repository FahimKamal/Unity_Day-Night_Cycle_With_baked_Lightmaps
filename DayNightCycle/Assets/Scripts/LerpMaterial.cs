using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class LerpMaterial : MonoBehaviour{
    [SerializeField] private Material dayMaterial;
    [SerializeField] private Material nightMaterial;
    [SerializeField] private Renderer targetRenderer;
    
    [SerializeField] private Volume dayPostProcessVolume;
    [SerializeField] private Volume nightPostProcessVolume;
    
    [SerializeField] private float changeDuration;

    [SerializeField] private Button dayBtn;
    [SerializeField] private Button nightBtn;
    
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
            var lerp = _timer / changeDuration;
            targetRenderer.material.Lerp (nightMaterial, dayMaterial, lerp);
            dayPostProcessVolume.weight = Mathf.Lerp(0, 1, lerp);
            nightPostProcessVolume.weight = Mathf.Lerp(1, 0, lerp);
            _timer += Time.deltaTime;
            if (_timer >= changeDuration){
                _switchDay = false;
                _timer = 0;
            }
        }

        if (_switchNight){
            var lerp =  _timer / changeDuration;
            targetRenderer.material.Lerp (dayMaterial, nightMaterial, lerp);
            dayPostProcessVolume.weight = Mathf.Lerp(1, 0, lerp);
            nightPostProcessVolume.weight = Mathf.Lerp(0, 1, lerp);
            _timer += Time.deltaTime;
            if (_timer >= changeDuration){
                _switchNight = false;
                _timer = 0;
            }            
        }
    }
}
