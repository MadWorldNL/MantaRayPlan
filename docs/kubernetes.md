# Kubernetes
Kubernetes is an open-source container orchestration platform designed to automate the deployment, scaling, and operation
of containerized applications. It allows you to manage clusters of containers, providing robust mechanisms for deploying,
maintaining, and scaling applications.

## Prerequisites
Before installing Kubernetes on your local machine, ensure you have the following tools installed:
* Docker
* Git

## Install on local machine
### Step 1: Install Kubectl

kubectl is the command-line tool for interacting with the Kubernetes cluster.
[See Install Guide](https://kubernetes.io/docs/tasks/tools/)

### Step 2: Install Minikube
Minikube allows you to run a single-node Kubernetes cluster locally.
[See Install Guide](https://minikube.sigs.k8s.io/docs/start/?arch=%2Fmacos%2Farm64%2Fstable%2Fbinary+download)

### Step 3: Install Helm
Helm is a package manager for Kubernetes, which helps in defining, installing, and upgrading applications.
[See Install Guide](https://helm.sh/docs/intro/install/)

## Usage on local machine
### Create new helm chart
The `helm create` command generates a new Helm chart with a predefined directory structure and template files, facilitating the development of Kubernetes applications.
```shell
helm create Cloud
```

### Deploy to minikube
```shell
minikube start
minikube stop
minikube addons enable metrics-server
minikube addons enable ingress
minikube dashboard
minikube tunnel
helm install -f environments/values-development.yaml manta-ray-plan-cloud .
helm upgrade -f environments/values-development.yaml manta-ray-plan-cloud .
helm uninstall manta-ray-plan-cloud
```