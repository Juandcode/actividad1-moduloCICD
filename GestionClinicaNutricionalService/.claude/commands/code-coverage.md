---
description: Genera el reporte de cobertura completo (unitarios + integración) y resume el resultado con el agente orquestador code-coverage.
argument-hint: ""
context: fork
agent: code-coverage
background: false
allowed-tools: Read, Grep, Glob, Edit, Write, Bash, PowerShell
---

Generá el reporte de cobertura completo del repo (unitarios `Tests/` + integración `IntegrationTests/`) siguiendo el flujo del skill `code-coverage`, y resumí el resultado.

$ARGUMENTS