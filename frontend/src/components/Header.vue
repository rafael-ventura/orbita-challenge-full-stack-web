<template>
  <v-app-bar color="white" elevation="1">
    <v-btn icon @click="emit('toggleSidebar')">
      <v-icon>mdi-menu</v-icon>
    </v-btn>
    
    <v-app-bar-title class="d-flex align-center">
      <v-icon class="me-3" color="primary">mdi-school</v-icon>
      A+ Educação
    </v-app-bar-title>
    
    <v-spacer></v-spacer>
    
    <v-btn 
      @click="logout" 
      icon="mdi-logout" 
      title="Sair"
      color="primary"
      variant="text"
    ></v-btn>
  </v-app-bar>
</template>

<script setup>
import { defineEmits } from "vue";
import { useRouter } from 'vue-router';
import { AuthService } from '@/services';

const emit = defineEmits(["toggleSidebar"]);
const router = useRouter();

const logout = () => {
  AuthService.logout();
  router.push('/auth/login');
};
</script>

<style scoped lang="scss">
@import '@/assets/styles/variables.scss';

.v-app-bar {
  border-bottom: 1px solid $grey-light;
}
</style> 