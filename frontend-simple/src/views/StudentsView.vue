<template>
  <div class="students-container">
    <h1 class="mb-4">Consulta de Alunos</h1>
    
    <!-- Header com busca e botão adicionar -->
    <v-card class="pa-4 mb-4" elevation="2">
      <v-row align="center">
        <v-col cols="9">
          <SearchBar
            :searchQuery="search"
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

    <!-- Alert quando não há estudantes -->
    <v-alert v-if="!loading && students.length === 0" type="info" class="mt-4">
      Nenhum aluno encontrado.
    </v-alert>

    <!-- Dialog de confirmação de exclusão -->
    <v-dialog v-model="deleteDialog" max-width="400px">
      <v-card>
        <v-card-title>Confirmação</v-card-title>
        <v-card-text>Tem certeza que deseja excluir este aluno?</v-card-text>
        <v-card-actions>
          <v-btn color="grey" variant="outlined" @click="deleteDialog = false">Cancelar</v-btn>
          <v-btn color="red" variant="outlined" @click="deleteStudent">Confirmar</v-btn>
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
  if (!search.value) return students.value
  
  const searchLower = search.value.toLowerCase()
  return students.value.filter(student => 
    student.name?.toLowerCase().includes(searchLower) ||
    student.email?.toLowerCase().includes(searchLower) ||
    student.ra?.toLowerCase().includes(searchLower) ||
    student.cpf?.includes(search.value)
  )
})

// Métodos
const loadStudents = async () => {
  loading.value = true
  try {
    students.value = await StudentService.getAllStudents()
    console.log('Estudantes carregados:', students.value)
  } catch (error) {
    console.error('Erro ao carregar estudantes:', error)
    snackbar.show = true
    snackbar.message = 'Erro ao carregar lista de estudantes'
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
  studentToDelete.value = studentId
  deleteDialog.value = true
}

const deleteStudent = async () => {
  if (!studentToDelete.value) return
      
  try {
    await StudentService.deleteStudent(studentToDelete.value.toString())
    await loadStudents() // Recarregar lista
    snackbar.show = true
    snackbar.message = 'Estudante excluído com sucesso!'
    snackbar.color = 'success'
  } catch (error) {
    console.error('Erro ao excluir estudante:', error)
    snackbar.show = true
    snackbar.message = 'Erro ao excluir estudante'
    snackbar.color = 'error'
  } finally {
    deleteDialog.value = false
    studentToDelete.value = null
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
  padding: 20px;
}
</style> 