using System;
using System.IO;
using UnityEngine;
using MSCLoader;

namespace Trailer
{
    public class TrailerBehaviour : MonoBehaviour
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


        public WheelCollider[] wheelColliders = new WheelCollider[2];
        public Transform[] tireMeshes = new Transform[2];

        //private Collider m_coffinTrigger;

        //private GameObject m_coffinPrefab;

        private HingeJoint Joint;
        private Collider m_Connector;
        public GameObject Hitch;
        public GameObject HitchV;
        private GameObject SATSUMA;
        private GameObject CAMERA;
        private Rigidbody ridgbody;
        private Transform centerOfMass;
        private Transform Support;

        private bool m_isConnect;
        private bool m_isCaught;
        private bool m_connectColision;
        private bool m_connectColisionV;

        //private AudioSource m_audio;

        private void Start()
        {
            try
            {
                //ModConsole.Print("Load Trailer");
                base.gameObject.name = "trailer";
                //base.gameObject.layer = LayerMask.NameToLayer("Object");
                //base.gameObject.tag = "OBJECT";
                //ModConsole.Print("Find Trailer");
                if (transform != null)
                {
                    m_Connector = transform.FindChild("BodyCollider/ConnectTrigger").GetComponent<Collider>();
                    centerOfMass = transform.FindChild("CenterOfMass");
                }
                else
                {
                   // ModConsole.Print("Transform Null");
                    return;
                }

                if (centerOfMass != null)
                {
                    //ModConsole.Print("Definiu centro de gravidade.");
                    ridgbody = GetComponent<Rigidbody>();
                    ridgbody.centerOfMass = centerOfMass.localPosition;
                }
                this.Load();
               // ModConsole.Print("Trailer Loaded");
                GameHook.InjectStateHook(GameObject.Find("ITEMS"), "Save game", new Action(this.Save));
            }catch(Exception E)
            {
                ModConsole.Error("Start Error: " + E.ToString());
            }
        }

        private void UpdateWhellsRotation()
        {
            for (int i = 0; i < 2; i++)
            {
                Quaternion quat;
                Vector3 pos;
                wheelColliders[i].GetWorldPose(out pos, out quat);
                tireMeshes[i].position = pos;
                tireMeshes[i].rotation = quat;
                
            }
        }

        private void Update()
        {
            UpdateWhellsRotation();
            if (Input.GetMouseButtonDown(0) && m_isCaught && Joint != null && !m_isConnect)
            {
                Support.localScale = new Vector3(1f, 1f, 1f);
                Destroy(Joint);
                m_isCaught = false;
            }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit;
            if (Physics.Raycast(ray, out raycastHit))
            {
                if (raycastHit.collider.name == "ConnectTrigger")
                {
                    //ModConsole.Print(m_connectColision + " - " + m_connectColisionV);
                    if (m_connectColision || m_connectColisionV)
                    {
                       // ModConsole.Print("m_connectColision");
                        PlayMakerGlobals.Instance.Variables.FindFsmBool("GUIuse").Value = true;
                        PlayMakerGlobals.Instance.Variables.FindFsmString("GUIinteraction").Value = this.m_isConnect ? "Disconnect" : "Connect";
                        if (cInput.GetButtonDown("Use"))
                        {
                            //ModConsole.Print("Use Comand " + m_isConnect);
                            if (!m_isConnect)
                            {
                                //ModConsole.Print("Conneting");
                                try
                                {
                                    if (Joint != null)
                                        Destroy(Joint);
                                    Joint = base.gameObject.AddComponent<HingeJoint>();
                                    
                                    //ModConsole.Print("ridgbody localizado");
                                    if (m_connectColisionV && HitchV != null)
                                    {
                                        //ModConsole.Print("HiutchV localizado");
                                        CreateJoint(new Vector3(0f, 0f, 0f), HitchV.GetComponent<Rigidbody>(), false);
                                    }
                                    else
                                    if (Hitch != null)
                                    {
                                        //ModConsole.Print("Hitch localizado");
                                        CreateJoint(new Vector3(0f, 0f, 0f), Hitch.GetComponent<Rigidbody>(), false);
                                    }

                                }
                                catch (Exception e)
                                {
                                    ModConsole.Error("Connect Erro: " + e.ToString());
                                }
                            }
                            else
                            {
                                ModConsole.Print("Desconnecting");
                                if (Joint != null)
                                {
                                    Destroy(Joint);
                                    Support.localScale = new Vector3(1f, 1f, 1f);
                                }
                            }
                            this.m_isConnect = !this.m_isConnect;


                            return;
                        }
                        else
                        {
                            CreateJointToPlayer();
                        }
                    }
                    else
                    {
                        CreateJointToPlayer();
                    }
                }
            }
        }

        void CreateJointToPlayer()
        {
            PlayMakerGlobals.Instance.Variables.FindFsmBool("GUIuse").Value = false;

            if (m_isConnect)
            {
                ModConsole.Print("Trailer Engage.");
                ModConsole.Print("Trailer Engage..");
                ModConsole.Print("Trailer Engage...");
                m_connectColision = true;
                return;
            }

            if (Input.GetMouseButtonDown(0))
            {
                //ModConsole.Print("Mouse Pressed");
                if (Joint == null)
                {
                    //ModConsole.Print("Add Joint to Player");
                    try
                    {
                        if (CAMERA.GetComponent<Rigidbody>() != null)
                        {
                            Joint = base.gameObject.AddComponent<HingeJoint>();
                            CreateJoint(new Vector3(0f, 0f, 1.5f), CAMERA.GetComponent<Rigidbody>(), true);
                            //transform.rotation = Quaternion.Euler(359f, transform.rotation.y, transform.rotation.z);
                            //base.GetComponent<Rigidbody>().mass = 50f;//Mais leve
                            m_isCaught = true;
                            m_connectColision = false;
                            m_connectColisionV = false;
                        }
                        else
                        {
                            ModConsole.Print("Player dont have Ridgibody.");
                        }
                    }
                    catch (Exception e)
                    {
                        ModConsole.Error("Error Player Joing: " + e.ToString());
                    }
                }
                else
                {
                    //ModConsole.Print("Destroy Player joing");
                    Destroy(Joint);
                    base.GetComponent<Rigidbody>().mass = 200f;//Massa original
                }
            }
        }

        void OnTriggerEnter(Collider other)
        {
            //ModConsole.Print(other.gameObject.name);
            if (other.gameObject.name == "HitchTrigger")
            {
                //ModConsole.Print(other.gameObject.name);
                if (other.gameObject.transform.parent != null)
                {
                    //ModConsole.Print("Parent1: " + other.gameObject.transform.parent.name);
                    if (other.gameObject.transform.parent.parent != null)
                    {
                        //ModConsole.Print("Parent2: " + other.gameObject.transform.parent.parent.name);
                        if (other.gameObject.transform.parent.parent.gameObject.name == "hitch")
                            m_connectColision = true;
                        else
                        if (other.gameObject.transform.parent.parent.gameObject.name == "hitchv")
                            m_connectColisionV = true;
                    }
                }
                
            }
            
        }

        private void CreateJoint(Vector3 connectAnchor, Rigidbody connectBody, bool auto)
        {
            try
            {
                //ModConsole.Print("Criando Joint");
                if (Joint != null)
                {
                    //ModConsole.Print("JOINT COM: "+connectBody.gameObject.name);

                    if (connectBody != null)
                        Joint.connectedBody = connectBody;

                    Joint.autoConfigureConnectedAnchor = auto;
                    Joint.connectedAnchor = connectAnchor;
                    Joint.anchor = new Vector3(0f, 0.39f, -1.75f);
                    //if (auto)
                    //    Joint.axis = new Vector3(1f, 1f, 0f);
                    //else
                    //    Joint.axis = new Vector3(1f, 1f, 0f);
                    JointLimits limits = Joint.limits;
                    limits.min = -75;
                    limits.max = 75;
                    Joint.limits = limits;
                    Joint.useLimits = true;
                    //JointMotor motor = Joint.motor;
                    //motor.freeSpin = true;
                    //Joint.motor = motor;

                    Joint.enableCollision = false;
                    // ModConsole.Print("Joint Criada");
                    Support.localScale = new Vector3(1f, 0f, 1f);
                }
            }
            catch(Exception e)
            {
                ModConsole.Error("Joint Error: "+e.ToString());
            }

        }

        private void Load()
        {
            //ModConsole.Print("Load()");
            string path = Path.Combine(Application.persistentDataPath, "trailer.xml");
            //ModConsole.Print("Save Patch:" + path);
            if (File.Exists(path))
            {
                TrailerBehaviour.SaveData saveData = SaveUtil.DeserializeReadFile<SaveData>(path);
                //ModConsole.Print("Set save Location: " + saveData.posX + "," + saveData.posY + "," + saveData.posZ);
                base.transform.position = (new Vector3(saveData.posX, saveData.posY, saveData.posZ));
                base.transform.rotation = (Quaternion.Euler(saveData.rotX, saveData.rotY, saveData.rotZ));
            }
            else
            {
                //ModConsole.Print("Set base Location");
                base.transform.position = (new Vector3(-1551.468f, 2.93f, 1178.136f));
                base.transform.rotation = (Quaternion.Euler(0F, 323.95f, 0f));
            }
            Support = transform.FindChild("Support");
            Support.localScale = new Vector3(1f, 1f, 1f);
            wheelColliders[0] = base.transform.FindChild("WheelColliders/WheelColliderL").GetComponent<WheelCollider>();
            wheelColliders[1] = base.transform.FindChild("WheelColliders/WheelColliderR").GetComponent<WheelCollider>();
            for (int i = 0;i<2; i++)
            {
                WheelFrictionCurve foward = wheelColliders[i].forwardFriction;
                foward.extremumSlip = 0.001f;
                wheelColliders[i].forwardFriction = foward;
            }
            tireMeshes[0] = base.transform.FindChild("Mesh/Tires/TireL").GetComponent<Transform>();
            tireMeshes[1] = base.transform.FindChild("Mesh/Tires/TireR").GetComponent<Transform>();

            if (GameObject.Find("SATSUMA(557kg, 248)") != null)
            {
                //ModConsole.Print("Carregou Satsuma");
                SATSUMA = GameObject.Find("SATSUMA(557kg, 248)");
            }
            if (GameObject.Find("PLAYER/Pivot/Camera/FPSCamera/1Hand_Assemble/Hand") != null)
            {
                //ModConsole.Print("Carregou CAMERA");
                CAMERA = GameObject.Find("PLAYER/Pivot/Camera/FPSCamera/1Hand_Assemble/Hand");
            }
            else
                ModConsole.Print("Player NULL");
        }

        private void Save()
        {
            string path = Path.Combine(Application.persistentDataPath, "trailer.xml");
            SaveUtil.SerializeWriteFile(new SaveData
            {
                posX = base.transform.position.x,
                posY = base.transform.position.y,
                posZ = base.transform.position.z,
                rotX = base.transform.rotation.eulerAngles.x,
                rotY = base.transform.rotation.eulerAngles.y,
                rotZ = base.transform.rotation.eulerAngles.z
            }, path);
        }
    }
}
