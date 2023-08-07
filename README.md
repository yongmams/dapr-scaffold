# pre-preparation

## When using local docker desktop kubernetes, can choose install SMB CSI 

```
helm repo add csi-driver-smb https://raw.githubusercontent.com/kubernetes-csi/csi-driver-smb/master/charts
helm install csi-driver-smb csi-driver-smb/csi-driver-smb --namespace kube-system --version v1.11.0
```
or
```
helm install csi-driver-smb .\mirror\csi-driver-smb-v1.11.0.tgz --namespace kube-system --create-namespace
```

Create SMB Storage Class (dapr-standard-sc)
```
kubectl create secret generic smbcreds --from-literal=username="k8suser" --from-literal=password="********" -n kube-system
kubectl apply -f .\csi\dapr-standard.sc.yaml
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