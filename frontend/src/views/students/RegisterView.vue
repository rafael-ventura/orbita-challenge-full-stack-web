<template>
  <div class="register-container">
    <v-snackbar v-model="snackbar.show" :color="snackbar.color" timeout="4000">
      {{ snackbar.message }}
    </v-snackbar>

    <v-card class="form-card elevation-4">
      <v-card-title class="form-title">
        <v-icon size="40" color="primary" class="me-3">mdi-account-plus</v-icon>
        Cadastro de Aluno
      </v-card-title>
      
      <v-card-text>
        <v-form ref="form" v-model="valid">
          <v-text-field 
            label="Nome" 
            v-model="student.name" 
            :rules="[rules.required, rules.minLength(2)]" 
            required
            color="primary"
            density="compact"
            class="mb-4"
          />
          
          <v-text-field 
            label="E-mail" 
            v-model="student.email" 
            :rules="[rules.required, rules.email]" 
            required
            color="primary"
            density="compact"
            class="mb-4"
          />
          
          <v-text-field
            label="RA (Registro Acadêmico)"
            v-model="student.ra"
            :rules="[rules.required, rules.numeric]"
            required
            color="primary"
            density="compact"
            class="mb-4"
            @input="student.ra = formatRA(student.ra)"
            maxlength="10"
          />
          
          <v-text-field
            label="CPF"
            v-model="student.cpf"
            :rules="[rules.required, rules.cpf]"
            required
            color="primary"
            density="compact"
            class="mb-4"
            maxlength="14"
            @input="student.cpf = formatCPF(student.cpf)"
          />
          

        </v-form>
      </v-card-text>
      
      <v-card-actions class="d-flex justify-space-between pa-4">
        <v-btn 
          color="grey" 
          variant="outlined" 
          @click="cancel"
          size="large"
        >
          Cancelar
        </v-btn>
        <v-btn 
          color="primary" 
          variant="flat" 
          :disabled="!valid || loading" 
          :loading="loading"
          class="btn-save" 
          @click="saveStudent"
          size="large"
        >
          Salvar
        </v-btn>
      </v-card-actions>
    </v-card>
  </div>
</template>

<script setup>
import { ref, reactive } from 'vue'
import { useRouter } from 'vue-router'
import { StudentService, NotificationService } from '@/services'
import { Student } from '@/models'
import { validationRules } from '@/utils/validationRules'
import { formatCPF, formatRA } from '@/utils/formatHelper'
import { AuthService } from '@/services'

const router = useRouter()
const form = ref(null)
const valid = ref(false)
const loading = ref(false)

const student = ref(new Student())

const snackbar = reactive({
  show: false,
  message: '',
  color: 'success'
})

const rules = validationRules

const saveStudent = async () => {
  if (!form.value?.validate()) return

  // Verificar se o usuário está autenticado
  const token = AuthService.getToken()
  console.log('🔑 Token no cadastro:', token ? 'Presente' : 'Ausente')
  console.log('👤 Usuário autenticado:', AuthService.isAuthenticated())
  
  if (!AuthService.isAuthenticated()) {
    snackbar.show = true
    snackbar.message = 'Você precisa estar logado para cadastrar estudantes'
    snackbar.color = 'error'
    router.push('/auth/login')
    return
  }

  loading.value = true
  
  try {
    // Formatar dados para o formato esperado pelo backend
    const studentData = {
      Name: student.value.name,
      Email: student.value.email,
      CPF: student.value.cpf.replace(/\D/g, ""), // Remove formatação
      RA: student.value.ra
    }

    console.log('📤 Enviando dados:', studentData)
    await StudentService.createStudent(studentData)
    
    snackbar.show = true
    snackbar.message = 'Aluno cadastrado com sucesso!'
    snackbar.color = 'success'
    
    // Limpar formulário
    student.value = new Student()
    form.value?.resetValidation()
    
    // Redirecionar após um breve delay
    setTimeout(() => {
      router.push('/students')
    }, 1500)
    
  } catch (error) {
    console.error('Erro ao cadastrar estudante:', error)
    
    let errorMessage = 'Erro ao cadastrar estudante'
    
    if (error.response?.data?.errors && Array.isArray(error.response.data.errors) && error.response.data.errors.length > 0) {
      errorMessage = error.response.data.errors.join(' | ')
    } else if (error.response?.data?.message) {
      errorMessage = error.response.data.message
    } else if (error.message) {
      errorMessage = error.message
    }
    
    snackbar.show = true
    snackbar.message = errorMessage
    snackbar.color = 'error'
  } finally {
    loading.value = false
  }
}

const cancel = () => {
  router.push('/students')
}
</script>

<style scoped lang="scss">
@import '@/assets/styles/variables.scss';

.register-container {
  padding: 20px;
}

.form-card {
  max-width: 800px;
  margin: 0 auto;
  border-radius: 12px;
}

.form-title {
  font-size: 1.5rem;
  font-weight: bold;
  text-align: center;
  color: $primary-color;
  padding: 24px 24px 0 24px;
}

.btn-save {
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