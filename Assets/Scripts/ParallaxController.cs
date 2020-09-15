using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxController : MonoBehaviour
{
    public float speed;
    private float cameraPositionX;
    private CameraController cameraController;
    private Parallax[] parallaxes;
    private Vector3 lastCameraPos;
    private float sizeX;

    // Start is called before the first frame update
    void Start()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
        parallaxes = new Parallax[3];
        parallaxes[1] = GetComponentInChildren<Parallax>();

        Vector3 pos = parallaxes[1].transform.position;

        sizeX = parallaxes[1].GetComponent<SpriteRenderer>().bounds.size.x;

        parallaxes[0] = Instantiate(parallaxes[1], new Vector3(pos.x - sizeX, pos.y, pos.z), Quaternion.identity, transform);
        parallaxes[2] = Instantiate(parallaxes[1], new Vector3(pos.x + sizeX, pos.y, pos.z), Quaternion.identity, transform);
    }

    // Update is called once per frame
    void Update()
    {
        // Create parallax effect if graphics is not set to low
        float moveX = lastCameraPos.x - cameraController.transform.position.x;
        transform.position += Vector3.right * moveX * speed;

        cameraPositionX = cameraController.transform.position.x;

        for (int i = 0; i < parallaxes.Length; i++) {
            float pos = parallaxes[i].transform.position.x;

            parallaxes[i].isCameraInPosition = cameraPositionX > pos - (sizeX / 2)
                && cameraPositionX < pos + (sizeX / 2);

            if (parallaxes[i].isCameraInPosition) {
                if (!parallaxes[i].isLastVisited) {
                    Vector3 currentpos = parallaxes[i].transform.position;
                    parallaxes[i].isLastVisited = true;

                    parallaxes[(i + 2) % 3].transform.position = currentpos - (Vector3.right * sizeX);
                    parallaxes[(i + 1) % 3].transform.position = currentpos + (Vector3.right * sizeX);
                }
            } else {
                parallaxes[i].isLastVisited = false;
            }
        }

        lastCameraPos = cameraController.transform.position;
    }

    // The calculation on the above method is derived from this

    //if (parallaxes[0].isCameraInPosition && currentPos != 1) {
    //    parallaxes[2].transform.position = parallaxes[0].transform.position - (Vector3.right * parallaxes[1].sizeX);
    //    parallaxes[1].transform.position = parallaxes[0].transform.position + (Vector3.right * parallaxes[1].sizeX);
    //    currentPos = 1;
    //}
    //else if (parallaxes[1].isCameraInPosition && currentPos != 0) {
    //    parallaxes[0].transform.position = parallaxes[1].transform.position - (Vector3.right * parallaxes[1].sizeX);
    //    parallaxes[2].transform.position = parallaxes[1].transform.position + (Vector3.right * parallaxes[1].sizeX);
    //    currentPos = 0;
    //}
    //else if (parallaxes[2].isCameraInPosition && currentPos != 2) {
    //    parallaxes[1].transform.position = parallaxes[2].transform.position - (Vector3.right * parallaxes[1].sizeX);
    //    parallaxes[0].transform.position = parallaxes[2].transform.position + (Vector3.right * parallaxes[1].sizeX);
    //    currentPos = 2;
    //}
}
