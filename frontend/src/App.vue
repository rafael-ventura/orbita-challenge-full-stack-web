<script>
export default {
  name: 'App'
}
</script>

<template>
  <v-app>
    <Sidebar 
      v-if="isAuthenticated" 
      v-model:drawer="drawer"
    />
    
    <v-main :class="{ 
      'main-with-sidebar': isAuthenticated && drawer,
      'main-without-sidebar': isAuthenticated && !drawer
    }">
      <Header 
        v-if="isAuthenticated" 
        @toggleSidebar="toggleSidebar"
      />
      
      <v-container fluid class="main-container">
        <router-view />
      </v-container>
    </v-main>
  </v-app>
</template>

<script setup>
import { ref, computed } from 'vue'
import { AuthService } from '@/services'
import Sidebar from '@/components/Sidebar.vue'
import Header from '@/components/Header.vue'

const drawer = ref(true)

const isAuthenticated = computed(() => AuthService.isAuthenticated())

const toggleSidebar = () => {
  drawer.value = !drawer.value
}
</script>

<style scoped lang="scss">
@import '@/assets/styles/app.scss';

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

// Garantir que o conteúdo se ajuste corretamente
.v-main__wrap {
  min-height: 100vh;
  background-color: #fafafa;
  width: 100%;
}

// Melhorar transições
.v-navigation-drawer {
  transition: transform 0.3s ease;
}

// Garantir que o header fique fixo
.v-app-bar {
  position: sticky;
  top: 0;
  z-index: 1000;
}

// Garantir que o conteúdo não transborde
.v-main {
  overflow-x: hidden;
}

// Sobrescrever as propriedades CSS do Vuetify
.v-main {
  --v-layout-left: 0px !important;
  --v-layout-right: 0px !important;
}
</style>
