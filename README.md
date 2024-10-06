[![SonarCloud](https://github.com/MadWorldNL/MantaRayPlan/actions/workflows/sonarqube.yaml/badge.svg)](https://github.com/MadWorldNL/MantaRayPlan/actions/workflows/sonarqube.yaml)
[![CodeQL](https://github.com/MadWorldNL/MantaRayPlan/actions/workflows/github-code-scanning/codeql/badge.svg)](https://github.com/MadWorldNL/MantaRayPlan/actions/workflows/github-code-scanning/codeql)

# MantaRayPlan 
MantaRayPlan is a scheduling application that allows you to share your agenda with friends and family. 
Designed for multiple users, this application is available on multiple platforms, making it easier to plan together.

> [!WARNING]
> Currently, MantaRayPlan is in the setup phase and is not yet usable.

## Key Features
*TODO*

## Getting Started
### Prerequisites
Download the following software
* Docker 
* .NET 8 SDK

### Installation
Installation guide for your development environment
#### Option 1: From Docker
* Clone this repository
* Run this command in the src folder:
```shell
docker compose --profile all up
```

#### Option 2: From your editor
* Clone this repository
* Run this command in the src folder:
```shell
sudo dotnet workload restore
docker compose up
```
* Set this projects as startup:
    * Admin.Bff
    * Admin.Web
    * Viewer.Bff
    * Viewer.Mobile
    * Viewer.Web
    * Api.Grpc *
    * Api.MessageBus *
* Press run

*This project is required to set as startup project

`dotnet workload restore` is required to install the necessary workloads for the project. However, when you download a new update,
it may be necessary to run this command to ensure all workloads are up to date.

### Running the tests
Before running the tests, ensure Docker is up and running.

#### Option 1: From the cli
Run the test projects in the src folder:
```shell
sudo dotnet workload restore
dotnet test
```

#### Option 2: From your editor
Run the test projects by pressing the unit test run button in your editor.

## Production Applications
Want to see the applications in action? Check out the links below:
* **Web Viewer:** Here you can view the application as a user.
  * [https://mantarayplan.mad-world.nl](https://mantarayplan.mad-world.nl/)
* **Api Bff Viewer:** Here the web viewer gets its data.
  * [https://viewer-bff-mantarayplan.mad-world.nl](https://viewer-bff-mantarayplan.mad-world.nl/swagger/index.html)
* **Web Admin:** Here you can view the application as an admin.
  * [https://admin-mantarayplan.mad-world.nl](https://admin-mantarayplan.mad-world.nl/)
* **Api Bff Admin:** Here the web admin gets its data.
  * [https://admin-bff-mantarayplan.mad-world.nl](https://admin-bff-mantarayplan.mad-world.nl/swagger/index.html)

## Documentation
*TODO*

## Acknowledgements
I would like to thank the following sources and individuals for their templates, which I used in this application:
- [Bootstrap 5 theme - Elegant](https://themewagon.com/themes/free-bootstrap-5-html-5-admin-dashboard-website-template-elegant/) - Made by: Freebibuges
- [Cloud Native PG](https://cloudnative-pg.io/) - Made by: The CloudNativePG Contributors.

Your work has been incredibly helpful, and I greatly appreciate your contributions and generosity in sharing these resources.