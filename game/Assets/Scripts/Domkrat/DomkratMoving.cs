using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Модуль отвечает за перемещение домкрата.
/// В домкрате есть:
///     1. Объекты всех трех колес (leftwheelT, rightWheelT, smallT)
///     2. Wheel колайдеры для всех трех колес (leftwheelW, rightWheelW, smallW)
///     3. Обычные циллендрические колайдеры, которые предотвращают соприкосновение
///        Wheel колайдеров с землей (иначе физика начинает творить хуйню)
/// У Wheel колайдеров есть атрибут, который мы будем варьировать - motorTorque, это
/// сила "мотора" колеса, от её величины зависит с какой скоростью будет крутиться колесо.
/// 
/// При инициализации устанавливаем эту силу в 0, при движении увеличиваем - колеса начинают вращаться.
/// </summary>

public class DomkratMoving : MovingSelect
{
    public Transform leftwheelT, rightWheelT, smallT;
    public WheelCollider leftwheelW, rightWheelW, smallW;
    public GameObject domrkatObj;

    void Start() {
        // motorTorque - сила мотора колеса, инициализируем в нули, чтобы в начале сцены колеса домкратов
        // не вращались на месте (во время движения нужно увеличить силу до нужной)
        leftwheelW.motorTorque = 0;
        rightWheelW.motorTorque = 0;
        smallW.motorTorque = 0;
        UpdateWheelPose(smallW, smallT);
        UpdateWheelPose(leftwheelW, leftwheelT);
        UpdateWheelPose(rightWheelW, rightWheelT);
    } 

/// <summary>
/// Функция вызывается каждый кадр, когда игрок управляет домкратом, отвечает за перемещение и вращение всех колес домкрата.
/// 
/// Домкрат здесь является ведомым объектом, т.е. он следует за позицией игрока.
/// Каждый кадр домкрат телепортируется в точку перед игроком.
/// Чтобы создать эфект вращения колес:
///     1. Считываем нажатые игроком кнопки.
///     2. На кнопки вперед-назад придаем колесу силу вращения (motorTorque) в нужную сторону
/// 
/// В функции есть захардкоженная переменная motorForce, увеличивая/уменьшая её значение
/// можно увеличить/уменьшить скорость вращения колес.
/// </summary>
    public override void Moving()
    {
        Debug.Log("Moving domkrat");
        // Телепортируем домкрат в точку-поинтер перед игроком
        transform.rotation = Pointer.transform.rotation;
        transform.position = Pointer.transform.position;

        // Добавляем силы моторам Wheel колайдеров чтобы создать эффект вращения
        // TODO: управлять скоростью более умно
        int motorForce = 10;

        // Считываем нажатые кнопки (.x == влево-вправо; .y == ничего; .z == вперед-назад)
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        // Обновляем параметры Wheel колайдеров
        // input.z > 0 - движение вперед
        // input.z < 0 - движение назад
        leftwheelW.motorTorque = input.z * motorForce;
        rightWheelW.motorTorque = input.z * motorForce;
        smallW.motorTorque = input.z * motorForce;

        // Синхроним положение колёс с положением колайдеров
        UpdateWheelPose(leftwheelW, leftwheelT);
        UpdateWheelPose(rightWheelW, rightWheelT);
        UpdateWheelPose(smallW, smallT);
    }

/// <summary>
/// Синхронизирует положение колеса (Transform-like object) с фактическим положением WheelCollider'а
/// </summary>
/// <param name="_collider"></param>
/// <param name="_transform"></param>
    private void UpdateWheelPose(WheelCollider _collider, Transform _transform)
	{
        Debug.Log("upd domkrat");
		Vector3 _pos = _transform.position;
		Quaternion _quat = _transform.rotation;

		_collider.GetWorldPose(out _pos, out _quat);
		_transform.position = _pos;
		_transform.rotation = _quat;
	}
}
