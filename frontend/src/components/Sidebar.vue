<template>
  <v-navigation-drawer
      v-if="isClient"
      :model-value="drawer"
      @update:model-value="emit('update:drawer', $event)"
      width="300"
      permanent
      location="left"
      class="sidebar-drawer"
  >
    <Profile/>

    <v-list-subheader class="subheader">Módulo Acadêmico</v-list-subheader>

    <v-list>
      <v-list-item 
        prepend-icon="mdi-account-group" 
        title="Alunos Cadastrados" 
        to="/students" 
        class="nav-item"
        :active="isActive('/students')"
      ></v-list-item>

      <v-list-item 
        prepend-icon="mdi-plus" 
        title="Cadastrar Aluno" 
        to="/students/register" 
        class="nav-item"
        :active="isActive('/students/register')"
      ></v-list-item>
    </v-list>
  </v-navigation-drawer>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useRoute } from 'vue-router'
import Profile from './Profile.vue'

const props = defineProps({
  drawer: Boolean,
});

const emit = defineEmits(["update:drawer"]);
const route = useRoute();

const isClient = ref(false);

onMounted(() => {
  isClient.value = true;
});

// Verificar se a rota está ativa
const isActive = (path) => {
  return route.path === path || route.path.startsWith(path + '/')
}
</script>

<style scoped lang="scss">
@import '@/assets/styles/variables.scss';

.sidebar-drawer {
  background-color: $grey-light;
  width: 300px;
  border-right: 1px solid #e0e0e0;
  box-shadow: 2px 0 8px rgba(0, 0, 0, 0.1);
}

.nav-item {
  color: #333;
  margin: 4px 8px;
  border-radius: 8px;
  transition: all 0.3s ease;

  &:hover {
    background-color: rgba($secondary-color, 0.9);
    color: white;
    transform: translateX(4px);
  }
  
  &.v-list-item--active {
    background-color: $primary-color;
    color: white;
    font-weight: bold;
  }
}

.subheader {
  padding: 16px 16px 8px 16px;
  font-weight: bold;
  color: $red-dark;
  text-transform: uppercase;
  font-size: 0.875rem;
  letter-spacing: 0.5px;
}

// Melhorar aparência do Profile
:deep(.sidebar-header) {
  border-bottom: 1px solid #e0e0e0;
  margin-bottom: 16px;
}
</style> 