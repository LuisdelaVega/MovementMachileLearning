using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof(ThirdPersonCharacter))]
public class Brain : MonoBehaviour
{
  public int DNALength = 1;
  public float timeAlive;
  public DNA dna;
  private ThirdPersonCharacter m_Character;
  private Vector3 m_Move;
  private bool m_Jump;
  bool alive = true;
  Vector3 startPosition;
  public float distanceTravelled;

  /// <summary>
  /// OnCollisionEnter is called when this collider/rigidbody has begun
  /// touching another rigidbody/collider.
  /// </summary>
  /// <param name="obj">The Collision data associated with this collision.</param>
  void OnCollisionEnter(Collision obj)
  {
    if (obj.gameObject.tag == "dead")
    {
      alive = false;
    }
  }

  public void Init()
  {
    // Initialise DNA
    // 0 forward
    // 1 back
    // 2 left
    // 3 right
    // 4 jump
    // 5 crouch
    dna = new DNA(DNALength, 6);
    m_Character = GetComponent<ThirdPersonCharacter>();
    timeAlive = 0;
    distanceTravelled = 0;
    alive = true;
    startPosition = this.transform.position;
  }

  /// <summary>
  /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
  /// Fixed update is called in sync with physics
  /// </summary>
  void FixedUpdate()
  {
    float horizontalDirection = 0;
    float verticalDirection = 0;
    bool crouch = false;

    switch (dna.GetGene(0))
    {
      case 1:
        verticalDirection = -1;
        break;
      case 2:
        horizontalDirection = -1;
        break;
      case 3:
        horizontalDirection = 1;
        break;
      case 4:
        m_Jump = true;
        break;
      case 5:
        crouch = true;
        break;
      default:
        verticalDirection = 1;
        break;
    }

    m_Move = verticalDirection * Vector3.forward + horizontalDirection * Vector3.right;
    m_Character.Move(m_Move, crouch, m_Jump);
    m_Jump = false; // We don't want to keep jumping. Needed for Ethan's code

    if (alive)
    {
      timeAlive += Time.deltaTime;
      distanceTravelled = Vector3.Distance(transform.position, startPosition);
    }
  }
}
