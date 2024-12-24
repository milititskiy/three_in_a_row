// using UnityEngine;

// public class HighlightPlayer : MonoBehaviour
// {
//     public string playerTag = "Player"; // Tag of the GameObject you want to highlight
//     public Color highlightColor = Color.red; // Color to highlight with

//     private GameObject playerObject;
//     private Material originalMaterial;
//     private Material highlightMaterial;

//     void Start()
//     {
//         // Find the GameObject with the specified tag
//         playerObject = GameObject.FindGameObjectWithTag(playerTag);

//         if (playerObject != null)
//         {
//             // Get the original material of the GameObject
//             originalMaterial = playerObject.GetComponent<Renderer>().material;

//             // Create a new material with the highlight color
//             highlightMaterial = new Material(originalMaterial);
//             highlightMaterial.color = highlightColor;
//         }
//         else
//         {
//             Debug.LogError("No GameObject with tag '" + playerTag + "' found.");
//         }
//     }

//     void Update()
//     {
//         if (playerObject != null)
//         {
//             // Highlight the player object by assigning the highlight material
//             playerObject.GetComponent<Renderer>().material = highlightMaterial;
//         }
//     }

//     void OnDestroy()
//     {
//         // Restore the original material when the script is destroyed
//         if (playerObject != null)
//         {
//             playerObject.GetComponent<Renderer>().material = originalMaterial;
//         }
//     }
// }
