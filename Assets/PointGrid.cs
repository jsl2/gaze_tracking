using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PointGrid : MonoBehaviour
{
    public GameObject point;
    public int x_size;
    public int y_size;
    public int z_size;
    public bool world_xy;    

    private List<GameObject> points;
    private int pointIndex;
    // Start is called before the first frame update
    void Start()
    {
        Camera camera = GetComponent<Camera>();
        points = new List<GameObject>();
        pointIndex = 0;        
        Vector3 front_top_right = camera.ViewportToWorldPoint(new Vector3(0.8f, 0.9f, 4.0f));
        Vector3 front_bottom_left = camera.ViewportToWorldPoint(new Vector3(0.2f, 0.1f, 4.0f));
        if (world_xy)
        {
            var diff = front_top_right - front_bottom_left;
            var dx = diff.x / (x_size - 1);
            var dy = diff.y / (y_size - 1);
            var dz = dy;
            for (int x = 0; x < x_size; x++)
            {
                for (int y = 0; y < y_size; y++)
                {
                    for (int z = 0; z < z_size; z++)
                    {
                        var pos = front_bottom_left + new Vector3(x * dx, y * dy, z * dz);                        
                        var new_point = Instantiate(point, pos, new Quaternion());                        
                        new_point.transform.parent = this.transform;
                        points.Add(new_point);
                    }
                }
            }
        }
        else
        {
            var front_z = 4.0f;
            var back_z = 10.0f;
            var dz = (back_z - front_z) / (z_size - 1);
            for (int z = 0; z < z_size; z++)
            {
                Vector3 top_right = camera.ViewportToWorldPoint(new Vector3(0.8f, 0.9f, front_z + dz * z));
                Vector3 bottom_left = camera.ViewportToWorldPoint(new Vector3(0.2f, 0.1f, front_z + dz * z));
                var diff = top_right - bottom_left;
                var dx = diff.x / (x_size - 1);
                var dy = diff.y / (y_size - 1);                
                for (int x = 0; x < x_size; x++)
                {
                    for (int y = 0; y < y_size; y++)
                    {
                        var pos = bottom_left + new Vector3(x * dx, y * dy, 0);                        
                        var new_point = Instantiate(point, pos, new Quaternion());                        
                        new_point.transform.parent = this.transform;
                        points.Add(new_point);
                    }
                }
            }
        }
        points[0].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            points[pointIndex].SetActive(false);
            pointIndex = (pointIndex + 1) % points.Count;
            points[pointIndex].SetActive(true);            
        }
    }
}
