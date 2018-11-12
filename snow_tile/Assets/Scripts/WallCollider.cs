using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCollider : MonoBehaviour
{

	public AudioSource rock;

	void OnControllerColliderHit(ControllerColliderHit hit)
	{
		if (!rock.isPlaying && hit.collider.tag == "wall")
		{
			rock.volume = Random.Range(0.4f, 0.6f);
			rock.pitch = Random.Range(0.8f, 1.1f);
			rock.Play();
		}
	}
}
