using System;
using UnityEngine;


/// <summary>
/// Camera controls (around axis & zoom)
/// </summary>
public class CameraCtrlr : MonoBehaviour
{

    private float m_rotSpeed;

    private float m_fov;

    private void Start()
    {
        m_rotSpeed = 100;

    }


    private void Update()
    {
        //transform.LookAt(Vector3.zero);
        ////transform.Translate(Vector3.right * Time.deltaTime);
        //transform.Translate(new Vector3(Input.GetAxis("Horizontal"),0,0) * Time.deltaTime * m_rotSpeed);
        if(Input.anyKey)
        {
            Vector3 rot = new Vector3(0, Input.GetAxis("Horizontal") * Time.deltaTime * m_rotSpeed);

            transform.Rotate(-rot, Space.Self);

            //Vector3 move = new Vector3(0,0,Input.GetAxis("Vertical") * Time.deltaTime * m_rotSpeed);

            transform.position += transform.forward * Time.deltaTime * Input.GetAxis("Vertical") * 20;

            //transform.Translate(transform.forward * Time.deltaTime * Input.GetAxis("Vertical") * 40);


        }


        // fov zoom
        m_fov = Camera.main.fieldOfView;
        m_fov -= Input.GetAxis("Mouse ScrollWheel") * 15;

        //max zoom in/ out
        if ((m_fov < 80.1 && m_fov > 49.9))
            Camera.main.fieldOfView = m_fov;

    }


}

//Vector3 direction = hit.point - transform.position;
//direction = Vector3.ProjectOnPlane(direction, Vector3.up).normalized;
//            gameObject.transform.LookAt(transform.position + direction);