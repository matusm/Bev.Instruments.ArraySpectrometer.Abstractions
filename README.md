# Bev.Instruments.ArraySpectrometer.Abstractions

A small, well-documented interface library that defines the minimal contract required to interact with array (multichannel) spectrometer devices.

This repository contains the `IArraySpectrometer` interface (`IArraySpectrometer.cs`) which describes device identification, wavelength calibration, data acquisition and integration-time control. Concrete drivers and consumers implement this contract.

## Quick overview

- Target framework: .NET Framework 4.7.2  
- C# language level: 7.3  
- Primary API surface: `IArraySpectrometer`

The interface is intentionally minimal so implementations can remain lightweight and portable across different hardware vendors.

## Interface summary

`IArraySpectrometer` exposes:
- Identification properties: `InstrumentManufacturer`, `InstrumentType`, `InstrumentSerialNumber`, `InstrumentFirmwareVersion`
- Calibration and range: `double[] Wavelengths`, `MinimumWavelength`, `MaximumWavelength`
- Data acquisition: `double[] GetIntensityData()`
- Integration time control: `void SetIntegrationTime(double seconds)`, `double GetIntegrationTime()`

Contract expectations (short):
- `Wavelengths` MUST be non-null and contain >= 1 entry; values expected in ascending order (typical units: nm).
- `GetIntensityData()` MUST return a non-null `double[]` whose length equals `Wavelengths.Length`.
- `SetIntegrationTime` uses seconds. Implementations SHOULD validate the value and throw `ArgumentOutOfRangeException` for invalid values.
- Methods may throw `InvalidOperationException` if the instrument is not connected or in an error state.
- Instances are not guaranteed to be thread-safe — callers should synchronize access if needed.

## Consumers in this repository

The following projects in this solution consume and implement `IArraySpectrometer`:
- `Bev.Instruments.Thorlabs.Ctt`
- `Bev.Instruments.Thorlabs.Css`

These projects provide Thorlabs-specific implementations/drivers that adhere to the `IArraySpectrometer` contract (see their project directories for usage examples and device-specific notes).

## Typical usage

1. Obtain an instance of a concrete implementation (factory or DI container).
2. Configure integration time: `SetIntegrationTime(0.1)` (seconds).
3. Optionally call `GetIntegrationTime()` to confirm.
4. Acquire data: `var intensities = GetIntensityData()` and map each value to `Wavelengths`.

Example (conceptual):

## Building

Open the solution in Visual Studio 2022 and build the solution:
- File -> Open -> Project/Solution or use the command line `msbuild` for the solution file.
- Ensure your project references target `.NET Framework 4.7.2`.

If you need to adjust IDE settings, look under __Tools > Options__ and project properties for framework/target settings.

## Contributing

- Follow the existing coding style in the project.
- Add unit tests for any behavior you introduce.
- Document any deviations from the interface contract in the implementing project.

## License

See the repository root for licensing information.

## Contact / Maintainer

For questions about the interface or implementations, check the concrete driver projects (`Bev.Instruments.Thorlabs.Ctt`, `Bev.Instruments.Thorlabs.Css`) and open an issue or pull request on the repository.