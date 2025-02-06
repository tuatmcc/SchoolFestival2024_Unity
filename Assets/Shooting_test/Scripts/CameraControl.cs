using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Shooting_test
{
    public class CameraControl : MonoBehaviour
    {
        Transform transform_parent;

        //マウス感度
        float sensitivity = 2f;
        Vector2 RStick_input = new Vector2(0,0);

        private void Start()
        {
            transform_parent = this.transform;
        }

        void Update()
        {
            //CameraRotate_Mouse();
            transform_parent.RotateAround(transform.position, Vector3.up, RStick_input.x*sensitivity);
        }

        private void CameraRotate_Mouse()
        {
            //マウスの移動量取得
            float x = Input.GetAxis("Mouse X") * sensitivity;
            float y = Input.GetAxis("Mouse Y") * sensitivity;

            //Y軸中心にカメラを回転
            transform_parent.RotateAround(transform.position, Vector3.up, x);

            //上を見上げすぎて逆に後ろを向いちゃうのを防止．のつもりだったけど挙動変なので別途付け加えてください．
            if (transform.forward.y + y > 0.9f)
            {
                y = 0.9f - transform.forward.y;
            }

            //カメラのX軸を中心に回転
            //transform.RotateAround(transform.position, transform.right, -y);
        }

        public void CameraRotate_Gamepad(InputAction.CallbackContext context)
        {
            RStick_input = context.ReadValue<Vector2>();
            
        }
    }
}
