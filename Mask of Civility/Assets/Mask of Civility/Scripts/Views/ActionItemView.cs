using System;
using System.Collections.Generic;
using Escalon;
using Nova;
using NovaSamples.UIControls;
using UnityEngine;

[Serializable]
public class ActionItemView : ButtonVisuals
{
    public TextBlock Name;
    public TextBlock Description;
    public ListView Costs;

    public void Init(TileActionData actionData, SerializableDictionary<ResourceType, Sprite> icons)
    {
        Costs.RemoveDataBinder<CostVisual, ImageButtonVisuals>(BindCost);
        Costs.AddDataBinder<CostVisual, ImageButtonVisuals>(BindCost);
        
        Name.Text = actionData.Name;
        Description.Text = actionData.Text;
        
        List<CostVisual> costVisuals = new List<CostVisual>(actionData.Cost.Count);
        foreach (var cost in actionData.Cost)
        {
            costVisuals.Add(new CostVisual()
            {
                Sprite = icons[cost.Key],
                Value = cost.Value
            });
        }
    }
    
    private void BindCost(Data.OnBind<CostVisual> evt, ImageButtonVisuals visuals, int index)
    {
        CostVisual costVisual = evt.UserData;
        visuals.Image.SetImage(costVisual.Sprite);
        visuals.Label.Text = costVisual.Value.ToString();
    }

    private struct CostVisual
    {
        public Sprite Sprite;
        public int Value;
    }
}
