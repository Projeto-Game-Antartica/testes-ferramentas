using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharController : MonoBehaviour
{

    public float speed = 2f;

    public bool walking = false;

    private CharacterController controller;

	private Vector3 forward, right, up;

    // Use this for initialization
    void Start()
    {
		forward = Camera.main.transform.forward;
        forward.y = 0;
        forward = Vector3.Normalize(forward);
        right = Quaternion.Euler(new Vector3(0, 60, 0)) * forward;
		up = Quaternion.Euler(new Vector3(0, -60, 0)) * forward;
		controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKey(KeyCode.W) ||Input.GetKey(KeyCode.UpArrow) ||
		    Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            Move();
            walking = true;
			controller.GetComponent<SpriteRenderer>().flipX = true;
        }
		else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || 
		         Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) {
			Move();
            walking = true;
            controller.GetComponent<SpriteRenderer>().flipX = false;
		}
		else
            walking = false;

    }

    void Move()
    {
		Vector3 move = new Vector3(Input.GetAxis("HorizontalKey"), 0, Input.GetAxis("VerticalKey"));
		Vector3 rightMovement = right * speed * Time.deltaTime * Input.GetAxis("HorizontalKey");
		Vector3 upMovement = up * speed * Time.deltaTime * Input.GetAxis("VerticalKey");

		controller.SimpleMove(move);

		// rotaciona a animação
		// Vector3 heading = Vector3.Normalize(rightMovement + upMovement);
		// controller.transform.forward = heading;

		// movimentação de acordo com a camera isométrica
		controller.transform.position += rightMovement;
		controller.transform.position += upMovement;
    }
}
