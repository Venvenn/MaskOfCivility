using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Escalon.Unity
{
    public class ApplicationManagerUnity : ApplicationManager
    {
        public ApplicationManagerUnity()
        {
            Debug.Init(new DebugLoggerUnity());
        }
    
        public sealed override void Init()
        {
            InputManagerUnity inputManager = new InputManagerUnity(Camera.main);
            NovaInputProcessor novaInputProcessor = new NovaInputProcessor(inputManager);
            Container.AddAspect(inputManager);
            Container.AddAspect(novaInputProcessor);
            
            ResolutionManagerUnity resolutionManager = new ResolutionManagerUnity();
            resolutionManager.Init();
            Container.AddAspect(resolutionManager);
        }

        public override float GetDeltaTime()
        {
            return Time.deltaTime;
        }
        
        public override float GetTimeSinceStartup()
        {
            return Time.realtimeSinceStartup;
        }
        
        public override void AddViews()
        {
        }

        public override void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        public override void Pause(bool pause)
        {
            if (pause)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
        }

        public override string GetDataPath(string additionalPath = "")
        { 
            return $"{Application.streamingAssetsPath}/{additionalPath}";
        }

        public override async Task LoadAssets()
        {
        }

        public override void AddOnQuitAction(Action onQuit)
        {
            
        }
    }
}



