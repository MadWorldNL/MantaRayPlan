# Setup Manta Ray Plan
This document describes how to setup the Manta Ray Plan in production.

## Kubernetes
1. Download the helm chart to your server.
2. Change every values in the `environments/values-production.yaml` file to match your requirements.
3. Install or upgrade the cluster

## Seq
1. Visit your seq url 
2. Navigate to the general settings page
   1. Select the API Keys tab
   2. Enable Require authentication for all incoming events
   3. Create a new API key for every service that will be sending logs to Seq
   4. Copy the API key and save it in a secure location
3. Navigate to your account settings page
   1. Change your default password

## Values-production.yaml
*TODO*