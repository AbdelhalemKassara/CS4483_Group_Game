// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class TerrainChecker : MonoBehaviour
// {
//     private enum TerrainTags
//     {
//         Dirt,
//         Rock
//     }

//     [SerializeField] 
//     private AudioClip[] footstepAudios;
//     private AudioSource audioSource;

//     private float footstepTimer;
//     private float timePerStep = 0.5f;

//     private void Start()
//     {
//         audioSource = GetComponent<AudioSource>();
//     }

//     void Update()
//     {
//         footstepTimer += Time.deltaTime;
//         if (Movement.isMoving && audioSource.clip && footstepTimer > timePerStep)
//         {
//             audioSource.Play();
//             footstepTimer = 0;
//         }

//     }

//     private void OnCollisionEnter(Collision col)
//     {
//         int index = 0;
//         foreach (string tag in Enum.GetNames(typeof(TerrainTags)))
//         {
//             if (col.gameObject.tag == tag)
//             {
//                 audioSource.clip = footstepAudios[index];
                
//             }

//             index++;
//         }
//     }
// }
