using System.Collections.Generic;
using Arch.Core;
using Arch.Core.Extensions;
using Escalon;
using Nova;
using UnityEngine;

public class GenerationScreenUI : MonoBehaviour
{
    [SerializeField]
    private UIBlock2D _loadingScreen;
    
    private CoreManagers _coreManagers;

    public void Init(CoreManagers coreManagers)
    {
        _coreManagers = coreManagers;
        Notification.AddObserver<FSGeneration>(Reveal, FSGeneration.k_reveal);
    }

    public void Reveal(object sender, object args)
    {
        _loadingScreen.gameObject.SetActive(false);
    }
    
    public void EnterGame()
    {
        this.PostNotification(FSGeneration.k_beginGame);
    }
}
