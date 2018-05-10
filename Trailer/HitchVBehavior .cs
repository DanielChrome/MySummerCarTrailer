using System;
using System.IO;
using UnityEngine;
using MSCLoader;

namespace Trailer
{
    public class HitchVBehaviour : MonoBehaviour
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


        private GameObject VAN;
        private Rigidbody rb;

        private void Start()
        {
            try
            {
                //ModConsole.Print("Load Hitch");
                base.gameObject.name = "hitchv";
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
            if (GameObject.Find("HAYOSIKO(1500kg, 250)") != null)
            {
                //ModConsole.Print("Carregou vAN");
                VAN = GameObject.Find("HAYOSIKO(1500kg, 250)");
                rb = gameObject.GetComponent<Rigidbody>();
                // ModConsole.Print("Carregou Engate");
                transform.SetParent(VAN.transform);
                transform.localScale = new Vector3(1f, 1f, 1f);
                transform.localPosition = new Vector3(0f, 0.18f, -2.367f);
                transform.localEulerAngles = new Vector3(0f, 0f, 0f);
                //if (rb != null)
                //    rb.isKinematic = true;

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
                    if (VAN.GetComponent<Rigidbody>() != null)
                        Joint.connectedBody = VAN.GetComponent<Rigidbody>();
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
