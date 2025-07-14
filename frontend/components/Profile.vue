<template>
  <v-sheet class="pa-4 sidebar-header d-flex flex-column align-center">
    <v-row class="w-100 align-center">
      <v-col cols="10" class="d-flex justify-center">
        <v-img src="https://maisaedu.com.br/hubfs/site-grupo-a/logo-mais-a-educacao.svg" class="logo" contain></v-img>
      </v-col>
      <v-col cols="2" class="d-flex justify-end">
        <v-btn icon="mdi-logout" variant="text" color="white" size="24" @click="logout"></v-btn>
      </v-col>
    </v-row>

    <v-avatar class="mb-4 avatar-overlay" size="64">
      <v-img src="https://randomuser.me/api/portraits/lego/6.jpg"></v-img>
    </v-avatar>

    <div class="text-white user-name">{{ userName }}</div>

    <div class="text-white user-email">{{ userEmail }}</div>
  </v-sheet>

  <v-divider></v-divider>
</template>

<script setup>
import {useRouter} from "vue-router";
import {onMounted, ref} from "vue";
import {useState} from "#app"; // Usa o estado global de autenticação

const router = useRouter();
const userName = ref("Usuário");
const userEmail = ref("usuario@email.com");

const isAuthenticated = useState("isAuthenticated", () => false);

onMounted(() => {
  userName.value = localStorage.getItem("userName") || "Usuário";
  userEmail.value = localStorage.getItem("userEmail") || "usuario@email.com";
});

const logout = () => {
  console.log("🔴 Usuário deslogado!");

  localStorage.removeItem("token");
  localStorage.removeItem("userId");
  localStorage.removeItem("userName");
  localStorage.removeItem("userEmail");

  isAuthenticated.value = false;

  router.push("/auth/login");
};
</script>


<style scoped lang="scss">
@use "@/styles/app.scss" as app;

.sidebar-header {
  width: 100%;
  background-color: app.$red-light;
  text-align: center;
  padding: 16px;
}

.logo {
  max-width: 30%;
  display: block;
  margin: 0 auto;
}

.avatar-overlay {
  border: 3px solid white;
  margin-top: 40px;
}

.user-name,
.user-email {
  font-size: 1.2rem;
  margin-top: 8px;
  max-width: 90%;
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
}
</style>
