<template>
  <v-data-table
      v-if="!loading"
      :headers="headers"
      :items="students"
      class="elevation-1 custom-table"
      :items-per-page="itemsPerPage"
      item-value="id"
      :no-data-text="noDataText"
  >
    <template v-slot:item.ra="{ item }">
      {{ item.ra || "N/A" }}
    </template>

    <template v-slot:item.name="{ item }">
      {{ item.name || "Sem Nome" }}
    </template>

    <template v-slot:item.email="{ item }">
      {{ item.email || "Sem Email" }}
    </template>

    <template v-slot:item.cpf="{ item }">
      {{ formatCPF(item.cpf) || "Sem CPF" }}
    </template>

    <template v-slot:item.actions="{ item }">
      <div class="actions-column">
        <v-btn 
          icon="mdi-pencil" 
          variant="text" 
          color="primary"
          size="small"
          @click="editStudent(item)"
          title="Editar"
        ></v-btn>
        <v-btn 
          icon="mdi-delete" 
          variant="text" 
          color="error"
          size="small"
          @click="confirmDelete(item)"
          title="Excluir"
        ></v-btn>
      </div>
    </template>
  </v-data-table>
</template>

<script lang="ts" setup>
import { useRouter } from 'vue-router'
import { computed } from 'vue'
import type { Student } from '@/models/Student'
import { formatCPF } from '@/utils/formatHelper'

const props = defineProps<{
  students: Student[];
  headers: { title: string; key: string; sortable?: boolean }[];
  loading: boolean;
  itemsPerPage: number;
}>();

// Computed para o texto quando não há dados
const noDataText = computed(() => {
  return props.students.length === 0 ? 'Nenhum aluno encontrado' : 'Nenhum dado disponível'
})

const emit = defineEmits(["editStudent", "deleteStudent"]);

const router = useRouter();

const editStudent = (student: Student) => {
  router.push({ path: "/students/edit/" + student.id });
};

const confirmDelete = (student: Student) => {
  console.log('🗑️ Confirmando exclusão do estudante:', student)
  if (!student.id) {
    console.error('❌ ID do estudante não encontrado:', student)
    return
  }
  emit("deleteStudent", student.id);
};
</script>

<style lang="scss">
@import '@/assets/styles/variables.scss';

.custom-table {
  border: 2px solid $grey-light;
  border-radius: 8px;
  overflow: hidden;

  td {
    padding: 10px;
    text-align: left;
    font-size: 14px;
  }

  .actions-column {
    display: flex;
    justify-content: center;
    gap: 8px;
    
    .v-btn {
      transition: all 0.3s ease;
      
      &:hover {
        transform: scale(1.1);
      }
      
      &.v-btn--icon {
        width: 36px;
        height: 36px;
      }
    }
  }

  th {
    font-weight: bold;
    font-size: 16px;
    color: $white-text;
    background-color: red;
    text-align: center;
  }
}
</style> 