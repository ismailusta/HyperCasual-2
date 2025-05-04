using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEditor;

public class HoleMovement : MonoBehaviour
{
    [Header("Hole Mesh")]
    [SerializeField] MeshFilter meshFlter;
    [SerializeField] MeshCollider meshCollider;
    [Header("Hole Settings")]
    [SerializeField] float radius;
    [SerializeField] Transform holeCenterTransform;
    [Header("Space")]
    [SerializeField] float moveSpeed;
    Mesh mesh;
    List<int> holeVertices;
    List<Vector3> offsets;
    int holeVerticesCount;

    float x, y;
    Vector3 touch, targetPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Game.isGameOver = false;
        Game.isMoving = false;
        holeVertices = new List<int>();
        offsets = new List<Vector3>();

        mesh = meshFlter.mesh;

        FindHoleVertices();
    }


    // Update is called once per frame
    void Update()
    {
        Game.isMoving = Input.GetMouseButton(0);
        if (!Game.isGameOver && Game.isMoving)
        {
            // Move hole the center
            MoveHole();
            // Update hole vertices positions
            UpdateHoleVerticesPositions();
        }
    }

    private void MoveHole()
    {
        x = Input.GetAxis("Mouse X");
        y = Input.GetAxis("Mouse Y");

        touch = Vector3.Lerp(holeCenterTransform.position, holeCenterTransform.position + new Vector3(x, 0f, y), moveSpeed * Time.deltaTime);
        holeCenterTransform.position = touch;
    }
    private void UpdateHoleVerticesPositions()
    {
        Vector3[] vertices = mesh.vertices;
        for (int i = 0; i < holeVertices.Count; i++)
        {
            vertices[holeVertices[i]] = holeCenterTransform.position + offsets[i];
        }
        mesh.vertices = vertices;
        meshFlter.mesh = mesh;
        meshCollider.sharedMesh = mesh;

    }
    private void FindHoleVertices()
    {
        for (int i = 0; i < mesh.vertices.Length; i++)
        {
            float distance = Vector3.Distance(holeCenterTransform.position, mesh.vertices[i]);
            if (distance < radius)
            {
                holeVertices.Add(i);
                offsets.Add(mesh.vertices[i] - holeCenterTransform.position);
            }
        }
        holeVerticesCount = holeVertices.Count;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(holeCenterTransform.position, radius);
    }
}
