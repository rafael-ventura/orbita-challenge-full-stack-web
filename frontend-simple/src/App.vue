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
    
    <v-main>
      <Header 
        v-if="isAuthenticated" 
        @toggleSidebar="toggleSidebar"
      />
      
      <v-container fluid>
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
</style>
