import { defineConfig } from "cypress";

export default defineConfig({
  e2e: {
    baseUrl: "http://localhost:3000",
    defaultCommandTimeout: 30000,
    pageLoadTimeout: 30000,
    requestTimeout: 60000,
    responseTimeout: 60000,
    viewportWidth: 1920,
    viewportHeight: 1080,
    screenshotOnRunFailure: true,
    setupNodeEvents(on, config) {}
  },
});
