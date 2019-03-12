using UnityEngine;

public class SimpleParallaxScrolling : MonoBehaviour {

    [Range(0.1f, 5f)]
    public float speed = 1f;

    private Transform[] layers;
    private Transform cam;
    private float startX;
    private float increment;

    private void Start() {

        // Cache our camera transform because Camera.main shouldn't be used in the main update loop.
        cam = Camera.main.transform;

        // Set the size of our layers array to the number of children we currently have under the parallax object.
        layers = new Transform[transform.childCount];

        // Loop through each of those children.
        int childCount = 0;
        foreach (Transform child in transform) {

            // Assign the children to the layers array.
            layers[childCount] = child;

            // Loop through any children of this child and check for SpriteRenderers. If we find any, set the sorting order of to our current childCount.
            // We're working back to front so the order of the layers is also the sorting order of the sprites.
            foreach (Transform childOfchild in child) {

                SpriteRenderer sR = childOfchild.GetComponent<SpriteRenderer>();
                if (sR != null)
                    sR.sortingOrder = childCount;

            }

            childCount++;

        }

        // Save our camera's starting X position so we can always tell how far the camera has moved. Also cache an increment value which we can use to
        // give each layer a different scrolling speed. An increment value of 1 would mean the layer is moving away from the camera at the same speed
        // as the camera is moving.
        startX = cam.position.x;
        increment = speed / layers.Length;

    }

    private void Update() {

        // Loop through each layer and adjust the position relative to the camera.
        int count = 0;
        foreach (Transform layer in layers) {

            // We calculate our positions based on the distance the camera has travelled from its starting position. We then get our multiplier value by multiplying
            // the increment by the layer number. Eg, the very first layer is 0, and so will have an increment of 0, and as anything times by 0 is 0, that layer will not
            // not move. Finally we get our position offset by multiplying the distance the camera has travelled by the multiplier.
            float cameraTravel = startX - cam.position.x;
            float multiplier = increment * count;
            float xOffset = cameraTravel * multiplier;

            layer.position = new Vector3(cam.position.x + xOffset, layer.position.y, layer.position.z);
            count++;

        }

        // Move our camera along so we can see that sweet parallax in action.
        cam.position = Vector3.MoveTowards(cam.position, new Vector3(cam.position.x + 0.5f, cam.position.y, cam.position.z), Time.deltaTime * 2f);

    }

}
