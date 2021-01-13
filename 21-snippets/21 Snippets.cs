// 1. QUIT

// If player presses ESCAPE, game quits. Does not work in editor.
if (Input.GetKeyDown(KeyCode.Escape)) Applciation.Quit();

// 2. PAUSE GAME

// Keeps track of whether game is paused with a private bool while presenting a public
// bool for modification. If changed, game's timeScale is modified appropriately.
private bool _pause;
public bool pause {
  get { return _pause; }
  set {
    _paused = value;
    Time.timeScale = (_pause) ? 0f : 1f;
  }
}

// 3. CAMERA REFERENCE

// Stores a reference to main scene camera.
Camera _camera;
void Awake() {
    _camera = Camera.Main;
}

// 4. BASIC MOVEMENT

// Takes basic up/down/left/right input and moves the object (in 3D space) accordingly.
float speed = 10f; // Change if needed.
float horizontal = Input.GetAxisRaw("Horizontal");
float vertical = Input.GetAxisRaw("Vertical");
transform.position = Vector3.MoveTowards(transform.position,
                                        transform.position +
                                        (transform.forward * vertical) +
                                        (transform.right * horizontal),
                                        Time.deltaTime * speed);

// 5. BASIC LOOK CAMERA

// Gives camera basic mouse look functionality. In order to work, camera must be childed
// to a separate GameObject. This code must be on that GameObject, and CAMERATRANSFORM must be
// replaced with a reference to the Camera's transform.
float mouseSensitivy = 10f;
float mouseHorizontal = Input.GetAxisRaw("Mouse X");
float mouseVertical = Input.GetAxisRaw("Mouse Y");
transform.Rotate (Vector3.up * mouseHorizontal * Time.deltaTime * mouseSensitivity);
CAMERATRANSFORM.Rotate (Vector3.right * -mouseVertical * Time.deltaTime * mouseSensitivity);

// 6. HIGH SCORE SAVING

// Creates a public int that will store a highscore value that can be retrieved in future
// playthroughs. If no high score value has been saved, it returns zero.
public int HighScore {
  get { return PlayerPrefs.GetInt("HighScore", 0); }
  set { PlayerPrefs.SetInt("HighScore", value); }
}

// 7. "TICK" COUNTER

// Registers a "tick" in the console every second. "AdvanceTick()" must be called in Update().
private float counter;
void AdvanceTick() {
  if (Time.time > counter) {
    Debug.Log("tick");
    counter = Time.time + 1f; // Change 1f for different intervals.
  }
}

// 8. COLLISION DETECTION

// Reports in console when this object has collided with something, giving the names of
// both objects. There has a Rigidbody on the object with this code.
void OnCollisionEnter(Collision col) {
  Debug.Log(string.Format("{0} collided with {1}", transform.name, col.gameObject.name));
}

// 9. TAG-DEPENDENT TRIGGER DETECTION

// On entering a trigger collider, checks to see if it has the tag we are looking for.
void OnTriggerEnter(Collider other) {
  if (other.gameObject.CompareTag("TAG")) {
    Debug.Log(string.Format("{0} collided with {1}", transform.name, other.gameObject.name);
  }
}

// 10. GROUND CHECK

// Casts a ray down from object's posiition to just beyond the object's y bounds, returns
// true if hits anything. COLLIDER must be replaced with reference to object's collider.
public bool isGrounded {
  get {
      float dst = COLLIDER.bounds.extents.y + 0.1f;
      return Physics.Raycast(transform.position, -Vector3.up, dst);
  }
}

// 11. TIMED DESPAWN

// When called, waits for the specified time before destroying the GameObject it is attached to.
// Call using "StartCoroutine(Despawn())".
IEnumerator Despawn() {
  yield return new WaitForSeconds(5f); // Change value to suit.
  Destroy(gameObject);
}

// 12. SINGLETON PATTERN

// Creates a public static instance of a Monobehaviour class that can be accessed from any
// script using CLASSNAME.Instance. CLASSNAME must be replaced with the actual class name.
private static CLASSNAME _instance;
public static CLASSNAME Instance { get { return _instance; } }

private void Awake() {

  if (_instance != null && _instance != this)
    Destroy(this.gameObject);
  else
    _instance = this;

}

// 13. SCRIPTABLE OBJECT ASSET MENU ATTRIBUTE

// Place above ScriptableObject class. Creates menu item in editor's asset menu.
[CreateAssetMenu(fileName = "fileName", menuName = "Project/Menu Item Name")]

// 14. INSPECTOR ATTRIBUTES

[Header("Section Header")] // Adds header in inspector.
[Space] // Creates a space in the inspector.
[TextArea(15,20)] // Creates a text area of the defined height and width.
[Range(0,100)] // Creates a slider for selecting between the defined values.
[Tooltip("This is a tooltip.")] // Adds tootip text shown when mouse hovers over field in inspector.
[HideInInspector] // Hide public variables from inspector view.
[SerializeField] // Show non-public variables in the inspector.

// 15. NO DUPLICATES LIST

// Function to add to a list only if the current item is not already in the list. TYPE
// must be swapped for an valid class, struct, or variable.
List<TYPE> listName = new List<TYPE>();
void NoDuplicateAdd (TYPE item) {
  foreach (TYPE type in listName) // Loop through each item in the list.
    if (item == type) return; // If any match the item we're adding, return without adding.
  listName.Add(item); // If we get here, add the item to the list.
}

// 16. RANDOM BOOL

// Readonly bool that returns a random result each time.
bool rndBool {
  get { return (Random.value > 0.5f); }
}

// 17. MOUSE POSITION IN 3D SPACE

// Casts a ray from the mouse's position straight forward relative to the camera and reports
// back if it hits something.
RaycastHit hit = new RaycastHit();
if (Physics.Raycast(CAMERA.ScreenPointToRay(Input.mousePosition), out hit)) {
  Debug.Log(string.Format("Mouse is over {0}", hit.transform.name));
}

// 18. CAMERA SHAKE

// Function for producing camera shake. Must be placed on the same transform of the camera
// (or a parent transform of the camera). Call by using "StartCoroutine(Shake(duration))" where needed.
IEnumerator Shake (float duration) {
  float shakeAmount = 0.2f; // Change to suite.
  Vector3 originalPos = transform.position;
  while (duration > 0) {
    transform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
    duration -= Time.deltaTime;
    yield return null;
  }
  transform.position = originalPos;
}

// 19. LOOP THROUGH CHILD OBJECTS

// Function loops through all the children of the given Transform. Can be used to find particular
// children or all children of a certain type, etc.
void LoopThroughChildren (Transform parent) {
  foreach (Transform child in parent) {
    Debug.Log(child.name);
  }
}

// 20. BACKGROUND MUSIC

// Adds an AudioSource component to the attached object and loads a clip from the resources folder
// to play. Clip can be set directly with an AudioClip reference.
AudioSource bgMusic = gameObject.AddComponent<AudioSource>();
bgMusic.clip = Resources.Load("PATHTOMUSIC") as AudioClip;
bgMusic.loop = true;
bgMusic.Play();

// 21.
