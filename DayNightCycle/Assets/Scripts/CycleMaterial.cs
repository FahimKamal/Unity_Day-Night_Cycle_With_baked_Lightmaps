using System;
using UnityEngine;

public class CycleMaterial : MonoBehaviour{
    [SerializeField] private Material firstMat;
    [SerializeField] private Material secondMat;
    [SerializeField] private Renderer targetRenderer;

    private bool _isChangeTriggered = false;
    private float _changeDuration;
    private float _timer;

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
            _changeDuration = dayNightTriggerData.ChangeDuration;
            _isChangeTriggered = dayNightTriggerData.ChangeTrigger;
        }
    }


    private void Update(){
        if (!_isChangeTriggered) return;
        
        var lerp = _timer / _changeDuration;
        targetRenderer.material.Lerp(firstMat, secondMat, lerp);
        _timer += Time.deltaTime;
        if (_timer >= _changeDuration){
            _isChangeTriggered = false;
            _timer = 0;
            (firstMat, secondMat) = (secondMat, firstMat);
        }
    }
}
