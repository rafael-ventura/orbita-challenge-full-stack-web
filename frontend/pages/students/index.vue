<template>
  <v-container class="students-container">
    <h1 class="mb-4">Consulta de Alunos</h1>
    <v-card class="pa-4 mb-4" elevation="2">
      <v-row align="center">
        <v-col cols="9">
          <SearchBar
              :searchQuery="search"
              @update:searchQuery="search = $event"
          />
        </v-col>
        <v-col cols="3" class="d-flex justify-end">
          <v-btn color="red" @click="openAddStudentPage">Cadastrar Aluno</v-btn>
        </v-col>
      </v-row>
    </v-card>

    <v-progress-linear v-if="loading" indeterminate color="primary"></v-progress-linear>

    <StudentsTable
        :students="filteredStudents"
        :headers="headers"
        :loading="loading"
        :itemsPerPage="itemsPerPage"
        @editStudent="openEditStudentModal"
        @deleteStudent="confirmDeleteStudent"
    />

    <v-alert v-if="!loading && students.length === 0" type="info" class="mt-4">
      Nenhum aluno encontrado.
    </v-alert>

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
  </v-container>
</template>

<script setup lang="ts">
import {computed, onMounted, ref} from "vue";
import {useStudentApi} from "@/composables/useStudentApi";
import {useRouter} from "vue-router";
import {Student} from "@/models/Student";
import SearchBar from "@/components/SearchBar.vue";
import StudentsTable from "@/components/StudentsTable.vue";

const { fetchStudents, deleteStudent: deleteStudentApi } = useStudentApi();
const router = useRouter();

const students = ref<Student[]>([]);
const search = ref("");
const itemsPerPage = ref(10);
const loading = ref(false);
const deleteDialog = ref(false);
const editDialog = ref(false);
const studentToDelete = ref<number | null>(null);
const selectedStudent = ref<Student | null>(null);

const headers = [
  { title: "Registro Acadêmico", key: "ra" },
  { title: "Nome", key: "name" },
  { title: "Email", key: "email" },
  { title: "CPF", key: "cpf" },
  { title: "Ações", key: "actions", sortable: false },
];

const filteredStudents = computed(() => {
  if (!search.value) return students.value;

  return students.value.filter(student => {
    const name = student.name?.toLowerCase().trim() || "";
    const email = student.email?.toLowerCase().trim() || "";
    const query = search.value.toLowerCase().trim();
    const cpf = student.cpf?.replace(/\D/g, "") || "";
    const ra = student.ra?.toString() || "";

    return name.includes(query) || email.includes(query) || cpf.includes(query) || ra.includes(query);
  });
});


const loadStudents = async () => {
  loading.value = true;
  try {
    const result = await fetchStudents();
    students.value = Array.isArray(result) ? result : [];

    console.log("📥 Alunos carregados:", students.value);
  } catch (error) {
    console.error(" Erro ao carregar alunos:", error);
    students.value = [];
  } finally {
    loading.value = false;
  }
};

const openAddStudentPage = () => router.push("/students/register");

const openEditStudentModal = (student: Student) => {
  console.log("✏️ Editando aluno:", student);
  selectedStudent.value = { ...student };
  editDialog.value = true;
};

const confirmDeleteStudent = (studentId: number) => {
  console.log("🟡 Preparando para excluir aluno com ID:", studentId);
  studentToDelete.value = studentId;
  deleteDialog.value = true;
};

const deleteStudent = async () => {
  if (!studentToDelete.value) return;

  try {
    await deleteStudentApi(studentToDelete.value);
    console.log("Aluno excluído com sucesso.");
  } catch (error) {
    console.error(" Erro ao excluir aluno:", error);
  }

  deleteDialog.value = false;
  studentToDelete.value = null;
  await loadStudents();
};

onMounted(loadStudents);
</script>
