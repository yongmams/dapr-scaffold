# Dapr Playground Project
> **warning**  Please do not use this project in the production environment, otherwise the operation and maintenance personnel will "bless" you all the time soon.  

## Reference
> **warning**  The following collation is for reference only

| Name | Version | License | Site |  
| --- | --- | --- |  --- |  
| Dapr chart | 1.11.2 | [Apache-2.0](https://github.com/dapr/helm-charts/blob/master/LICENSE) | https://dapr.github.io/helm-charts |  
| Dapr-dashboard chart | 0.13.0 | [Apache-2.0](https://github.com/dapr/helm-charts/blob/master/LICENSE) | https://dapr.github.io/helm-charts |  
| ECK-operator chart | 2.9.0 | [Elastic License 2.0](https://github.com/elastic/cloud-on-k8s/blob/main/LICENSE.txt) | https://helm.elastic.co |  
| Elasticsearch | 7.16.2 | [Elastic License 2.0](https://github.com/elastic/cloud-on-k8s/blob/main/LICENSE.txt)  |   | 
| Kibana | 7.16.2 | [Elastic License 2.0](https://github.com/elastic/cloud-on-k8s/blob/main/LICENSE.txt) |  | 
| Redis chart | 17.14.6 | [APACHE-2.0](https://github.com/bitnami/charts/blob/main/LICENSE.md) | https://charts.bitnami.com/bitnami |  
| Redis | 7.0.12-debian-11-r19 | [License](https://redis.io/docs/about/license/) | https://hub.docker.com/r/bitnami/redis |
| Redis Insight | 1.14.0 | [RedisInsight License Terms](https://redis.com/legal/redis-insight-license-terms/) | https://hub.docker.com/r/redislabs/redisinsight |  
| Prometheus chart | 15.8.0 | [Apache License 2.0](https://github.com/prometheus-community/helm-charts/blob/main/LICENSE) | https://prometheus-community.github.io/helm-charts |  
| Prometheus | v2.34.0 | [Apache License 2.0](https://github.com/prometheus/prometheus/blob/main/LICENSE) | https://quay.io/repository/prometheus/prometheus |  
| Apisix chart | 2.0.7 | [APACHE-2.0](https://github.com/bitnami/charts/blob/main/LICENSE.md) | https://charts.bitnami.com/bitnami |  
| Apisix | 3.4.1-debian-11-r5 | [Apache License 2.0](https://github.com/apache/apisix/blob/master/LICENSE) | https://hub.docker.com/r/bitnami/apisix | 
| MinIO chart | 11.6.8 | [APACHE-2.0](https://github.com/bitnami/charts/blob/main/LICENSE.md) | https://charts.bitnami.com/bitnami |
| MinIO | 2022.6.3-debian-11-r0 | [License](https://min.io/pricing) |  https://hub.docker.com/r/bitnami/minio |
| MSSQL | 2019-latest | [Licensing](https://www.microsoft.com/en-us/licensing/product-licensing/sql-server?activetab=sql-server-pivot%3aprimaryr2) | https://mcr.microsoft.com/en-us/product/mssql/server/about |  
| Sqlpad | 7.1.0 | [MIT License](https://github.com/sqlpad/sqlpad/blob/master/LICENSE.md) | https://github.com/sqlpad/sqlpad |  
| Zipkin | 2.24 |  [Apache License 2.0](https://github.com/openzipkin/zipkin/blob/master/LICENSE) | https://github.com/openzipkin/zipkin | 
| CSI Driver Smb | v1.11.0 | [Apache License 2.0](https://github.com/kubernetes-csi/csi-driver-smb/blob/master/LICENSE) | https://raw.githubusercontent.com/kubernetes-csi/csi-driver-smb/master/charts |  


# pre-preparation

## When using local docker desktop kubernetes, can choose install SMB CSI 

```
helm repo add csi-driver-smb https://raw.githubusercontent.com/kubernetes-csi/csi-driver-smb/master/charts
helm install csi-driver-smb csi-driver-smb/csi-driver-smb --namespace kube-system --version v1.11.0
```
or
```
helm install csi-driver-smb .\preparation\csi\csi-driver-smb-v1.11.0.tgz --namespace kube-system --create-namespace
```

Create SMB Storage Class (dapr-standard-sc)
```
kubectl create secret generic smbcreds --from-literal=username="k8suser" --from-literal=password="********" -n kube-system
kubectl apply -f .\preparation\dapr-standard.sc.yaml
```
## Install
```
helm install dapr .\infra\ --namespace dapr-playground --create-namespace  
```

## Update
```
helm update dapr .\infra\ --namespace dapr-playground --create-namespace  
```

## other
```
helm repo add microsoft http://mirror.azure.cn/kubernetes/charts
```