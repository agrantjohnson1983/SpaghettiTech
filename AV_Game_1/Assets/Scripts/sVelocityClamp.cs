using UnityEngine;

// Attach to any pushable/grabbable Rigidbody (boxes, truss, etc.) to stop a
// quick bump from launching it across the room, while still allowing it to
// be pushed normally by sustained contact (e.g. walking into it).
//
// A single sharp collision imparts a large instantaneous velocity - that is
// the "sent flying" problem. A sustained push (continuous contact from
// walking into something) keeps re-applying a smaller force every physics
// step, so the object's speed stays low and controllable even without a
// clamp. Capping velocity magnitude each FixedUpdate targets exactly the
// bump case without preventing pushing.
public class sVelocityClamp : MonoBehaviour
{
    Rigidbody _rb;

    [Header("Speed Caps")]
    // Highest speed (units/sec) this object is allowed to move under
    // physics collision - tune this to about how fast you want a push to
    // feel. A hard bump that would have imparted much more than this gets
    // clamped down to it instead of launching the object.
    public float maxLinearSpeed = 3f;

    // Same idea for spin - stops a bump from sending an object tumbling.
    public float maxAngularSpeed = 5f;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (_rb == null)
        {
            return;
        }

        // While grabbed (constrained by a FixedJoint to the player), the
        // joint itself controls motion - clamping is harmless here since
        // the object moves with the player rather than under free physics,
        // but skipping it avoids any unnecessary velocity changes while held.
        if (TryGetComponent<iGrabbable>(out iGrabbable _grabbable))
        {
            //_grabbable.IsGrabbed = true;
            return;
        }

        if (_rb.velocity.magnitude > maxLinearSpeed)
        {
            _rb.velocity = _rb.velocity.normalized * maxLinearSpeed;
        }

        if (_rb.angularVelocity.magnitude > maxAngularSpeed)
        {
            _rb.angularVelocity = _rb.angularVelocity.normalized * maxAngularSpeed;
        }
    }
}
