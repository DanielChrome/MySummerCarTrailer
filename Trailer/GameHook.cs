using HutongGames.PlayMaker;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Trailer
{
    public class GameHook
    {
        private class FsmHookAction : FsmStateAction
        {
            public Action hook;

            public override void OnEnter()
            {
                this.hook();
                base.Finish();
            }
        }

        public static void InjectStateHook(GameObject gameObject, string stateName, Action hook)
        {
            FsmState stateFromGameObject = GameHook.GetStateFromGameObject(gameObject, stateName);
            if (stateFromGameObject != null)
            {
                List<FsmStateAction> list = new List<FsmStateAction>(stateFromGameObject.Actions);
                list.Insert(0, new GameHook.FsmHookAction
                {
                    hook = hook
                });
                stateFromGameObject.Actions = list.ToArray();
            }
        }

        private static FsmState GetStateFromGameObject(GameObject obj, string stateName)
        {
            PlayMakerFSM[] components = obj.GetComponents<PlayMakerFSM>();
            PlayMakerFSM[] array = components;
            for (int i = 0; i < array.Length; i++)
            {
                PlayMakerFSM playMakerFSM = array[i];
                FsmState fsmState = playMakerFSM.FsmStates.FirstOrDefault((FsmState x) => x.Name == stateName);
                if (fsmState != null)
                {
                    return fsmState;
                }
            }
            return null;
        }
    }
}
