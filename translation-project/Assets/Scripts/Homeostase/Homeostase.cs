using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Homeostase : MonoBehaviour {

    public Vector3 initialPosition;
	// Use this for initialization
	void Start () {
        initialPosition = transform.position;
	}

    public void SwipePositive()
    {
        transform.DOMoveX(400, 1);
        transform.DOMoveY(-300, 2);
        transform.DORotate(new Vector3(0, 0, -45), 2);
    }

    public void SwipeNegative()
    {
        transform.DOMoveX(-400, 1);
        transform.DOMoveY(-300, 2);
        transform.DORotate(new Vector3(0, 0, 45), 2);
    }

    public void SwipePositiveScaled()
    {
        transform.DOMoveY(25, 1);
        transform.DORotate(new Vector3(0, 0, -45), 2);
        transform.DOScale(new Vector3(0.4f, 0.4f), 2);
    }

    public void ResetPosition()
    {
        transform.position = initialPosition;
        transform.rotation = Quaternion.identity;
        transform.DOScale(Vector3.one, 0);
    }
}
