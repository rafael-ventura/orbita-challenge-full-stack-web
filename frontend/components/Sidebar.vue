<template>
  <v-navigation-drawer
      v-if="isClient"
      :model-value="drawer"
      @update:model-value="emit('update:drawer', $event)"
      app
      width="300"
      permanent
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
@use "@/styles/app.scss" as app;

.v-navigation-drawer {
  background-color: app.$grey-light;
  width: 30%;
}

.nav-item {
  color: #333;

  &:hover {
    background-color: rgba(app.$secondary-color, 0.9);
    color: white;
  }
}

.subheader {
  padding-left: 10px;
  font-weight: bold;
  color: app.$red-dark;
  text-transform: uppercase;
}
</style>
