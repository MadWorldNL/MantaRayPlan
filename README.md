[![SonarCloud](https://github.com/MadWorldNL/MantaRayPlan/actions/workflows/sonarqube.yaml/badge.svg)](https://github.com/MadWorldNL/MantaRayPlan/actions/workflows/sonarqube.yaml)
[![CodeQL](https://github.com/MadWorldNL/MantaRayPlan/actions/workflows/github-code-scanning/codeql/badge.svg)](https://github.com/MadWorldNL/MantaRayPlan/actions/workflows/github-code-scanning/codeql)

# MantaRayPlan 
MantaRayPlan is a scheduling application that allows you to share your agenda with friends and family. 
Designed for multiple users, this application is available on multiple platforms, making it easier to plan together. 
Currently, MantaRayPlan is in the setup phase and is not yet usable.

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

### Running the tests
Before running the tests, ensure Docker is up and running.

#### Option 1: From the cli
Run the test projects in the src folder:
```shell
dotnet test
```

#### Option 2: From your editor
Run the test projects by pressing the unit test run button in your editor.

## Documentation
*TODO*

## Acknowledgements
I would like to thank the following sources and individuals for their templates, which I used in this application:
- [Bootstrap 5 theme - Elegant](https://themewagon.com/themes/free-bootstrap-5-html-5-admin-dashboard-website-template-elegant/) - Made by: Freebibuges

Your work has been incredibly helpful, and I greatly appreciate your contributions and generosity in sharing these resources.