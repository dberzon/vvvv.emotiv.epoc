﻿#region usings
using System;
using System.ComponentModel.Composition;
using System.Collections;
using System.Collections.Generic;

using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;
using VVVV.Utils.VColor;
using VVVV.Utils.VMath;

using Emotiv;

using VVVV.Core.Logging;
#endregion usings

namespace VVVV.EmotivEpoc
{
    #region PluginInfo
    [PluginInfo(Name = "Cognitiv",
                Category = "EmoState",
                Help = "Exposes the Cognitiv properties of an EmoState, i.e. mental imagery",
                Tags = "Emotiv, Epoc, Cognitiv, EmoState")]
    #endregion PluginInfo
    public class CognitivEmoStateNode : IPluginEvaluate
    {
        #region fields & pins
        [Input("EmoState")]
        public IDiffSpread<EmoState> FEmoState;

        [Output("Current Action")]
        public ISpread<EdkDll.EE_CognitivAction_t> FCurrentAction;

        [Output("Power")]
        public ISpread<Single> FPower;

        [Output("Is Active")]
        public ISpread<Boolean> FIsActive;
        #endregion fields & pins


        #region vars
        private EdkDll.EE_CognitivAction_t mCogAction = EdkDll.EE_CognitivAction_t.COG_NEUTRAL;
        private Single mPower = 0;
        private Boolean mIsActive = false;
        #endregion vars

        //Update fields when Emo state updated
        void CognitivEmoStateUpdated()
        {
            if (FEmoState[0] != null)
            {
                EmoState es = FEmoState[0];

                mCogAction = es.CognitivGetCurrentAction();
                mPower = es.CognitivGetCurrentActionPower();
                mIsActive = es.CognitivIsActive();
            }
        }

        //Processing loop
        public void Evaluate(int SpreadMax)
        {
            if (FEmoState.IsChanged && FEmoState.SliceCount > 0)
                CognitivEmoStateUpdated();

            //Output data to pins
            FCurrentAction.SliceCount = 1;
            FCurrentAction[0] = mCogAction;

            FPower.SliceCount = 1;
            FPower[0] = mPower;

            FIsActive.SliceCount = 1;
            FIsActive[0] = mIsActive;
        }
    }
}
