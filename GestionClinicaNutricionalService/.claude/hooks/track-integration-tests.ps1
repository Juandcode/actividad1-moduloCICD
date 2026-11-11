$EventInput = [Console]::In.ReadToEnd()
try {
  $Event = $EventInput | ConvertFrom-Json
} catch {
  exit 0
}

if ($Event.hook_event_name -ne "UserPromptExpansion") { exit 0 }
if ($Event.command_name -ne "integration-tests") { exit 0 }

$LogDir = Join-Path $env:USERPROFILE ".claude\logs"
New-Item -ItemType Directory -Path $LogDir -Force | Out-Null

$entry = [ordered]@{
  timestamp      = (Get-Date).ToString("o")
  session_id     = $Event.session_id
  command_name   = $Event.command_name
  command_args   = $Event.command_args
  expansion_type = $Event.expansion_type
  cwd            = $Event.cwd
}

$entry | ConvertTo-Json -Compress | Add-Content -Path (Join-Path $LogDir "integration-tests-hook.log")

exit 0