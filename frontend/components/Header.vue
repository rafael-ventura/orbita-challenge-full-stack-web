<template>
  <v-app-bar color="white" elevation="1">
    <v-btn icon @click="emit('toggleSidebar')">
      <v-icon>mdi-menu</v-icon>
    </v-btn>
    <v-app-bar-title>A+ Educação</v-app-bar-title>
  </v-app-bar>
</template>

<script setup>
import { defineEmits } from "vue";

const emit = defineEmits(["toggleSidebar"]);
</script>
