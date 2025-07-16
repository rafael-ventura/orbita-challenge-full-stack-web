<template>
  <v-text-field
      :model-value="searchQuery"
      @update:model-value="updateSearch"
      label="Pesquise por Nome ou E-mail"
      variant="outlined"
      dense
      hide-details
      append-inner-icon="mdi-magnify"
      clearable
      placeholder="Digite nome ou email do aluno..."
      class="search-field"
      :loading="loading"
  />
</template>

<script setup>
import { defineEmits, defineProps } from "vue";

defineProps({
  searchQuery: String,
  loading: {
    type: Boolean,
    default: false
  }
});

const emit = defineEmits(["update:searchQuery", "addStudent"]);

const updateSearch = (value) => {
  emit("update:searchQuery", value);
};
</script>

<style scoped lang="scss">
@import '@/assets/styles/variables.scss';

.search-field {
  .v-field__input {
    padding: 12px 16px;
  }
  
  .v-field__outline {
    border-color: $primary-color;
  }
  
  &:focus-within {
    .v-field__outline {
      border-color: $red-dark;
    }
  }
  
  // Melhorar aparência do ícone de busca
  .v-field__append-inner {
    color: $primary-color;
  }
  
  // Melhorar placeholder
  .v-field__input::placeholder {
    color: #999;
    font-style: italic;
  }
}

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