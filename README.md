# pdfgenerator

1. Download the repository
2. Open the solution in Visual Studio 2022 Preview, and build.
3. Copy the Rotativa folder at the build output.
4. Run the solution. It should expose an API to generate a report.
5. Invoke a POST /api/report request with the following payload:

{
  "qrCodedata": null,
  "serialNumber": null,
  "feederNumber": null,
  "customText": null,
  "size": 0
}
