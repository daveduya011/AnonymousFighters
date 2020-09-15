using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform boundaryLeft, boundaryRight;
    public Transform ground;

    [HideInInspector]
    public float minX, maxX, yPos;
    private new Camera camera;
    public float focusSpeed = 0.2f;
    public GameObject focusedObject;

    private Vector3 lastPlayerPos = Vector3.zero;
    private Vector3 targetDistance = Vector3.zero;
    private float distanceToMove;
    private bool isViewMode;
    private float initOrtographicSize;
    private PlayerController player;

    // Start is called before the first frame update
    void Start() {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        camera = this.GetComponent<Camera>();
        transform.position = new Vector3(lastPlayerPos.x, transform.position.y, transform.position.z);
        initOrtographicSize = GetComponent<Camera>().orthographicSize;

        if (focusedObject == null) {
            focusedObject = player.gameObject;
        }

        //checkPos();
        //Invoke("checkPos", 0f);
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyUp(KeyCode.Alpha1)) {
            isViewMode = !isViewMode;
            if (isViewMode) {
                //checkPos();
            }
            else {
                GetComponent<Camera>().orthographicSize = initOrtographicSize;
            }
        }
        targetDistance = new Vector3(focusedObject.transform.position.x, transform.position.y, transform.position.z);
        camera.transform.position =
            Vector3.Lerp(camera.transform.position, targetDistance, focusSpeed);
        reCalculateCamera();
    }
    private void reCalculateCamera() {
        minX = boundaryLeft.position.x + camera.orthographicSize * camera.aspect;
        maxX = boundaryRight.position.x - camera.orthographicSize * camera.aspect;

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, minX, maxX),
            transform.position.y, -10);
    }
}
