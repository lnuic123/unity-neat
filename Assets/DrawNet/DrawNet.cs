using UnityEngine;
using System.Collections;
using SharpNeat.Phenomes;
using SharpNeat.Network;
using System.Collections.Generic;
using SharpNeat.EvolutionAlgorithms;
using SharpNeat.Genomes.Neat;
using System;

public class DrawNet : MonoBehaviour
{
    public GameObject NetDrawCircle, NetDrawLine;
    public float PositionOffsetX = 0f;
    public float PositionOffsetY = 0f;
    public GameObject MainCamera;
    public void DrawNetwork (NeatEvolutionAlgorithm<NeatGenome> ea)
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        NeatGenome genome;
        try
        {
            genome = new NeatGenome(ea.CurrentChampGenome, 1, 1);
        }
        catch (Exception e1)
        {
            // print(champFileLoadPath + " Error loading genome from file!\nLoading aborted.\n"
            //						  + e1.Message + "\nJoe: " + champFileLoadPath);
            return;
        }
        float TotalOffsetX = PositionOffsetX + MainCamera.transform.position.x - 5f;
        float j = 0f;
        float k = 0f;
        float l = 0f;
        Dictionary<uint, Vector3> NeuronPositions = new Dictionary<uint, Vector3>();
        foreach (NeuronGene a in genome.NeuronGeneList)
        {
            if (a.NodeType == NodeType.Input || a.NodeType == NodeType.Bias)
            {
                GameObject obj = Instantiate(NetDrawCircle, new Vector3(TotalOffsetX - j, PositionOffsetY, 0f), Quaternion.identity, transform);
                NeuronPositions.Add(a.Id, new Vector3(0f - j, 0f, 0f));
                obj.GetComponent<SpriteRenderer>().color = a.NodeType == NodeType.Input ? Color.red : new Color(0f, 0f, 0.7f);
                j++;
            }
            else if (a.NodeType == NodeType.Hidden)
            {
                GameObject obj = Instantiate(NetDrawCircle, new Vector3(TotalOffsetX - k, PositionOffsetY + 2f, 0f), Quaternion.identity, transform);
                NeuronPositions.Add(a.Id, new Vector3(0f - k, 0f + 2f, 0f));
                obj.GetComponent<SpriteRenderer>().color = Color.yellow;
                k++;
            }
            else if (a.NodeType == NodeType.Output)
            {
                GameObject obj = Instantiate(NetDrawCircle, new Vector3(TotalOffsetX - l, PositionOffsetY + 4f, 0f), Quaternion.identity, transform);
                NeuronPositions.Add(a.Id, new Vector3(0f - l, 0f + 4f, 0f));
                obj.GetComponent<SpriteRenderer>().color = new Color(0f, 0.7f, 0f);
                l++;
            }
        }
        foreach (ConnectionGene ConGene in genome.ConnectionGeneList)
        {
            Vector3 SourceNeuronPosition = NeuronPositions[ConGene.SourceNodeId];
            Vector3 TargetNeuronPosition = NeuronPositions[ConGene.TargetNodeId];
            float x = (TargetNeuronPosition.x - SourceNeuronPosition.x) / 2 + SourceNeuronPosition.x;
            float y = (TargetNeuronPosition.y - SourceNeuronPosition.y) / 2 + SourceNeuronPosition.y;
            float a = TargetNeuronPosition.x - SourceNeuronPosition.x;
            float b = TargetNeuronPosition.y - SourceNeuronPosition.y;
            float c = Mathf.Sqrt(a * a + b * b);
            float ConnectionRotationZ = SourceNeuronPosition.x <= TargetNeuronPosition.x ? (Mathf.Asin(b / c) * 360) / (2 * Mathf.PI) : -(Mathf.Asin(b / c) * 360) / (2 * Mathf.PI);

            Quaternion ConGeneRotation = Quaternion.Euler(new Vector3(transform.rotation.x, transform.rotation.y, ConnectionRotationZ));

            GameObject obj = Instantiate(NetDrawLine, new Vector3(TotalOffsetX + x, PositionOffsetY + y, 0f), ConGeneRotation, transform);

            float ConWidth = Mathf.Abs((float)ConGene.Weight) / 10;
            if (ConWidth < 0.15f) ConWidth = 0.15f;
            else if (ConWidth > 0.6f) ConWidth = 0.6f;
            obj.transform.localScale = new Vector3(0.279f * c, ConWidth, obj.transform.localScale.z);
        }
    }
}
