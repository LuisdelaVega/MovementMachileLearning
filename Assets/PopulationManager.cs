using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PopulationManager : MonoBehaviour
{
  public GameObject botPrefab;
  public int populationSize = 50;
  List<GameObject> population = new List<GameObject>();
  public static float elapsed = 0;
  public float trialTime = 5;
  int generation = 1;

  GUIStyle guiStyle = new GUIStyle();
  /// <summary>
  /// OnGUI is called for rendering and handling GUI events.
  /// This function can be called multiple times per frame (one call per event).
  /// </summary>
  void OnGUI()
  {
    guiStyle.fontSize = 25;
    guiStyle.normal.textColor = Color.white;
    GUI.BeginGroup(new Rect(10, 10, 250, 150));
    GUI.Box(new Rect(0, 0, 140, 140), "Stats", guiStyle);
    GUI.Label(new Rect(10, 25, 200, 30), "Gen: " + generation, guiStyle);
    GUI.Label(new Rect(10, 50, 200, 30), string.Format("Time: {0:0.00}", elapsed), guiStyle);
    GUI.Label(new Rect(10, 75, 200, 30), "Population: " + population.Count, guiStyle);
    GUI.EndGroup();
  }

  // Use this for initialization
  void Start()
  {
    for (int i = 0; i < populationSize; i++)
    {
      Vector3 startingPos = new Vector3(this.transform.position.x + Random.Range(-3, 3), this.transform.position.y, this.transform.position.z + Random.Range(-3, 3));
      GameObject bot = Instantiate(botPrefab, startingPos, this.transform.rotation);
      bot.GetComponent<Brain>().Init();
      population.Add(bot);
    }
  }

  GameObject Breed(GameObject parent1, GameObject parent2)
  {
    Vector3 startingPos = new Vector3(this.transform.position.x + Random.Range(-3, 3), this.transform.position.y, this.transform.position.z + Random.Range(-3, 3));
    GameObject offspring = Instantiate(botPrefab, startingPos, this.transform.rotation);
    Brain brain = offspring.GetComponent<Brain>();

    if (Random.Range(0, 1000) == 1)
    { // Mutate 1 in 1000
      brain.Init();
      brain.dna.Mutate();
    }
    else
    {
      brain.Init();
      brain.dna.Combine(parent1.GetComponent<Brain>().dna, parent2.GetComponent<Brain>().dna);
    }

    return offspring;
  }

  void BreedNewPopulation()
  {
    List<GameObject> sortedList = population.OrderBy(o => o.GetComponent<Brain>().distanceTravelled).ToList();

    population.Clear();

    // Breed upper half of sorted list
    for (int i = (int)(sortedList.Count / 2.0f) - 1; i < sortedList.Count - 1; i++)
    {
      population.Add(Breed(sortedList[i], sortedList[i + 1]));
      population.Add(Breed(sortedList[i + 1], sortedList[i]));
    }

    // Destroy all parents and previous population
    foreach (GameObject bot in sortedList)
    {
      Destroy(bot);
    }

    generation++;
  }

  // Update is called once per frame
  void Update()
  {
    elapsed += Time.deltaTime;
    if (elapsed >= trialTime)
    {
      BreedNewPopulation();
      elapsed = 0;
    }
  }
}
