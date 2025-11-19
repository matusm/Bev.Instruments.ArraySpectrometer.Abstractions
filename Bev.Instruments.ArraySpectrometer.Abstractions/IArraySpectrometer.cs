/*
 IArraySpectrometer.cs
 Bev.Instruments.ArraySpectrometer.Abstractions

 Purpose:
 - Defines the minimal contract required to interact with an array (multichannel) spectrometer device.
 - Provides device identification, wavelength calibration data and methods to read intensity data and control integration time.

 Contract / Expectations:
 - `Wavelengths` MUST be non-null and contain one or more entries. Values are expected to be in ascending order (typically nanometers).
 - `GetIntensityData()` MUST return a non-null `double[]` whose length equals `Wavelengths.Length`. Each intensity value corresponds index-for-index with `Wavelengths`.
 - `MinimumWavelength` and `MaximumWavelength` represent the valid wavelength bounds for the device (consistent with `Wavelengths`).
 - `SetIntegrationTime(double seconds)` uses seconds as the unit. Implementations SHOULD validate the value (positive, non-zero) and throw `ArgumentOutOfRangeException` for invalid values.
 - Methods may throw `InvalidOperationException` if the instrument is not connected or is in an error state.

 Thread-safety:
 - Instances are not guaranteed to be thread-safe. Callers should synchronize access if the same instance is used from multiple threads.

 Typical usage pattern:
 1. Configure device (e.g., `SetIntegrationTime(0.1)`).
 2. Optionally check `GetIntegrationTime()`.
 3. Acquire data via `GetIntensityData()` and map values to `Wavelengths`.

 Remarks:
 - This file contains only the interface/contract. Device-specific behaviors (buffering, averaging, asynchronous acquisition) belong to concrete implementations.
 - Keep implementations lightweight and explicit about exception behavior.

 Copyright: Michael Matus (project)
*/

namespace Bev.Instruments.ArraySpectrometer.Abstractions
{
    public interface IArraySpectrometer
    {
        string InstrumentManufacturer { get; }
        string InstrumentType { get; }
        string InstrumentSerialNumber { get; }
        string InstrumentFirmwareVersion { get; }

        double[] Wavelengths { get; }
        double MinimumWavelength { get; }
        double MaximumWavelength { get; }

        double[] GetIntensityData();
        void SetIntegrationTime(double seconds);
        double GetIntegrationTime();
    }
}
