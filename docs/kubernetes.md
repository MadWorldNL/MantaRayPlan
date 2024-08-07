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
helm create MantaRayPlanCloud
```

### Prepare minikube
First, start Minikube:
```shell
minikube start
```

The following addons are required:
```shell
minikube addons enable metrics-server
minikube addons enable ingress
```

Convenient tools for debugging Kubernetes:
```shell
minikube dashboard
```

Open access localhost domains like www.mantarayplan.localhost:
```shell
minikube tunnel
```

When you stop development for the day:
```shell
minikube stop
```

### Deploy to minikube
Install the cluster for the first time:
```shell
helm install -f environments/values-development.yaml manta-ray-plan-cloud .
```

Upgrade the cluster when you have a new version:
```shell
helm upgrade -f environments/values-development.yaml manta-ray-plan-cloud .
```

Remove the cluster from Kubernetes:
```shell
helm uninstall manta-ray-plan-cloud
```
## Install on production
### Step 1: Install MicroK8s
Execute this install command:
```shell
sudo snap install microk8s --classic
sudo microk8s status --wait-ready
```

### Step 2: Enable services
Required:
```shell
sudo microk8s enable dns
sudo microk8s enable helm
sudo microk8s enable dashboard
sudo microk8s enable ingress
sudo microk8s enable cert-manager
```

Optional:
```shell
sudo microk8s enable metrics-server
sudo microk8s enable prometheus
```

## Usage on production
### Step 1 - Download source code
```shell
git clone https://github.com/MadWorldNL/MantaRayPlan
```

### Step 2 - Install Cluster
Navigate to the folder `deployment/MantaRayPlanCloud` and execute this command:
```shell
microk8s helm install -f environments/values-production.yaml manta-ray-plan-cloud .
```

See [Deploy to minikube](#deploy-to-minikube) for other helm reference

## Reference
[MicroK8s install guide](https://microk8s.io/)