<template>
  <div class="students-container">
    <div class="students-header">
      <h1 class="mb-4">Consulta de Alunos</h1>
      <p class="text-subtitle-1 text-grey-darken-1 mb-4">
        Gerencie os dados dos estudantes do sistema
      </p>
    </div>
    
    <!-- Header com busca e botão adicionar -->
    <v-card class="pa-4 mb-4" elevation="2">
      <v-row align="center">
        <v-col cols="9">
          <SearchBar
            :searchQuery="search"
            :loading="loading"
            @update:searchQuery="search = $event"
          />
        </v-col>
        <v-col cols="3" class="d-flex justify-end">
          <v-btn 
            @click="openAddStudentPage" 
            color="primary" 
            prepend-icon="mdi-plus"
            size="large"
          >
            Cadastrar Aluno
          </v-btn>
        </v-col>
      </v-row>
    </v-card>

    <!-- Loading -->
    <v-progress-linear v-if="loading" indeterminate color="primary" class="mb-4"></v-progress-linear>

    <!-- Tabela de estudantes -->
    <StudentsTable
      :students="filteredStudents"
      :headers="headers"
      :loading="loading"
      :itemsPerPage="itemsPerPage"
      @editStudent="openEditStudentModal"
      @deleteStudent="confirmDeleteStudent"
    />

    <!-- Estado vazio quando não há estudantes -->
    <div v-if="!loading && students.length === 0" class="empty-state">
      <v-card class="empty-card" elevation="2">
        <v-card-text class="text-center pa-8">
          <v-icon size="80" color="grey-lighten-1" class="mb-4">mdi-account-group-outline</v-icon>
          <h3 class="text-h5 mb-2 text-grey-darken-1">Nenhum aluno cadastrado</h3>
          <p class="text-body-1 text-grey-darken-2 mb-6">
            Comece cadastrando o primeiro aluno do sistema
          </p>
          <v-btn 
            @click="openAddStudentPage" 
            color="primary" 
            size="large"
            prepend-icon="mdi-plus"
            variant="flat"
          >
            Cadastrar Primeiro Aluno
          </v-btn>
        </v-card-text>
      </v-card>
    </div>

    <!-- Dialog de confirmação de exclusão -->
    <v-dialog v-model="deleteDialog" max-width="500px">
      <v-card>
        <v-card-title class="d-flex align-center">
          <v-icon color="error" class="me-2">mdi-alert-circle</v-icon>
          Confirmar Exclusão
        </v-card-title>
        <v-card-text>
          <p class="text-body-1 mb-2">
            Tem certeza que deseja excluir este aluno?
          </p>
          <v-alert type="warning" variant="tonal" class="mt-3">
            <v-icon start>mdi-information</v-icon>
            Esta ação não pode ser desfeita.
          </v-alert>
        </v-card-text>
        <v-card-actions class="pa-4">
          <v-spacer></v-spacer>
          <v-btn 
            color="grey" 
            variant="outlined" 
            @click="deleteDialog = false"
            size="large"
          >
            Cancelar
          </v-btn>
          <v-btn 
            color="error" 
            variant="flat" 
            @click="deleteStudent"
            size="large"
            :loading="deleting"
          >
            <v-icon start>mdi-delete</v-icon>
            Excluir
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <!-- Snackbar para notificações -->
    <v-snackbar 
      v-model="snackbar.show" 
      :color="snackbar.color" 
      :timeout="snackbar.timeout"
    >
      {{ snackbar.message }}
    </v-snackbar>
  </div>
</template>

<script setup>
import { ref, computed, onMounted, reactive } from 'vue'
import { useRouter } from 'vue-router'
import { StudentService, NotificationService } from '@/services'
import SearchBar from '@/components/SearchBar.vue'
import StudentsTable from '@/components/StudentsTable.vue'

const router = useRouter()

const snackbar = reactive({
  show: false,
  message: '',
  color: 'success',
  timeout: 4000
})
    
// Estado reativo
const students = ref([])
const loading = ref(false)
const deleting = ref(false)
const search = ref('')
const deleteDialog = ref(false)
const studentToDelete = ref(null)
const itemsPerPage = ref(10)

// Headers da tabela
const headers = [
  { title: 'RA', key: 'ra', sortable: true },
  { title: 'Nome', key: 'name', sortable: true },
  { title: 'E-mail', key: 'email', sortable: true },
  { title: 'CPF', key: 'cpf', sortable: true },
  { title: 'Ações', key: 'actions', sortable: false }
]

// Computed para filtrar estudantes
const filteredStudents = computed(() => {
  if (!search.value.trim()) return students.value
  
  const searchTerm = search.value.toLowerCase().trim()
  const searchNumbers = searchTerm.replace(/\D/g, '') // Remove tudo que não é número
  
  console.log('🔍 Buscando por:', searchTerm, '| Números:', searchNumbers)
  
  return students.value.filter(student => {
    console.log('📋 Verificando aluno:', student.name, '| RA:', student.ra, '| CPF:', student.cpf)
    
    // Busca por nome (case insensitive)
    if (student.name && student.name.toLowerCase().includes(searchTerm)) {
      console.log('✅ Encontrado por nome:', student.name)
      return true
    }
    
    // Busca por email (case insensitive)
    if (student.email && student.email.toLowerCase().includes(searchTerm)) {
      console.log('✅ Encontrado por email:', student.email)
      return true
    }
    
    // Busca por RA (exata ou parcial)
    if (student.ra) {
      const studentRa = student.ra.toString().toLowerCase()
      const studentRaNumbers = student.ra.toString().replace(/\D/g, '')
      
      if (studentRa.includes(searchTerm) || studentRaNumbers.includes(searchNumbers)) {
        console.log('✅ Encontrado por RA:', student.ra)
        return true
      }
    }
    
    // Busca por CPF (com ou sem formatação)
    if (student.cpf) {
      const studentCpfNumbers = student.cpf.replace(/\D/g, '')
      const searchCpfNumbers = searchTerm.replace(/\D/g, '')
      
      // Busca por CPF formatado (123.456.789-01)
      if (student.cpf.toLowerCase().includes(searchTerm)) {
        console.log('✅ Encontrado por CPF formatado:', student.cpf)
        return true
      }
      
      // Busca por CPF apenas números
      if (studentCpfNumbers.includes(searchCpfNumbers)) {
        console.log('✅ Encontrado por CPF números:', studentCpfNumbers)
        return true
      }
    }
    
    console.log('❌ Não encontrado:', student.name)
    return false
  })
})

// Métodos
const loadStudents = async () => {
  loading.value = true
  try {
    students.value = await StudentService.getAllStudents()
    console.log('Estudantes carregados:', students.value)
    
    // Se não há estudantes, não é um erro - apenas log informativo
    if (students.value.length === 0) {
      console.log('Nenhum estudante encontrado - lista vazia')
    }
  } catch (error) {
    console.error('Erro ao carregar estudantes:', error)
    snackbar.show = true
    snackbar.message = error.message || 'Erro ao carregar lista de estudantes'
    snackbar.color = 'error'
  } finally {
    loading.value = false
  }
}

const openAddStudentPage = () => {
  router.push('/students/register')
}

const openEditStudentModal = (student) => {
  router.push(`/students/edit/${student.id}`)
}

const confirmDeleteStudent = (studentId) => {
  console.log('🗑️ Tentando excluir estudante ID:', studentId)
  studentToDelete.value = studentId
  deleteDialog.value = true
}

const deleteStudent = async () => {
  if (!studentToDelete.value) {
    console.error('❌ ID do estudante não fornecido')
    return
  }
      
  deleting.value = true
  try {
    console.log('🗑️ Excluindo estudante ID:', studentToDelete.value)
    await StudentService.deleteStudent(studentToDelete.value.toString())
    await loadStudents() // Recarregar lista
    snackbar.show = true
    snackbar.message = 'Estudante excluído com sucesso!'
    snackbar.color = 'success'
  } catch (error) {
    console.error('Erro ao excluir estudante:', error)
    snackbar.show = true
    snackbar.message = error.message || 'Erro ao excluir estudante'
    snackbar.color = 'error'
  } finally {
    deleteDialog.value = false
    studentToDelete.value = null
    deleting.value = false
  }
}

// Lifecycle
onMounted(() => {
  loadStudents()
})
</script>

<style scoped lang="scss">
@import '@/assets/styles/variables.scss';

.students-container {
  padding: 24px;
  min-height: 100vh;
  background-color: #fafafa;
  width: 100%;
  max-width: 100%;
  box-sizing: border-box;
}

.students-header {
  margin-bottom: 24px;
  
  h1 {
    color: $primary-color;
    font-weight: bold;
    margin-bottom: 8px;
  }
  
  p {
    color: #666;
    margin-bottom: 0;
  }
}

// Melhorar aparência dos cards
.v-card {
  border-radius: 12px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
}

// Melhorar aparência dos botões
.v-btn {
  border-radius: 8px;
  font-weight: 600;
}

// Estado vazio
.empty-state {
  margin-top: 40px;
  
  .empty-card {
    max-width: 500px;
    margin: 0 auto;
    border-radius: 16px;
    background: linear-gradient(135deg, #f8f9fa 0%, #e9ecef 100%);
  }
}
</style> 