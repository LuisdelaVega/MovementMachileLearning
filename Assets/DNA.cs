using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Standard class (Helper class) not a Monodevelop/Monobehavior
public class DNA
{
  List<int> genes = new List<int>();
  int dnaLength = 0;
  int maxValues = 0;
  public DNA(int length, int values)
  {
    dnaLength = length;
    maxValues = values;
    SetRandom();
  }

  public void SetRandom()
  {
    genes.Clear();

    for (int i = 0; i < dnaLength; i++)
    {
      genes.Add(Random.Range(0, maxValues));
    }
  }

  public void SetInt(int pos, int value)
  {
    genes[pos] = value;
  }

  public void Combine(DNA d1, DNA d2)
  {
    for (int i = 0; i < dnaLength; i++)
    {
      if (i < dnaLength / 2.0)
      {
        genes[i] = d1.genes[i];
      }
      else
      {
        genes[i] = d2.genes[i];
      }
    }
  }

  public void Mutate()
  {
    genes[Random.Range(0, dnaLength)] = Random.Range(0, maxValues);
  }

  public int GetGene(int pos)
  {
    return genes[pos];
  }
}
