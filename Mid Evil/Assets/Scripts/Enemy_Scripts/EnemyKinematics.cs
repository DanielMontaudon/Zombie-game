using UnityEngine;

public class EnemyKinematics : MonoBehaviour
{
    [SerializeField] Rigidbody headRB;
    [SerializeField] Rigidbody chestRB;
    [SerializeField] public Rigidbody hipRB;
    [SerializeField] Rigidbody leftUpperArmRB;
    [SerializeField] Rigidbody leftLowerRB;
    [SerializeField] Rigidbody rightUpperArmRB;
    [SerializeField] Rigidbody rightLowerArmRB;
    [SerializeField] Rigidbody leftUpperLegRB;
    [SerializeField] Rigidbody leftLowerLegRB;
    [SerializeField] Rigidbody rightUpperLegRB;
    [SerializeField] Rigidbody rightLowerLegRB;

    /*
        headRB
        chestRB
        hipRB
        leftUpperArmRB
        leftLowerRB
        rightUpperArmRB
        rightLowerArmRB
        leftUpperLegRB
        leftLowerLegRB
        rightUpperLegRB
        rightLowerLegRB
     */
    private void Start()
    {
        headRB.isKinematic = true;
        chestRB.isKinematic = true;
        hipRB.isKinematic = true;
        leftUpperArmRB.isKinematic = true;
        leftLowerRB.isKinematic = true;
        rightUpperArmRB.isKinematic = true;
        rightLowerArmRB.isKinematic = true;
        leftUpperLegRB.isKinematic = true;
        leftLowerLegRB.isKinematic = true;
        rightUpperLegRB.isKinematic = true;
        rightLowerLegRB.isKinematic = true;
    }

    public void turnOnKinematics()
    {
        headRB.isKinematic = true;
        chestRB.isKinematic = true;
        hipRB.isKinematic = true;
        leftUpperArmRB.isKinematic = true;
        leftLowerRB.isKinematic = true;
        rightUpperArmRB.isKinematic = true;
        rightLowerArmRB.isKinematic = true;
        leftUpperLegRB.isKinematic = true;
        leftLowerLegRB.isKinematic = true;
        rightUpperLegRB.isKinematic = true;
        rightLowerLegRB.isKinematic = true;
    }

    public void turnOffKinematics()
    {
        headRB.isKinematic = false;
        chestRB.isKinematic = false;
        hipRB.isKinematic = false;
        leftUpperArmRB.isKinematic = false;
        leftLowerRB.isKinematic = false;
        rightUpperArmRB.isKinematic = false;
        rightLowerArmRB.isKinematic = false;
        leftUpperLegRB.isKinematic = false;
        leftLowerLegRB.isKinematic = false;
        rightUpperLegRB.isKinematic = false;
        rightLowerLegRB.isKinematic = false;
    }


    public void turnOnGravity()
    {
        headRB.useGravity = true;
        chestRB.useGravity = true;
        hipRB.useGravity = true;
        leftUpperArmRB.useGravity = true;
        leftLowerRB.useGravity = true;
        rightUpperArmRB.useGravity = true;
        rightLowerArmRB.useGravity = true;
        leftUpperLegRB.useGravity = true;
        leftLowerLegRB.useGravity = true;
        rightUpperLegRB.useGravity = true;
        rightLowerLegRB.useGravity = true;
    }

    public void turnOffGravity()
    {
        headRB.useGravity = false;
        chestRB.useGravity = false;
        hipRB.useGravity = false;
        leftUpperArmRB.useGravity = false;
        leftLowerRB.useGravity = false;
        rightUpperArmRB.useGravity = false;
        rightLowerArmRB.useGravity = false;
        leftUpperLegRB.useGravity = false;
        leftLowerLegRB.useGravity = false;
        rightUpperLegRB.useGravity = false;
        rightLowerLegRB.useGravity = false;
    }    

    public void FreezeConstraints()
    {
        headRB.constraints = RigidbodyConstraints.FreezeAll;
        chestRB.constraints = RigidbodyConstraints.FreezeAll;
        hipRB.constraints = RigidbodyConstraints.FreezeAll;
        leftUpperArmRB.constraints = RigidbodyConstraints.FreezeAll;
        leftLowerRB.constraints = RigidbodyConstraints.FreezeAll;
        rightUpperArmRB.constraints = RigidbodyConstraints.FreezeAll;
        rightLowerArmRB.constraints = RigidbodyConstraints.FreezeAll;
        leftUpperLegRB.constraints = RigidbodyConstraints.FreezeAll;
        leftLowerLegRB.constraints = RigidbodyConstraints.FreezeAll;
        rightUpperLegRB.constraints = RigidbodyConstraints.FreezeAll;
        rightLowerLegRB.constraints = RigidbodyConstraints.FreezeAll;
    }  
    
    public void FreeConstraints()
    {
        headRB.constraints = RigidbodyConstraints.None;
        chestRB.constraints = RigidbodyConstraints.None;
        hipRB.constraints = RigidbodyConstraints.None;
        leftUpperArmRB.constraints = RigidbodyConstraints.None;
        leftLowerRB.constraints = RigidbodyConstraints.None;
        rightUpperArmRB.constraints = RigidbodyConstraints.None;
        rightLowerArmRB.constraints = RigidbodyConstraints.None;
        leftUpperLegRB.constraints = RigidbodyConstraints.None;
        leftLowerLegRB.constraints = RigidbodyConstraints.None;
        rightUpperLegRB.constraints = RigidbodyConstraints.None;
        rightLowerLegRB.constraints = RigidbodyConstraints.None;
    }

    public void chestKnockback(Vector3 forcePosition, float force)
    {
        chestRB.AddTorque((transform.position - forcePosition).normalized * 5, ForceMode.Impulse);
        chestRB.AddForce((transform.position - forcePosition).normalized * force);
    }

    public void liftKinematics(float force)
    {
        chestRB.AddForce(transform.up * force);
    }

}
