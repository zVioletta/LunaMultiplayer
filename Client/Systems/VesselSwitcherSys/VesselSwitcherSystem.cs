﻿using LunaClient.Base;
using System;
using System.Collections;
using UnityEngine;

namespace LunaClient.Systems.VesselSwitcherSys
{
    /// <summary>
    /// This system handles the vessel loading into the game and sending our vessel structure to other players.
    /// We only load vesels that are in our subspace
    /// </summary>
    public class VesselSwitcherSystem : Base.System
    {
        #region Fields & properties

        private static Guid? VesselToSwitchTo { get; set; }

        #endregion

        #region Base overrides

        protected override void OnEnabled()
        {
            base.OnEnabled();
            SetupRoutine(new RoutineDefinition(1000, RoutineExecution.Update, CheckVesselToSwitch));
        }

        protected override void OnDisabled()
        {
            base.OnDisabled();
            VesselToSwitchTo = null;
        }

        #endregion

        #region Public

        /// <summary>
        /// Specifies the vessel that we should switch to
        /// </summary>
        public void SwitchToVessel(Guid vesselId)
        {
            VesselToSwitchTo = vesselId;
        }

        #endregion

        #region Update methods

        /// <summary>
        /// Checks if we must switch to a vessel and do so
        /// </summary>
        private void CheckVesselToSwitch()
        {
            try
            {
                if (VesselToSwitchTo.HasValue)
                {
                    Client.Singleton.StartCoroutine(SwitchToVessel());
                }
            }
            catch (Exception e)
            {
                LunaLog.LogError($"[LMP]: Error in SendVesselDefinition {e}");
            }

        }

        #endregion

        #region Private

        /// <summary>
        /// Switch to the vessel
        /// </summary>
        private static IEnumerator SwitchToVessel()
        {
            yield return new WaitForSeconds(0.25f);
            if (VesselToSwitchTo.HasValue)
            {
                var vesselToSwitchTo = FlightGlobals.FindVessel(VesselToSwitchTo.Value);
                if (vesselToSwitchTo != null)
                {
                    FlightGlobals.ForceSetActiveVessel(FlightGlobals.FindVessel(VesselToSwitchTo.Value));
                    if (FlightGlobals.ActiveVessel?.loaded ?? false)
                        VesselToSwitchTo = Guid.Empty;
                }
            }
        }

        #endregion
    }
}
