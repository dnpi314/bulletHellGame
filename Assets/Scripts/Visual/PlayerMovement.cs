using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
  private Vector2 moveDirection;
  private Rigidbody2D rigidBody;
  private bool invulnerable;
  private Animator animator;

  public float speed;
  public float invulnerabilityTime;

  private void Start()
  {
    rigidBody = GetComponent<Rigidbody2D>();
    transform.position = PanelSize.centerPoint;
    animator = GetComponent<Animator>();
    invulnerable = false;
  }

  private void Update()
  {
    GetInput();
    rigidBody.velocity = speed * moveDirection;
    Vector3 pos = transform.position;
    if(pos.x >= PanelSize.playArea.xMax)
    {
      pos.x = PanelSize.playArea.xMax;
    }
    if(pos.x <= PanelSize.playArea.xMin)
    {
      pos.x = PanelSize.playArea.xMin;
    }
    if (pos.y >= PanelSize.playArea.yMax)
    {
      pos.y = PanelSize.playArea.yMax;
    }
    if (pos.y <= PanelSize.playArea.yMin)
    {
      pos.y = PanelSize.playArea.yMin;
    }
    transform.position = pos;

    if (invulnerable)
    {
      animator.SetFloat("InvulnerabilityTime", animator.GetFloat("InvulnerabilityTime") - Time.deltaTime);
      if(animator.GetFloat("InvulnerabilityTime") <= 0)
      {
        invulnerable = false;
      }
    }
  }

  private void GetInput()
  {
    Vector2 inputDirection = new Vector2();
    inputDirection.x = Input.GetAxis("Horizontal");
    inputDirection.y = Input.GetAxis("Vertical");
    if(inputDirection.magnitude >= 1)
    {
      inputDirection.Normalize();
    }
    moveDirection = inputDirection;
  }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.tag.Equals("Bullet") && !invulnerable)
    {
      GameController.TakeDamage();
      invulnerable = true;
      animator.SetBool("TookDamage", true);
      animator.SetFloat("InvulnerabilityTime", invulnerabilityTime);
    }
  }
}
