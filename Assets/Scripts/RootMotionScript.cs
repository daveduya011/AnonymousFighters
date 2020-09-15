using UnityEngine;

public class RootMotionScript : MonoBehaviour
{
    //This script allows you to move the character position based on the animation set to the gameobject
    void OnAnimatorMove() {
        Animator animator = GetComponent<Animator>();
        if (animator) {
            transform.position += animator.deltaPosition / Time.deltaTime / 4f;
        }
    }
}
