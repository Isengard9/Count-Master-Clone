using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Movement script using for player movement
/// </summary>
public class Movement : MonoBehaviour
{
    [Header("L/R Position Values")]
    float distanceFixer;//Fixes distance around the player touch position
    Vector3 offsetVector, temp2;
    [Tooltip("Clamp Player Position")]
    [SerializeField] Vector2 ClampPosition = Vector2.zero;//Clamping player position
    GameObject offsetObject;//Calculating touch position using this gameobject


    [Header("Forward Movement")]

    /// <summary>
    /// Forward Movement Speed
    /// </summary>
    [Range(0, 20)]
    [SerializeField] float MoveSpeed = 10;//Forward Movement speed
    // Start is called before the first frame update
    void Start()
    {
        offsetObject = new GameObject();
        offsetObject.name = "Offset Object";
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.isGameStarted || GameManager.isGameEnded)
            return;
        MoveLeftRight();
    }
    private void FixedUpdate()
    {
        if (!GameManager.isGameStarted || GameManager.isGameEnded)
            return;
        if (PlayerManager.instance.isEnemyTime)
            return;
        if (Input.GetMouseButton(0))
            MoveForward();
    }
    #region Left Right Movement


    /// <summary>
    /// Moving player left or right using offset gameobject
    /// Why we are use offset object because real player touch any point of screen we must move the player using this offset
    /// if the player move finger too much left or right calculating distance fixer and get under control to lag of moving
    /// </summary>
    void MoveLeftRight()
    {
        if (PlayerManager.instance.isEnemyTime)
            return;


        if (Input.GetMouseButtonDown(0))
        {
            offsetObject.transform.position = new Vector3(CalculateXPos(), 2, this.transform.position.z);
            distanceFixer = Vector3.Distance(offsetObject.transform.position, this.transform.position);
            offsetVector = offsetObject.transform.position - this.transform.position;
        }
        if (Input.GetMouseButton(0))
        {
            offsetObject.transform.position = new Vector3(CalculateXPos(), 2, this.transform.position.z);
            temp2 = offsetObject.transform.position - offsetVector;
            temp2.x = Mathf.Clamp(temp2.x, ClampPosition.x, ClampPosition.y);

            this.transform.position = temp2;

            if (distanceFixer > Vector3.Distance(this.transform.position, offsetObject.transform.position))
            {
                distanceFixer = Vector3.Distance(this.transform.position, offsetObject.transform.position);
                offsetVector = offsetObject.transform.position - this.transform.position;
            }
        }
        if (Input.GetMouseButtonUp(0))
        {

        }
    }
    /// <summary>
    /// Calculating X position of offset object
    /// Example we are touching 540 of x dimension
    /// and we have 720*1280
    /// clamping on -5, 5 on the x 
    /// the result is 2.5
    /// the the offset object x position will be 2.5
    /// </summary>
    /// <returns></returns>
    public float CalculateXPos()
    {
        float xPos = 0;

        return ((Input.mousePosition.x * ClampPosition.y * 2) / Screen.width) + ClampPosition.x;
    }
    #endregion

    #region HorizontalMove

    /// <summary>
    /// Move forward
    /// </summary>
    void MoveForward()
    {
       this.transform.Translate(this.transform.forward * MoveSpeed * Time.deltaTime,Space.World);
    }

    #endregion
}
