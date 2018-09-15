using UnityEngine;

/// <summary>
/// 主相机脚本 —— 挂载到主相机上，并 层次列表中的 Plane 拖到 pivot 
/// </summary>
public class ChinarCamera : MonoBehaviour
{
    public Transform target;
    public float distance = 10.0f;
    public float minDistance = 2f;          //距离物体最近距离
    public float maxDistance = 15f;         //最远距离
    public float zoomSpeed = 1f;
    public float xSpeed = 250.0f;
    public float ySpeed = 120.0f;

    //private Parameters
    private float x = 0.0f;
    private float y = 0.0f;
    private float targetX = 0f;
    private float targetY = 0f;
    private float targetDistance = 0f;
    //以下参数用来接收SmoothDampAngle 和 SmoothDamp 的velocity数据。初始化的数据并无任何意义
    private float xVelocity = 1f;
    private float yVelocity = 1f;
    private float zoomVelocity = 1f;


    private void Start()
    {
        var angles = transform.eulerAngles;
        targetX = x = angles.x;
        targetY = y = angles.y;
        targetDistance = distance;
    }

    private void LateUpdate()
    {
        //当没有设置目标时，返回
        if (!target) return;
        
        //上下左右的旋转
        if (Input.GetMouseButton(1) || (Input.GetMouseButton(0) && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))))
        {
            targetX += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
            //当只需要左右旋转时可将下面两句话注释掉
            targetY -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
            targetY = Mathf.Clamp(targetY,-90,90);                  //对可旋转的角度进行一定的限制
        }
        Debug.Log("X:" + x + "  Y:  " + y);
        x = Mathf.SmoothDampAngle(x, targetX, ref xVelocity, 0.3f);
        y = Mathf.SmoothDampAngle(y, targetY, ref yVelocity, 0.3f);
        Quaternion rotation = Quaternion.Euler(y, x, 0);
        transform.rotation = rotation;


        //左右移动及远近拉伸脚本，整块注释后将只能在原地上下左右旋转，启用时可实现围绕某一特定物体旋转
        var scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0.0f)
            targetDistance -= zoomSpeed;
        else if (scroll < 0.0f)
            targetDistance += zoomSpeed;
        targetDistance = Mathf.Clamp(targetDistance, minDistance, maxDistance);
        distance = Mathf.SmoothDamp(distance, targetDistance, ref zoomVelocity, 0.5f);
        Vector3 position = rotation * new Vector3(0.0f, 0.0f, -distance) + target.position;
        transform.position = position;
    }
}