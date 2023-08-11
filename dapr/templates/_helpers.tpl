{{/*
Return  the proper Storage Class (adapted to the dapr configuration format)
{{ include "dapr.storage.class" ( dict "persistence" .Values.path.to.the.persistentVolume "global" $) }}
*/}}
{{- define "dapr.storage.class" -}}

{{- $storageClass := .persistentVolume.storageClass -}}
{{- if .global -}}
    {{- if .global.storageClass -}}
        {{- $storageClass = .global.storageClass -}}
    {{- end -}}
{{- end -}}

{{- if $storageClass -}}
  {{- if (eq "-" $storageClass) -}}
      {{- printf "\"\"" -}}
  {{- else }}
      {{- printf "%s" $storageClass -}}
  {{- end -}}
{{- end -}}

{{- end -}}