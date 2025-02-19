using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Interaction : MonoBehaviour
{
    private ManoGestureContinuous grab;
    private ManoGestureContinuous pinch;
    private ManoGestureTrigger click;
    private ManoGestureTrigger pick;
    private ManoGestureTrigger drop;

    [SerializeField]
    private Material[] arCubeMaterial;
    //[SerializeField]
    //private GameObject smallCube;

    private string handTag = "Player";
    private Renderer cubeRenderer;

    void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        grab = ManoGestureContinuous.CLOSED_HAND_GESTURE;
        pinch = ManoGestureContinuous.HOLD_GESTURE;
        click = ManoGestureTrigger.CLICK;
        pick = ManoGestureTrigger.PICK;
        drop = ManoGestureTrigger.DROP;

        cubeRenderer = GetComponent<Renderer>();
        cubeRenderer.sharedMaterial = arCubeMaterial[0];
        cubeRenderer.material = arCubeMaterial[0];
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="other">The collider that stays</param>

    private void OnTriggerStay(Collider other)
    {

        MoveWhenPick(other);
        if (other.transform.childCount == 0)
        {
            RotateWhenHolding(other);
        }

        //SpawnWhenClicking(other);
    }

    /// <summary>
    /// If grab is performed while hand collider is in the cube.
    /// The cube will follow the hand.
    /// </summary>
    private void MoveWhenPick(Collider other)
    {

        if (ManomotionManager.Instance.Hand_infos[0].hand_info.gesture_info.mano_gesture_trigger == pick)
        {
            transform.parent = other.gameObject.transform;
            GameManager.I.isHolding = true;
            //GameManager.I.txt_debug.text = "pick";
        }
        else if(ManomotionManager.Instance.Hand_infos[0].hand_info.gesture_info.mano_gesture_trigger == drop)
        {
            transform.parent = null;
            GameManager.I.isHolding = false;
            GameManager.I.Btn_SetChar.SetActive(true);
            //GameManager.I.txt_debug.text = "drop";
        }
        
        
    }

    /// <summary>
    /// If pinch is performed while hand collider is in the cube.
    /// The cube will start rotate.
    /// </summary>
    private void RotateWhenHolding(Collider other)
    {
        if (ManomotionManager.Instance.Hand_infos[0].hand_info.gesture_info.mano_gesture_continuous == pinch)
        {
            transform.parent = other.gameObject.transform;
            //transform.Rotate(Vector3.up * Time.deltaTime * 50, Space.World);
        }
    }

    /// <summary>
    /// If pick is performed while hand collider is in the cube.
    /// The cube will follow the hand.
    /// </summary>
    

    /// <summary>
    /// Vibrate when hand collider enters the cube.
    /// </summary>
    /// <param name="other">The collider that enters</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == handTag)
        {
            cubeRenderer.sharedMaterial = arCubeMaterial[1];
            Handheld.Vibrate();
        }
    }

    /// <summary>
    /// Change material when exit the cube
    /// </summary>
    /// <param name="other">The collider that exits</param>
    private void OnTriggerExit(Collider other)
    {
        cubeRenderer.sharedMaterial = arCubeMaterial[0];
    }
}