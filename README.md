# Bev.Instruments.ArraySpectrometer.Abstractions

A small, well-documented interface library that defines the minimal contract required to interact with array (multichannel) spectrometer devices.

This repository contains the `IArraySpectrometer` interface (`IArraySpectrometer.cs`), describing device identification, wavelength calibration, data acquisition, saturation detection, and integration-time control. Concrete drivers and consumers implement this contract.

## Quick overview

- Target framework: .NET Framework 4.7.2  
- C# language level: 7.3  
- Primary API surface: `IArraySpectrometer`

The interface is intentionally minimal so implementations can remain lightweight and portable across different hardware vendors.

## Interface summary

`IArraySpectrometer` exposes:
- Identification: `InstrumentManufacturer`, `InstrumentType`, `InstrumentSerialNumber`, `InstrumentFirmwareVersion`
- Calibration and range: `double[] Wavelengths`, `MinimumWavelength`, `MaximumWavelength`
- Saturation: `double SaturationLevel` (maximum measurable intensity for the current configuration)
- Integration time limits: `double MinimumIntegrationTime`, `double MaximumIntegrationTime` (valid bounds for integration time in seconds)
- Data acquisition: `double[] GetIntensityData()`
- Integration time: `void SetIntegrationTime(double seconds)`, `double GetIntegrationTime()`

Contract expectations (short):
- `Wavelengths` MUST be non-null and contain >= 1 entry; values expected in ascending order (typical units: nm).
- `GetIntensityData()` MUST return a non-null `double[]` whose length equals `Wavelengths.Length`.
- `SaturationLevel` SHOULD bound measured intensities: values from `GetIntensityData()` are typically in `[0, SaturationLevel]`. Use it to detect clipping and tune acquisition (e.g., reduce integration time).
- `MinimumIntegrationTime` and `MaximumIntegrationTime` define valid bounds for `SetIntegrationTime(...)` in seconds. Implementations SHOULD enforce these bounds and reflect the actual value via `GetIntegrationTime()`.
- `SetIntegrationTime` uses seconds. Implementations SHOULD validate the value and throw `ArgumentOutOfRangeException` for invalid values.
- Methods may throw `InvalidOperationException` if the instrument is not connected or in an error state.
- Instances are not guaranteed to be thread-safe — callers should synchronize access if needed.

## Consumers of this interface

The following projects consume and implement `IArraySpectrometer`:
- `Bev.Instruments.Thorlabs.Ctt`
- `Bev.Instruments.Thorlabs.Css`

These provide Thorlabs-specific implementations/drivers adhering to the contract (see their project directories for device-specific notes and examples).

## Typical usage

1. Obtain an instance of a concrete implementation (factory or DI).
2. Read `MinimumIntegrationTime` and `MaximumIntegrationTime`, then configure integration time within bounds: `SetIntegrationTime(0.1)` (seconds).
3. Acquire data: `var intensities = GetIntensityData()` and map each value to `Wavelengths`.
4. Check saturation: compare intensities to `SaturationLevel` to detect clipping.

Example (conceptual):