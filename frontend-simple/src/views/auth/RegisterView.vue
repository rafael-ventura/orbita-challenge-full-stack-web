<template>
  <div class="register-bg register-center">
    <v-snackbar v-model="snackbar.show" :color="snackbar.color" timeout="4000">
      {{ snackbar.message }}
    </v-snackbar>

    <v-card class="form-card elevation-12">
      <v-card-title class="form-title pb-0 pt-2">
        <h2>Cadastro</h2>
      </v-card-title>
      <v-card-text class="pa-0">
        <v-form ref="form" v-model="valid">
          <v-text-field label="Nome" v-model="user.name" :rules="[rules.required, rules.minLength]" required color="primary" density="compact" class="mt-4 mb-4"/>
          <v-text-field label="E-mail" v-model="user.email" :rules="[rules.required, rules.email]" required color="primary" density="compact" class="mt-4 mb-4"/>
          <v-text-field
              label="Senha"
              v-model="user.password"
              :rules="[rules.required, rules.password]"
              required
              type="password"
              color="primary"
              density="compact"
              class="mt-4 mb-4"
          />
        </v-form>
      </v-card-text>
      <v-card-actions class="d-flex justify-space-between pa-0 pt-4">
        <v-btn color="grey" variant="outlined" @click="goToLogin"> Já tenho uma conta </v-btn>
        <v-btn color="primary" variant="flat" :disabled="!valid" class="btn-register" @click="registerUser">
          Cadastrar
        </v-btn>
      </v-card-actions>
    </v-card>
  </div>
</template>

<script setup>
import { ref, reactive } from 'vue'
import { useRouter } from 'vue-router'
import { AuthService } from '@/services'

const router = useRouter()

const snackbar = reactive({
  show: false,
  message: '',
  color: 'success'
})

const form = ref(null)
const valid = ref(false)
const user = ref({
  name: '',
  email: '',
  password: ''
})

const rules = {
  required: (value) => !!value || 'Campo obrigatório',
  email: (value) => {
    const pattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/
    return pattern.test(value) || 'E-mail inválido'
  },
  minLength: (value) => value.length >= 2 || 'O nome deve ter pelo menos 2 caracteres.',
  password: (value) => value.length >= 6 || 'A senha deve ter pelo menos 6 caracteres.'
}

const registerUser = async () => {
  if (!form.value?.validate()) return;
  try {
    await AuthService.register(user.value)
    snackbar.show = true
    snackbar.message = 'Cadastro realizado com sucesso!'
    snackbar.color = 'success'
    await router.push({
      path: '/auth/login',
      query: {
        email: user.value.email,
        password: user.value.password
      }
    })
  } catch (error) {
    snackbar.show = true
    snackbar.message = 'Erro ao registrar usuário. Tente novamente.'
    snackbar.color = 'error'
  }
}

const goToLogin = () => {
  router.push('/auth/login')
}
</script>

<style scoped lang="scss">
@import '@/assets/styles/variables.scss';

.register-bg {
  min-height: 100vh;
  background: linear-gradient(135deg, $primary-color 60%, $secondary-color 100%);
}

.register-center {
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
  margin-bottom: 0;
}

.btn-register {
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