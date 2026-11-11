import type { Plugin } from "@opencode-ai/plugin"

export const IntegrationTestsOrchestratorPlugin: Plugin = async ({ client }) => {
  return {
    event: async ({ event }) => {
      if (event.type !== "command.executed") return
      if (event.properties.name !== "integration-tests") return

      const { sessionID, arguments: args } = event.properties
      await client.app.log({
        body: {
          service: "integration-tests",
          level: "info",
          message: `Comando /integration-tests ejecutado en sesión ${sessionID}`,
          extra: { arguments: args },
        },
      })
    },
  }
}