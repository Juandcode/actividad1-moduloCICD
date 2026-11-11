---
description: Crea un test de integración nuevo y lo verifica con el agente orquestador integration-tests (skills create-integration-tests + integration-tests).
argument-hint: "<endpoint o controller a cubrir>"
context: fork
agent: integration-tests
background: false
allowed-tools: Read, Grep, Glob, Edit, Write, Bash, PowerShell
---

Creá un test de integración para: $ARGUMENTS

Seguí el flujo completo del agente orquestador: primero creá el test con el skill `create-integration-tests` y luego verificá que pase con el skill `integration-tests`.