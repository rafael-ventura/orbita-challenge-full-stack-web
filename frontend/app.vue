<template>
  <v-app>
    <Sidebar v-if="isAuthenticated" v-model:drawer="drawer"/>
    <v-main :class="{ 'main-expanded': drawer, 'main-collapsed': !drawer }">
      <Header v-if="isAuthenticated" @toggleSidebar="toggleSidebar"/>
      <v-container>
        <NuxtPage />
      </v-container>
    </v-main>
  </v-app>
</template>

<script setup>
import {onMounted, ref, watchEffect} from "vue";
import {navigateTo, useRoute, useState} from "#app";
import Sidebar from "@/components/Sidebar.vue";
import Header from "@/components/Header.vue";

const drawer = ref(true);
const toggleSidebar = () => {
  drawer.value = !drawer.value;
};

const isAuthenticated = useState("isAuthenticated", () => false);
const route = useRoute();

onMounted(() => {
  if (process.client) {
    const token = localStorage.getItem("token");
    isAuthenticated.value = !!token;
  }
});

const checkAuth = () => {
  if (process.client) {
    const token = localStorage.getItem("token");
    isAuthenticated.value = !!token;

    const publicRoutes = ["/auth/login", "/auth/register"];

    if (!token && !publicRoutes.includes(route.path)) {
      navigateTo("/auth/login");
    }
  }
};

watchEffect(checkAuth);
</script>
