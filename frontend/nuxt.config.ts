import {defineNuxtConfig} from 'nuxt/config';

export default defineNuxtConfig({
  devtools: { enabled: true },
  css: ["vuetify/styles", "@mdi/font/css/materialdesignicons.css", "@/styles/app.scss"],

  build: {
    transpile: ["vuetify"],
  },

  runtimeConfig: {
    public: {
      apiBaseUrl: process.env.NUXT_PUBLIC_API_BASE_URL || "http://localhost:7001/api",
    },
  },

  compatibilityDate: "2025-02-11",
});
