using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Game Objects")]
    //public Camera playerCam;
    public Transform leftHand;
    public Transform rightHand;

    [Header("Loadout")]
    public Spell leftSpell;
    public Spell rigtSpell;

    [Header("Keybinds")]
    public KeyCode leftKeybind = KeyCode.Mouse0;
    public KeyCode rightKeybind = KeyCode.Mouse1;

    float startY;

    private void Start()
    {
        startY = leftHand.localPosition.y;
    }

    // Update is called once per frame
    void Update()
    {
        MovementHandle();
    }

    void MovementHandle()
    {
        if (Input.GetKey(leftKeybind))
        {
            Vector3 raisedHeight = new Vector3(leftHand.localPosition.x, startY + 0.2f, leftHand.localPosition.z);
            leftHand.localPosition = Vector3.Lerp(leftHand.localPosition, raisedHeight, Time.deltaTime * 10f);
        }
        else
        {
            Vector3 raisedHeight = new Vector3(leftHand.localPosition.x, startY, leftHand.localPosition.z);
            leftHand.localPosition = Vector3.Lerp(leftHand.localPosition, raisedHeight, Time.deltaTime * 10f);
        }

        if (Input.GetKey(rightKeybind))
        {
            print("Pressed");
            Vector3 raisedHeight = new Vector3(rightHand.localPosition.x, startY + 0.2f, rightHand.localPosition.z);
            rightHand.localPosition = Vector3.Lerp(rightHand.localPosition, raisedHeight, Time.deltaTime * 10f);
        }
        else
        {
            print("Let go");
            Vector3 raisedHeight = new Vector3(rightHand.localPosition.x, startY, rightHand.localPosition.z);
            rightHand.localPosition = Vector3.Lerp(rightHand.localPosition, raisedHeight, Time.deltaTime * 10f);
        }
    }
}
