﻿using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using BleLab.Commands.Exceptions;
using BleLab.Model;

namespace BleLab.Commands.Characteristic
{
    public class WriteBytesCommand : BytesCommandBase
    {
        public WriteBytesCommand(CharacteristicInfo characteristicInfo, byte[] bytes, bool withoutResponse = false) : base(characteristicInfo)
        {
            WithoutResponse = withoutResponse;
            Bytes = bytes;
        }

        public bool WithoutResponse { get; }

        protected override async Task DoExecuteAsync()
        {
            var result = await WindowsRuntimeSystemExtensions.AsTask(Characteristic.WriteValueAsync(Bytes.AsBuffer(), WithoutResponse ? GattWriteOption.WriteWithoutResponse : GattWriteOption.WriteWithResponse)).ConfigureAwait(false);

            if (result == GattCommunicationStatus.Unreachable)
                throw new DeviceUnreachableException();
        }
    }
}
