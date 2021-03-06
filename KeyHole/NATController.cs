﻿using System;

namespace KeyHoleNAT {
    public class NATController {
        public delegate void ProgressUpdateHandler(KeyHoleEventMessage e);

        public GlobalOptions GlobalOptions;
        public STUNOptions STUNOptions;
        public UPNPOptions UPNPOptions;

        private UPNPModule upnpModule;

		public NATController(UPNPOptions upnpOptions = null, STUNOptions stunOptions = null, GlobalOptions globalOptions = null,
            ProgressUpdateHandler onProgressUpdate = null, ProgressUpdateHandler onProgressFinished = null) {
			UPNPOptions = upnpOptions ?? new UPNPOptions();
            STUNOptions = stunOptions ?? new STUNOptions();
            GlobalOptions = globalOptions ?? new GlobalOptions();

            OnProgressUpdate += onProgressUpdate;
            OnProgressFinish += onProgressFinished;

            // Start UPnP module:
            if(UPNPOptions.Enabled)
                upnpModule = new UPNPModule(UPNPOptions, GlobalOptions, HandleProgressUpdate, HandleProgressFinish);
        }

        /// <summary>
        /// Asyncronous method to bind the port specified in the Global Options within the given timeout
        /// period.
        /// </summary>
		public void BindPort(UInt16 portToBind, IPProtocol ipProtocol = IPProtocol.Both, string portDescription = "") {
            if(upnpModule != null)
                upnpModule.BindPort(portToBind, ipProtocol, portDescription);
        }

        public event ProgressUpdateHandler OnProgressUpdate;
        public event ProgressUpdateHandler OnProgressFinish;

        private void HandleProgressUpdate(KeyHoleEventMessage keyHoleEventMessage) {
            OnProgressUpdate(keyHoleEventMessage);
        }

        private void HandleProgressFinish(KeyHoleEventMessage keyHoleEventMessage) {
            OnProgressFinish(keyHoleEventMessage);

            // TODO: Handle errors here by moving on to the next method of NAT traversal, if necessary.
        }
    }

    public enum EventLoggingLevel {
        Informational,
        Warning,
        Error,
        Debug
    }
}