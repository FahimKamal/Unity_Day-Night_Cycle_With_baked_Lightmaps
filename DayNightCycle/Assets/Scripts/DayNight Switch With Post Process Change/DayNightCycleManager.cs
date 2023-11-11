using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class DayNightCycleManager : MonoBehaviour{
    public static DayNightCycleManager Instance{ get; private set; }
    [SerializeField] private float changeDuration;

    [SerializeField] private Volume dayPostProcessVolume;
    [SerializeField] private Volume nightPostProcessVolume;


    [SerializeField] private Button dayBtn;
    [SerializeField] private Button nightBtn;
    
    public enum State{
        Day,
        Night
    }
    
    private bool _switchDay;
    private bool _switchNight;
    public bool IsSwitching{ get; private set; }
    private float _timer;

    public event EventHandler<DayNightTriggerData> OnTriggerMaterialCycle; 
    
    public class DayNightTriggerData: EventArgs{
        public State State;
        public bool ChangeTrigger;
        public float ChangeDuration;
        
    }

    private void Awake(){
        Instance = this;
        dayBtn.onClick.AddListener(SwitchDay);
        nightBtn.onClick.AddListener(SwitchNight);
    }

    public void SwitchDay(){
        _switchDay = true;
        _switchNight = false;
    }
    public void SwitchNight(){
        _switchDay = false;
        _switchNight = true;
    }

    private void Update(){
        if (_switchDay){
            IsSwitching = true;
            var lerp = _timer / changeDuration;
            OnTriggerMaterialCycle?.Invoke(this, new DayNightTriggerData{
                State = State.Day,
                ChangeTrigger = true,
                ChangeDuration = changeDuration
            });
            dayPostProcessVolume.weight = Mathf.Lerp(0, 1, lerp);
            nightPostProcessVolume.weight = Mathf.Lerp(1, 0, lerp);
            _timer += Time.deltaTime;
            if (_timer >= changeDuration){
                _switchDay = false;
                IsSwitching = false;
                _timer = 0;
            }
        }

        if (_switchNight){
            IsSwitching = true;
            var lerp =  _timer / changeDuration;
            OnTriggerMaterialCycle?.Invoke(this, new DayNightTriggerData{
                State = State.Night,
                ChangeTrigger = true,
                ChangeDuration = changeDuration
            });
            dayPostProcessVolume.weight = Mathf.Lerp(1, 0, lerp);
            nightPostProcessVolume.weight = Mathf.Lerp(0, 1, lerp);
            _timer += Time.deltaTime;
            if (_timer >= changeDuration){
                _switchNight = false;
                IsSwitching = false;
                _timer = 0;
            }            
        }
    }
}
