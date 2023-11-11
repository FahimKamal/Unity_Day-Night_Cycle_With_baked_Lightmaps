using System;
using UnityEngine;
using UnityEngine.Serialization;

public class CycleMaterial : MonoBehaviour{
    [SerializeField] private Material dayMaterial;
    [SerializeField] private Material nightMaterial;
    [SerializeField] private Renderer targetRenderer;

    private bool _isChangeTriggered = false;
    private float _changeDuration;
    private float _timer;
    private DayNightCycleManager.State _state;

    private void OnValidate(){
        targetRenderer = GetComponent<Renderer>();
    }

    private void Start(){
        DayNightCycleManager.Instance.OnTriggerMaterialCycle += DayNightCycleManager_OnTriggerMaterialCycle;
    }

    private void OnDisable(){
        DayNightCycleManager.Instance.OnTriggerMaterialCycle -= DayNightCycleManager_OnTriggerMaterialCycle;
    }

    private void DayNightCycleManager_OnTriggerMaterialCycle(object sender, DayNightCycleManager.DayNightTriggerData dayNightTriggerData){
        if (!_isChangeTriggered){
            _state = dayNightTriggerData.State;
            _changeDuration = dayNightTriggerData.ChangeDuration;
            _isChangeTriggered = dayNightTriggerData.ChangeTrigger;
        }
    }


    private void Update(){
        if (!_isChangeTriggered) return;
        
        var lerp = _timer / _changeDuration;
        switch (_state){
            case DayNightCycleManager.State.Day:
                targetRenderer.material.Lerp(nightMaterial, dayMaterial, lerp);
                break;
            case DayNightCycleManager.State.Night:
                targetRenderer.material.Lerp(dayMaterial, nightMaterial, lerp);
                break;
        }
        
        _timer += Time.deltaTime;
        if (_timer >= _changeDuration){
            _isChangeTriggered = false;
            _timer = 0;
        }
    }
}
