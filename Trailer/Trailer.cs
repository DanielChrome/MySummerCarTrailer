using MSCLoader;
using UnityEngine;
using System.IO;
using System;

namespace Trailer
{
    public class Trailer : Mod
    {
        public override string ID { get { return "Trailer"; } }
        public override string Name { get { return "Trailer"; } }
        public override string Author { get { return "Chrome"; } }
        public override string Version { get { return "1.4"; } }
        public override bool UseAssetsFolder { get { return true; } }

        private bool m_isLoaded;
        private AssetBundle m_bundle;
        private GameObject trailer;
        private GameObject hitch;
        private GameObject hitchv;

        // Update is called once per frame
        public override void Update()
        {
            if (Application.loadedLevelName == "GAME")
            {
                try
                {
                    if (!this.m_isLoaded)
                    {
                        if (GameObject.Find("PLAYER/Pivot/Camera/FPSCamera/1Hand_Assemble/Hand") == null)
                        {
                            return;
                        }
                        if (GameObject.Find("SATSUMA(557kg, 248)") == null)
                        {
                            return;
                        }

                        string patch = Path.Combine(ModLoader.GetModAssetsFolder(this), "trailerbundle");
                        if (!File.Exists(patch))
                        {
                            ModConsole.Error("Couldn't find asset bundle from path " + patch);
                            return;
                        }
                        else
                        {
                            this.m_bundle = LoadAssets.LoadBundle(this,patch);
                            if (m_bundle != null)
                            {
                                hitch = UnityEngine.Object.Instantiate<GameObject>(this.m_bundle.LoadAsset<GameObject>("hitch"));
                                hitch.AddComponent<HitchBehaviour>();

                                hitchv = UnityEngine.Object.Instantiate<GameObject>(this.m_bundle.LoadAsset<GameObject>("hitch"));
                                hitchv.AddComponent<HitchVBehaviour>();

                                trailer = UnityEngine.Object.Instantiate<GameObject>(this.m_bundle.LoadAsset<GameObject>("carreta"));
                                trailer.AddComponent<TrailerBehaviour>();

                                trailer.GetComponent<TrailerBehaviour>().Hitch = hitch;
                                trailer.GetComponent<TrailerBehaviour>().HitchV = hitchv;

                                this.m_bundle.Unload(false);
                            }
                            else
                            {
                                ModConsole.Print("Bundle Null: " + patch);
                            }
                        }
                        this.m_isLoaded = true;
                    }
                    return;
                }
                catch (Exception ex)
                {
                    this.m_isLoaded = false;
                    ModConsole.Error(ex.ToString());
                    return;
                }
            }
            if (Application.loadedLevelName != "GAME" && this.m_isLoaded)
            {               
                this.m_isLoaded = false;
            }
        }
    }
}
