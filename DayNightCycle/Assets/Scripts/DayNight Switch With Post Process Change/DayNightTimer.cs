using UnityEngine;

public class DayNightTimer : MonoBehaviour{
    [SerializeField] private float changeInterval = 30;
    [SerializeField] DayNightCycleManager dayNightCycleManager;

    private float _timer;
    private bool _isDayActive = true;

    private void OnValidate(){
        dayNightCycleManager = GetComponent<DayNightCycleManager>();
    }

    private void Update(){
        // Only Proceed if dayNight is not changing. 
        if (dayNightCycleManager.IsSwitching) return;
        _timer += Time.deltaTime;
        
        // If timer is not greater than change Interval return from here. 
        if (!(_timer >= changeInterval)) return;
        
        if (_isDayActive){
            dayNightCycleManager.SwitchNight();
            _isDayActive = false;
        }
        else{
            dayNightCycleManager.SwitchDay();
            _isDayActive = true;
        }

        _timer = 0;
    }


}
