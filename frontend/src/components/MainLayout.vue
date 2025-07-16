<template>
  <v-app>
    <Sidebar v-model:drawer="drawer" />
    <v-main :class="{ 'main-with-sidebar': drawer, 'main-without-sidebar': !drawer }">
      <Header @toggleSidebar="toggleSidebar" />
      <v-container fluid class="main-container">
        <router-view />
      </v-container>
    </v-main>
  </v-app>
</template>

<script setup>
import { ref } from 'vue'
import Sidebar from '@/components/Sidebar.vue'
import Header from '@/components/Header.vue'

const drawer = ref(true)
const toggleSidebar = () => {
  drawer.value = !drawer.value
}
</script>

<style scoped lang="scss">
@import '@/assets/styles/app.scss';

.v-main {
  padding-top: 0 !important;
  margin-top: 0 !important;
}

.header-bar {
  margin-top: 0 !important;
  top: 0 !important;
}

.main-with-sidebar {
  margin-left: 300px !important;
  transition: margin-left 0.3s ease;
  width: calc(100vw - 300px) !important;
  max-width: calc(100vw - 300px) !important;
  --v-layout-left: 0px !important;
  --v-layout-right: 0px !important;
}

.main-without-sidebar {
  margin-left: 0 !important;
  transition: margin-left 0.3s ease;
  width: 100vw !important;
  max-width: 100vw !important;
  --v-layout-left: 0px !important;
  --v-layout-right: 0px !important;
}

.main-container {
  padding: 0 !important;
  max-width: none;
  margin: 0;
  width: 100%;
}
</style> 