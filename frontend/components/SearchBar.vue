<template>
  <v-text-field
      :model-value="searchQuery"
      @update:model-value="updateSearch"
      label="Pesquise por RA, Nome, E-mail ou CPF"
      variant="outlined"
      dense
      hide-details
      append-inner-icon="mdi-magnify"
  />
</template>

<script setup>
import {defineEmits, defineProps} from "vue";

defineProps({
  searchQuery: String,
});

const emit = defineEmits(["update:searchQuery", "addStudent"]);

const updateSearch = (value) => {
  emit("update:searchQuery", value);
};
</script>

<style scoped lang="scss">
.search-bar {
  margin-bottom: 0.2rem;
}

.search-input {
  height: 2.5rem;
}

.search-btn {
  font-size: 1rem;
}
</style>
