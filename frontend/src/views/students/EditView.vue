<template>
  <v-container>
    <v-row justify="center">
      <v-col cols="12" md="8" lg="6">
        <v-card class="elevation-4">
          <v-card-title class="text-h4 pa-6">
            <v-icon size="40" color="primary" class="me-3">mdi-pencil</v-icon>
            Editar Estudante
          </v-card-title>
          
          <v-card-text>
            <p class="text-center mb-4">Editando estudante ID: {{ $route.params.id }}</p>
            <p class="text-center mb-4">Funcionalidade em desenvolvimento...</p>
            <v-btn
              @click="$router.push('/students')"
              color="primary"
              variant="outlined"
              size="large"
              block
            >
              Voltar para Lista de Estudantes
            </v-btn>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup>
// Placeholder para edição de estudante
const props = defineProps(['id'])
</script> 