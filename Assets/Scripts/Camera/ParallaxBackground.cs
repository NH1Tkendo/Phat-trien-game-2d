using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
	Transform cam;
	Vector3 camStartPos;
	float distance;

	GameObject[] background;
	Material[] mat;
	float[] backSpeed;
	float farthestBack;

	[Range(0.01f, 0.05f)]
	public float parallaxSpeed;
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		cam = Camera.main.transform;
		camStartPos = cam.position;

		int backCount = transform.childCount;
		mat = new Material[backCount];
		backSpeed = new float[backCount];
		background = new GameObject[backCount];

		for (int i = 0; i < backCount; i++)
		{
			background[i] = transform.GetChild(i).gameObject;
			mat[i] = background[i].GetComponent<Renderer>().sharedMaterial;
		}
		BackSpeedCaculate(backCount);
	}
	void BackSpeedCaculate(int backCount)
	{
		for (int i = 0; i < backCount; i++)
		{
			if ((background[i].transform.position.z - cam.position.z) > farthestBack)
			{
				farthestBack = background[i].transform.position.z - cam.position.z;
			}
		}

		for (int i = 0; i < backCount; i++)
		{
			backSpeed[i] = 1 - (background[i].transform.position.z - cam.position.z) / farthestBack;
		}
	}
	private void LateUpdate()
	{
		distance = cam.position.x - camStartPos.x;
		transform.position = new Vector3(cam.position.x, transform.position.y, 0);
		for (int i = 0; i < background.Length; i++)
		{
			float speed = backSpeed[i] * parallaxSpeed;
			mat[i].SetTextureOffset("_MainTex", new Vector2(distance, 0) * speed);
		}

	}
	//public Transform target;
	//public Transform farBackground, middleBackground;

	//private float lastXPosition;

	//// Start is called before the first frame update
	//void Start()
	//{
	//	lastXPosition = transform.position.x;
	//}

	//// Update is called once per frame
	//void Update()
	//{
	//	transform.position = new Vector3(target.position.x, transform.position.y, transform.position.z);

	//	float amountToMoveX = transform.position.x - lastXPosition;

	//	farBackground.position += new Vector3(amountToMoveX, 0f, 0f);
	//	middleBackground.position += new Vector3(amountToMoveX * .5f, 0f, 0f);

	//	lastXPosition = transform.position.x;
	//}
}
