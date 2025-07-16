<template>
  <div class="edit-container">
    <v-snackbar v-model="snackbar.show" :color="snackbar.color" timeout="4000">
      {{ snackbar.message }}
    </v-snackbar>

    <v-card class="form-card elevation-4">
      <v-card-title class="form-title">
        <v-icon size="40" color="primary" class="me-3">mdi-pencil</v-icon>
        Editar Estudante
      </v-card-title>
      
      <v-card-text>
        <!-- Loading state -->
        <div v-if="loading" class="text-center pa-8">
          <v-progress-circular indeterminate color="primary" size="64"></v-progress-circular>
          <p class="mt-4">Carregando dados do estudante...</p>
        </div>

        <!-- Error state -->
        <div v-else-if="error" class="text-center pa-8">
          <v-icon size="64" color="error" class="mb-4">mdi-alert-circle</v-icon>
          <h3 class="text-h6 mb-2">Erro ao carregar estudante</h3>
          <p class="text-body-2 mb-4">{{ error }}</p>
          <v-btn color="primary" @click="loadStudent" variant="outlined">
            Tentar Novamente
          </v-btn>
        </div>

        <!-- Form -->
        <v-form v-else ref="form" v-model="valid">
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
            readonly
            disabled
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
            readonly
            disabled
          />

          <v-alert type="info" variant="tonal" class="mb-4">
            RA e CPF não podem ser alterados após o cadastro.
          </v-alert>
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
          :disabled="!valid || loading || updating" 
          :loading="updating"
          class="btn-save" 
          @click="updateStudent"
          size="large"
        >
          Salvar Alterações
        </v-btn>
      </v-card-actions>
    </v-card>
  </div>
</template>

<script setup>
import { ref, reactive, onMounted } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import { StudentService } from '@/services'
import { Student } from '@/models'
import { validationRules } from '@/utils/validationRules'
import { formatCPF, formatRA } from '@/utils/formatHelper'
import { AuthService } from '@/services'

const router = useRouter()
const route = useRoute()
const form = ref(null)
const valid = ref(false)
const loading = ref(false)
const updating = ref(false)
const error = ref('')

const student = ref(new Student())

const snackbar = reactive({
  show: false,
  message: '',
  color: 'success'
})

const rules = validationRules

const loadStudent = async () => {
  const studentId = route.params.id
  
  if (!studentId) {
    error.value = 'ID do estudante não fornecido'
    return
  }

  loading.value = true
  error.value = ''
  
  try {
    // Verificar autenticação
    if (!AuthService.isAuthenticated()) {
      snackbar.show = true
      snackbar.message = 'Você precisa estar logado para editar estudantes'
      snackbar.color = 'error'
      router.push('/auth/login')
      return
    }

    const studentData = await StudentService.getStudentById(studentId.toString())
    
    // Mapear dados do backend para o modelo local
    student.value = new Student(
      studentData.name,
      studentData.email,
      studentData.ra,
      studentData.cpf
    )
    student.value.id = studentData.id
    
    // Formatar CPF para exibição
    student.value.cpf = formatCPF(studentData.cpf)
    
  } catch (err) {
    console.error('Erro ao carregar estudante:', err)
    error.value = 'Erro ao carregar dados do estudante. Verifique se o ID é válido.'
  } finally {
    loading.value = false
  }
}

const updateStudent = async () => {
  if (!form.value?.validate()) return

  // Verificar se o usuário está autenticado
  if (!AuthService.isAuthenticated()) {
    snackbar.show = true
    snackbar.message = 'Você precisa estar logado para editar estudantes'
    snackbar.color = 'error'
    router.push('/auth/login')
    return
  }

  updating.value = true
  
  try {
    // Formatar dados para o formato esperado pelo backend
    const studentData = {
      Name: student.value.name,
      Email: student.value.email
      // CPF e RA não são enviados pois não podem ser alterados
    }

    console.log('📤 Enviando dados de atualização:', studentData)
    await StudentService.updateStudent(student.value.id, studentData)
    
    snackbar.show = true
    snackbar.message = 'Estudante atualizado com sucesso!'
    snackbar.color = 'success'
    
    // Redirecionar após um breve delay
    setTimeout(() => {
      router.push('/students')
    }, 1500)
    
  } catch (err) {
    console.error('Erro ao atualizar estudante:', err)
    
    let errorMessage = 'Erro ao atualizar estudante'
    
    if (err.response?.data?.message) {
      errorMessage = err.response.data.message
    } else if (err.message) {
      errorMessage = err.message
    }
    
    snackbar.show = true
    snackbar.message = errorMessage
    snackbar.color = 'error'
  } finally {
    updating.value = false
  }
}

const cancel = () => {
  router.push('/students')
}

// Carregar dados do estudante quando o componente for montado
onMounted(() => {
  loadStudent()
})
</script>

<style scoped lang="scss">
@import '@/assets/styles/variables.scss';

.edit-container {
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