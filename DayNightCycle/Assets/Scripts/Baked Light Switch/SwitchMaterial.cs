using UnityEngine;

namespace Baked_Light_Switch{
    public class SwitchMaterial : MonoBehaviour{
        [SerializeField] private Material dayMat;
        [SerializeField] private Material nightMat;
    
        [SerializeField] private Renderer targetRenderer;

        private void OnValidate(){
            targetRenderer = GetComponent<Renderer>();
        }

    
        private void Start(){
            SwitchLights.Instance.OnLightSwitch += SwitchLights_OnLightSwitch;
        }

        private void SwitchLights_OnLightSwitch(object sender, bool e){
            targetRenderer.material = e ? dayMat : nightMat;
        }
    }
}
