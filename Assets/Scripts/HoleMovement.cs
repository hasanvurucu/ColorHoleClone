using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleMovement : MonoBehaviour
{

    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private MeshCollider meshCollider;

	[SerializeField] private float radius;
	[SerializeField] Transform holeCenter;
	[SerializeField] Vector2 moveLimits;

	[SerializeField] private float moveSpeed;

	Mesh mesh;
	List<int> holeVertices;
	List<Vector3> offsets;
	int holeVerticesCount;

	private bool isLerpingPos;
	[SerializeField] private Transform fakeEnvironment;

	Vector2 mouseInitials;
	Vector2 mouseCurrent;
	Vector3 holePos;
	Vector3 holeInitialPos;


	private void Start()
    {
		holeCenter = this.transform;

		Game.isMoving = false;
		Game.isGameOver = false;
		isLerpingPos = false;

        holeVertices = new List<int>();
        offsets = new List<Vector3>();

        mesh = meshFilter.mesh;

        FindHoleVertices();

		transform.GetComponent<Collider>().enabled = false;
    }

    private void Update()
    {
		Game.isMoving = Input.GetMouseButton(0);

		if (Input.GetKeyUp(KeyCode.Space))
        {
			MoveToSecondPart();
        }

		if(!Game.isGameOver && Game.isMoving)
        {
			if(!isLerpingPos)
				MoveHole();

			UpdateHoleVerticesPosition();
		}

		
	}

	private void MoveHole()
    {
		if (Input.GetMouseButtonDown(0)) //Get initial values
		{
			mouseInitials.x = (Input.mousePosition.x / Screen.width);
			mouseInitials.y = (Input.mousePosition.y / Screen.height);

			holeInitialPos = holeCenter.position;
			holePos = holeCenter.position;
		}

		if (Input.GetMouseButton(0)) //Compare current values to the initial values and determine the position
		{
			mouseCurrent.x = (Input.mousePosition.x / Screen.width);
			mouseCurrent.y = (Input.mousePosition.y / Screen.height);

			holePos = holeCenter.position;

			holePos.x = holeInitialPos.x + (mouseCurrent.x - mouseInitials.x) * moveSpeed;
			holePos.z = holeInitialPos.z + (mouseCurrent.y - mouseInitials.y) * moveSpeed;

			holePos.x = Mathf.Clamp(holePos.x, -0.75f, 0.75f);
			holePos.z = Mathf.Clamp(holePos.z, -1.25f, 1.25f);

			holeCenter.position = holePos;

		}
    }

	private void UpdateHoleVerticesPosition()
    {
		Vector3[] vertices = mesh.vertices;

		for (int i = 0; i < holeVerticesCount; i++)
        {
			vertices[holeVertices[i]] = holeCenter.position + offsets[i];
        }

		mesh.vertices = vertices;
		meshFilter.mesh = mesh;
		meshCollider.sharedMesh = mesh;

    }


	private void FindHoleVertices()
    {
		for (int i = 0; i < mesh.vertices.Length; i++)
		{
			float distance = Vector3.Distance(holeCenter.position, mesh.vertices[i]);

			if (distance < radius)
			{
				holeVertices.Add(i);
				offsets.Add(mesh.vertices[i] - holeCenter.position);
			}
		}

		holeVerticesCount = holeVertices.Count;
	}

    private void OnDrawGizmos()
    {
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(holeCenter.position, radius);
    }

	public void MoveToSecondPart()
    {
		isLerpingPos = true;
		StartCoroutine(LerpPosition());
    }

	IEnumerator LerpPosition()
    {
		float t = 0;
		Vector3 pos = holeCenter.localPosition;
		Vector3 initialPos= pos;

		GameManager.Instance.DisableFirstPartObstacleGravity();

		while (t < 1)
        {
			t += Time.deltaTime / 1.5f;

			//Lerp to middle
			pos = holeCenter.localPosition;
			pos = Vector3.Lerp(initialPos, new Vector3(0, pos.y, -1.038f), t);
			holeCenter.localPosition = pos;

			UpdateHoleVerticesPosition();

			yield return new WaitForEndOfFrame();
        }

		t = 0;
		Vector3 fakeEnvironmentPos;
		Vector3 fakeEnvInitial = fakeEnvironment.transform.position;

		transform.GetComponent<Collider>().enabled = true;

		while (t < 1)
        {
			t += Time.deltaTime / 1.5f;

			//Move to second part (Whole object) target Z = 8f;
			fakeEnvironmentPos = fakeEnvironment.transform.position;
			fakeEnvironmentPos.z = Mathf.Lerp(fakeEnvInitial.z, -8f, t);
			fakeEnvironment.transform.position = fakeEnvironmentPos;

			yield return new WaitForEndOfFrame();
        }

		//Release movement input
		isLerpingPos = false;

		GameManager.Instance.EnableSecondPartGravity();

		transform.GetComponent<Collider>().enabled = true;
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == Tags.middleObstacleTag)
        {
			other.GetComponent<Rigidbody>().useGravity = true;
        }
    }
}
