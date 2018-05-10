using System;
using System.IO;
using UnityEngine;
using MSCLoader;

namespace Trailer
{
    public class HitchBehaviour : MonoBehaviour
    {
        public class SaveData
        {
            public float posX;

            public float posY;

            public float posZ;

            public float rotX;

            public float rotY;

            public float rotZ;
        }


        private GameObject SATSUMA;

        private void Start()
        {
            try
            {
                //ModConsole.Print("Load Hitch");
                base.gameObject.name = "hitch";
                base.gameObject.layer = LayerMask.NameToLayer("Parts");
                base.gameObject.tag = "PART";
                //ModConsole.Print("Find Trailer");

                Load();

               // ModConsole.Print("Hitch Loaded");
                
            }catch(Exception E)
            {
                ModConsole.Print("Start Hitch Error: " + E.ToString());
            }
        }

        private void Load()
        {
            //ModConsole.Print("Load()");
            
           // ModConsole.Print("Set parent to Satsuma");
            if (GameObject.Find("SATSUMA(557kg, 248)") != null)
            {
               // ModConsole.Print("Carregou Satsuma");
                SATSUMA = GameObject.Find("SATSUMA(557kg, 248)");

               // ModConsole.Print("Carregou Engate");
                transform.SetParent(SATSUMA.transform);
                transform.localScale = new Vector3(1f, 1f, 1f);
                transform.localPosition = new Vector3(0f, -0.075f, -1.90f);
                transform.localEulerAngles = new Vector3(0f, 0f, 0f);
                CreateJoint();
            }
        }

        private void CreateJoint()
        {
            try
            {
                //ModConsole.Print("Criando Joint Hitch");
                FixedJoint Joint = base.gameObject.GetComponent<FixedJoint>();
                if (Joint != null)
                {
                    if (SATSUMA.GetComponent<Rigidbody>() != null)
                        Joint.connectedBody = SATSUMA.GetComponent<Rigidbody>();
                   // ModConsole.Print("Joint Criada");
                }
            }
            catch (Exception e)
            {
                ModConsole.Print(e.ToString());
            }

        }
    }
}
