using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour { 
	public GameObject laserPrefab;
	public float speed = 20f;
	public float padding = 0.5f;
	public float laserSpeed;
	public float fireRate = 0.2f;
	public float health = 250f;
	public AudioClip fireSound;
	
	private LevelManager levelManager;
	float xmin, xmax;
	
	void Start () {
		levelManager = GameObject.FindObjectOfType<LevelManager>();
		float distance = transform.position.z - Camera.main.transform.position.z;
		Vector3 leftmost = Camera.main.ViewportToWorldPoint(new Vector3(0,0,distance));
		Vector3 rightmost = Camera.main.ViewportToWorldPoint(new Vector3(1,0,distance));
		xmin = leftmost.x + padding;
		xmax = rightmost.x - padding;
	}
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space)){
			InvokeRepeating("Fire", 0.0000000001f, fireRate);
		}
		if (Input.GetKeyUp(KeyCode.Space)){
			CancelInvoke("Fire");
		}
		if (Input.GetKey(KeyCode.LeftArrow)) {
			transform.position += Vector3.left * (speed * Time.deltaTime);
		} else if (Input.GetKey(KeyCode.RightArrow)) {
			transform.position += Vector3.right * (speed * Time.deltaTime);
		}
		float newX = Mathf.Clamp(transform.position.x, xmin, xmax);
		transform.position = new Vector3(newX, transform.position.y, transform.position.z);
		
	}
	
	void OnTriggerEnter2D (Collider2D collider) {
		Projectile missile = collider.gameObject.GetComponent<Projectile>();
		if (missile){
			health -= missile.GetDamage();
			missile.Hit();
			if (health <= 0) {
				Die ();
			}
		}
	}
	
	void Fire() {
		Vector3 startPostition = transform.position + new Vector3(0f, 1f, 0f);
		GameObject laser = Instantiate(laserPrefab, startPostition, Quaternion.identity) as GameObject;
		laser.rigidbody2D.velocity = new Vector3(0, laserSpeed);
		AudioSource.PlayClipAtPoint(fireSound, transform.position);
	}
	
	void Die() {
		Destroy(gameObject);
		levelManager.LoadLevel("Win Screen");
		
	}
}
