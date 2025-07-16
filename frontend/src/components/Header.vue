<template>
  <v-app-bar color="white" elevation="1" class="header-bar">
    <v-btn icon @click="emit('toggleSidebar')" class="menu-btn">
      <v-icon>mdi-menu</v-icon>
    </v-btn>
    
    <v-app-bar-title class="app-title">
      <v-icon size="24" color="primary" class="me-2">mdi-school</v-icon>
      A+ Educação
    </v-app-bar-title>
  </v-app-bar>
</template>

<script setup>
import { defineEmits } from "vue";

const emit = defineEmits(["toggleSidebar"]);
</script>

<style scoped lang="scss">
@import '@/assets/styles/variables.scss';

.header-bar {
  border-bottom: 1px solid #e0e0e0;
  left: 0 !important;
  width: 100% !important;
  --v-layout-left: 0px !important;
  --v-layout-right: 0px !important;
}

.menu-btn {
  margin-right: 8px;
  
  &:hover {
    background-color: rgba($primary-color, 0.1);
  }
}

.app-title {
  font-weight: bold;
  color: $primary-color;
  display: flex;
  align-items: center;
}

// Garantir que o header se ajuste ao layout
.v-app-bar {
  position: sticky !important;
  top: 0 !important;
  z-index: 1000 !important;
}
</style> 