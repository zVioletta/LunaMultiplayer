﻿using System;

namespace LunaClient.Systems.VesselProtoSys
{
    public class VesselProtoUpdate
    {
        public Guid VesselId { get; set; }
        public ProtoVessel ProtoVessel { get; set; }
        public bool Loaded { get; set; }

        private Vessel _vessel;
        public Vessel Vessel => _vessel ?? (_vessel = FlightGlobals.FindVessel(VesselId));

        public bool VesselExist => Vessel != null;

        public VesselProtoUpdate(ConfigNode vessel, Guid vesselId)
        {
            VesselId = vesselId;
            ProtoVessel = VesselCommon.CreateSafeProtoVesselFromConfigNode(vessel, vesselId);
        }

        public VesselProtoUpdate(VesselProtoUpdate protoUpdate)
        {
            VesselId = protoUpdate.VesselId;
            ProtoVessel = protoUpdate.ProtoVessel;
        }
    }
}