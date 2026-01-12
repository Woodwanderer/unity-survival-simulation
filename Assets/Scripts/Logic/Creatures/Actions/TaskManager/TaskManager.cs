using System.Collections.Generic;
using UnityEngine;

public class TaskManager
{
    Queue<IAction> tasks;

    Queue<HarvestAction> harvestTasks;
    Queue<EatAction> eatTasks;

    IAction currentTask;


    

}
