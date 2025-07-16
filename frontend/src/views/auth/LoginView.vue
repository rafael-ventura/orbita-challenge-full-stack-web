<template>
  <div class="login-bg login-center">
    <v-snackbar v-model="snackbar.show" :color="snackbar.color" timeout="4000">
      {{ snackbar.text }}
    </v-snackbar>

    <v-card class="form-card elevation-12">
      <v-card-title class="form-title no-padding">
        <h2 class="no-margin">Login</h2>
      </v-card-title>
      <v-card-text class="pa-0">
        <v-form ref="form" v-model="valid">
          <v-text-field label="E-mail" v-model="credentials.email" :rules="[rules.required, rules.email]" required color="primary" density="compact" class="mt-4 mb-4"/>
          <v-text-field
              label="Senha"
              v-model="credentials.password"
              :rules="[rules.required]"
              required
              type="password"
              color="primary"
              density="compact"
              class="mt-4 mb-4"
          />
        </v-form>
      </v-card-text>
      <v-card-actions class="d-flex justify-space-between pa-0 pt-4">
        <v-btn color="grey" variant="outlined" @click="register"> Registrar </v-btn>
        <v-btn color="primary" variant="flat" :disabled="!valid" class="btn-login" @click="loginUser">
          Entrar
        </v-btn>
      </v-card-actions>
    </v-card>
  </div>
</template>

<script setup>
import { ref, onMounted, reactive } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import { AuthService, NotificationService } from '@/services'

const router = useRouter()
const route = useRoute()

const snackbar = reactive({
  show: false,
  text: '',
  color: 'success'
})

const form = ref(null)
const valid = ref(false)
    
const credentials = ref({
  email: String(route.query.email || ""),
  password: String(route.query.password || ""),
})

const rules = {
  required: (value) => !!value || 'Campo obrigatório',
  email: (value) => {
    const pattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/
    return pattern.test(value) || 'E-mail inválido'
  }
}

const loginUser = async () => {
  if (!form.value?.validate()) return;

  try {
    const response = await AuthService.login(credentials.value)
    AuthService.setAuthData(response.token, response)
    snackbar.show = true
    snackbar.text = 'Login realizado com sucesso!'
    snackbar.color = 'success'
    router.push('/students')
  } catch (error) {
    snackbar.show = true
    snackbar.text = 'Erro ao fazer login. Verifique suas credenciais.'
    snackbar.color = 'error'
  }
}

const register = () => {
  router.push('/auth/register')
}

// Pré-preencher credenciais de teste
onMounted(() => {
  if (!credentials.value.email && !credentials.value.password) {
    credentials.value.email = 'admin@test.com'
    credentials.value.password = 'admin123'
  }
})
</script>

<style scoped lang="scss">
@import '@/assets/styles/variables.scss';

.login-bg {
  min-height: 100vh;
  background: linear-gradient(135deg, $primary-color 60%, $secondary-color 100%);
}

.login-center {
  display: flex;
  align-items: center;
  justify-content: center;
}

.form-card {
  width: 100%;
  max-width: 600px;
  min-width: 340px;
  display: flex;
  flex-direction: column;
  justify-content: flex-start;
  align-items: stretch;
  padding: 32px;
  border-radius: 18px;
  box-shadow: 0 8px 32px 0 rgba(44, 16, 30, 0.15);
  background: $white-text;
}

.form-title {
  font-size: 1.2rem;
  font-weight: bold;
  text-align: center;
  color: $primary-color;
  margin: 0 !important;
  padding: 0 !important;
  min-height: unset !important;
}

.no-padding {
  padding: 0 !important;
}

.no-margin {
  margin: 0 !important;
  padding: 0 !important;
}

.btn-login {
  background-color: $primary-color !important;
  color: $white-text !important;
  font-weight: bold;
  padding: 0 24px;
  transition: background 0.3s;
  border-radius: 8px;

  &:hover {
    background-color: $red-dark !important;
  }

  &:disabled {
    background-color: #cccccc !important;
    color: #888888 !important;
  }
}
</style> 