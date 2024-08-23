# Setup Manta Ray Plan
This document describes how to setup the Manta Ray Plan in production.

## Kubernetes
1. Install kubernetes from our [kubernetes.md](kubernetes.md) guide.
2. Download the helm chart to your server.
2. Change every values in the `environments/values-production.yaml` file to match your requirements.
3. Install or upgrade the cluster

## Postgresql Install
Use this install command to install the postgresql chart:
```shell
helm repo add cnpg https://cloudnative-pg.github.io/charts
helm upgrade --install cnpg \
  --namespace cnpg-system \
  --create-namespace \
  cnpg/cloudnative-pg
```
As of version 0.11 the hostpath provisioner operator now requires [cert manager](https://github.com/cert-manager/cert-manager) to be installed before deploying the operator. This is because the operator now has a validating webhook that verifies the contents of the CR are valid.
Before deploying the operator, you need to install cert manager:

```bash
kubectl create -f https://github.com/cert-manager/cert-manager/releases/download/v1.7.1/cert-manager.yaml
```

Please ensure the cert manager is fully operational before installing the hostpath provisioner operator:  

```bash
kubectl wait --for=condition=Available -n cert-manager --timeout=120s --all deployments
```

Next, you need to create the hostpath provisioner namespace:

```bash
kubectl create -f https://raw.githubusercontent.com/kubevirt/hostpath-provisioner-operator/main/deploy/namespace.yaml
```

Followed by the webhook:
```bash
kubectl create -f https://raw.githubusercontent.com/kubevirt/hostpath-provisioner-operator/main/deploy/webhook.yaml -n hostpath-provisioner
```

And then you can create the operator:

```bash
kubectl create -f https://raw.githubusercontent.com/kubevirt/hostpath-provisioner-operator/main/deploy/operator.yaml -n hostpath-provisioner
```

## Values-production.yaml
*TODO*

## Deploy
Deploy now to kubernetes and wait for the pods to be ready.

## Seq
1. Visit your seq url 
2. Navigate to the general settings page
   1. Select the API Keys tab
   2. Enable Require authentication for all incoming events
   3. Create a new API key for every service that will be sending logs to Seq
   4. Copy the API key and save it in a secure location
3. Navigate to the retention policies page
   1. Add a retention policy to match your requirements. At the moment there is no policy in place.
4. Navigate to your account settings page
   1. Change your default password

## Reference
[Install Guide Cloud Native PG](https://cloudnative-pg.io/documentation/1.23/installation_upgrade/)\
[Install Hostpath Provisioner](https://github.com/kubevirt/hostpath-provisioner-operator)