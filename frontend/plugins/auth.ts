import { defineNuxtPlugin, useNuxtApp } from '#app';
import type { $Fetch } from 'ofetch';

export default defineNuxtPlugin(() => {
  if (process.client) {
    const nuxtApp = useNuxtApp();
    const originalFetch = nuxtApp.$fetch as $Fetch;
    nuxtApp.$fetch = async (request: any, options: any = {}) => {
      const token = localStorage.getItem('token');
      if (token) {
        options.headers = options.headers || {};
        (options.headers as Record<string, string>)["Authorization"] = `Bearer ${token}`;
      }
      return originalFetch(request, options);
    };
  }
}); 