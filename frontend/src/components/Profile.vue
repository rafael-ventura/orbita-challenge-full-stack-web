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
      <v-img :src="userAvatar"></v-img>
    </v-avatar>

    <div class="text-white user-name">{{ userName }}</div>

    <div class="text-white user-email">{{ userEmail }}</div>
  </v-sheet>

  <v-divider></v-divider>
</template>

<script setup>
import { useRouter } from 'vue-router'
import { onMounted, ref } from 'vue'
import { AuthService } from '@/services'

const router = useRouter()
const userName = ref('Usuário')
const userEmail = ref('usuario@email.com')
const userAvatar = ref('https://randomuser.me/api/portraits/lego/6.jpg')

onMounted(() => {
  // Carregar dados do usuário do localStorage
  const storedName = localStorage.getItem('userName')
  const storedEmail = localStorage.getItem('userEmail')
  
  if (storedName) userName.value = storedName
  if (storedEmail) userEmail.value = storedEmail
  
  // Gerar avatar aleatório baseado no email
  if (storedEmail) {
    const emailHash = storedEmail.split('@')[0]
    const avatarId = Math.abs(emailHash.split('').reduce((a, b) => {
      a = ((a << 5) - a) + b.charCodeAt(0)
      return a & a
    }, 0)) % 99 + 1
    
    userAvatar.value = `https://randomuser.me/api/portraits/lego/${avatarId}.jpg`
  }
})

const logout = () => {
  console.log('🔴 Usuário deslogado!')
  
  // Limpar dados de autenticação
  AuthService.logout()
  
  // Redirecionar para login
  router.push('/auth/login')
}
</script>

<style scoped lang="scss">
@import '@/assets/styles/variables.scss';

.sidebar-header {
  width: 100%;
  background-color: $red-light;
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

.user-name {
  font-weight: bold;
  margin-bottom: 4px;
}

.user-email {
  font-size: 1rem;
  opacity: 0.9;
}
</style> 