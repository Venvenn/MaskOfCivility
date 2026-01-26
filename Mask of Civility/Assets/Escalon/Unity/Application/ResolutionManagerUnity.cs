using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Escalon;

public class ResolutionManagerUnity : ResolutionManager<Resolution>
{
    private List<Resolution> _supportedResolutionList;
    private List<string> _supportedResolutionStringList;
    
    public override void Init()
    {
        Resolution[] resolutions = Screen.resolutions;
        var supportedResolutionList = new List<Resolution>();
        var supportedResolutionStringList = new List<string>();
        
        for (int i = 0 ; i < resolutions.Length ; i++)
        {
            if (resolutions[i].width >= 1280 && resolutions[i].height >= 720)
            {
                supportedResolutionList.Add(resolutions[i]);
                supportedResolutionStringList.Add($"{resolutions[i].width}x{resolutions[i].height}");
            }
        }

        _supportedResolutionList = supportedResolutionList.Distinct().ToList();
        _supportedResolutionStringList = supportedResolutionStringList.Distinct().ToList();
    }

    public override List<Resolution> GetSupportedResolutions()
    {
        return _supportedResolutionList;
    }

    public override List<string> GetSupportedResolutionsByString()
    {
        return _supportedResolutionStringList;
    }

    public override void SetResolution(Resolution resolution)
    {
        Screen.SetResolution(resolution.width , resolution.height , Screen.fullScreen);
    }

    public override void SetResolution(string resolution)
    {
        var split = resolution.Split('x');
        var width = int.Parse(split[0]);
        var height = int.Parse(split[1]);
        
        Screen.SetResolution(width , height , Screen.fullScreen);
    }
}