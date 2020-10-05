using UnityEngine;

public class Objective
{
    private string objective;

    private bool completed = false;

    public Objective(string objective)
    {
        this.objective = objective;
    }

    public string GetObjective()
    {
        return objective;
    }

    public bool IsCompleted()
    {
        return completed;
    }

    public void Complete()
    {
        completed = true;
    }
}
