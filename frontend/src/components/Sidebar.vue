<template>
  <v-navigation-drawer
      v-if="isClient"
      :model-value="drawer"
      @update:model-value="emit('update:drawer', $event)"
      width="300"
      permanent
      location="left"
  >
    <Profile/>

    <v-list-subheader class="subheader">Módulo Acadêmico</v-list-subheader>

    <v-list>
      <v-list-item prepend-icon="mdi-account-group" title="Alunos" to="/students" class="nav-item"></v-list-item>

      <v-list-item prepend-icon="mdi-plus" title="Cadastro" to="/students/register" class="nav-item"></v-list-item>
    </v-list>
  </v-navigation-drawer>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import Profile from './Profile.vue'

defineProps({
  drawer: Boolean,
});

const emit = defineEmits(["update:drawer"]);

const isClient = ref(false);

onMounted(() => {
  isClient.value = true;
});
</script>

<style scoped lang="scss">
@import '@/assets/styles/variables.scss';

.v-navigation-drawer {
  background-color: $grey-light;
  width: 300px;
}

.nav-item {
  color: #333;

  &:hover {
    background-color: rgba($secondary-color, 0.9);
    color: white;
  }
}

.subheader {
  padding-left: 10px;
  font-weight: bold;
  color: $red-dark;
  text-transform: uppercase;
}
</style> 