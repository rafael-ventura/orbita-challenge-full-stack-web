<template>
  <v-app>
    <v-main>
      <v-container fluid fill-height>
        <v-row justify="center" align="center">
          <v-col cols="12" sm="8" md="6" lg="4" class="text-center">
            <v-card class="elevation-12 pa-8">
              <v-icon size="100" color="error" class="mb-4">mdi-alert-circle</v-icon>
              <h1 class="text-h2 mb-4">404</h1>
              <h2 class="text-h4 mb-4">Página não encontrada</h2>
              <p class="mb-6">A página que você está procurando não existe.</p>
              
              <v-btn
                @click="$router.push('/')"
                color="primary"
                size="large"
                prepend-icon="mdi-home"
              >
                Voltar ao Início
              </v-btn>
            </v-card>
          </v-col>
        </v-row>
      </v-container>
    </v-main>
  </v-app>
</template>

<script setup>
// Página 404
</script> 