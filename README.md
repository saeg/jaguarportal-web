# Jaguar Portal Web
Jaguar Portal Web is a web application responsible for receiving and displaying Spectrum-based Fault Localization (SFL) information.

## Jaguar Portal
Jaguar Portal is a solution composed of a group of tools that together bring the Spectrum-based Fault Localization (SFL) functionality in a continuous integration environment:
- [Jaguar 2](https://github.com/saeg/jaguar2) - JavA coveraGe faUlt locAlization Rank 2 - Jaguar implements the Spectrum-based Fault Localization (SFL) technique for Java programs.
- [Jaguar Portal Web](https://github.com/saeg/jaguarportal-web) - Web API and Web Site responsible for receiving and displaying Spectrum-based Fault Localization (SFL) information.
- [Jaguar Portal Submit](https://github.com/saeg/jaguarportal-submit) - Command Line responsible for submitting Spectrum-based Fault Localization (SFL) data to the Jaguar Portal Web API. This component is also available as a GitHub Action, for use in GitHub Actions.

In Jaguar Portal Web, where you can view SFL information together with the code, showing suspicious lines marked with colors ranging from green (least suspicious) to red (most suspicious):
![image](https://github.com/user-attachments/assets/774e3513-c45b-4b4c-8c30-8518d55b7510)


## Required component:
- .NET 6 runtime is required.
- PostgreSQL is required and must be configured in the tag `ConnectionString__DefaultConnection` tag in `appsettings.config`: