<template>
  <v-data-table
      v-if="!loading"
      :headers="headers"
      :items="students"
      class="elevation-1 custom-table"
      :items-per-page="itemsPerPage"
      item-value="id"
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
        <v-btn icon="mdi-pencil" variant="text" @click="editStudent(item)"></v-btn>
        <v-btn icon="mdi-delete" variant="text" @click="confirmDelete(item)"></v-btn>
      </div>
    </template>
  </v-data-table>
</template>

<script lang="ts" setup>
import { useRouter } from 'vue-router'
import type { Student } from '@/models/Student'
import { formatCPF } from '@/utils/formatHelper'

defineProps<{
  students: Student[];
  headers: { title: string; key: string; sortable?: boolean }[];
  loading: boolean;
  itemsPerPage: number;
}>();

const emit = defineEmits(["editStudent", "deleteStudent"]);

const router = useRouter();

const editStudent = (student: Student) => {
  router.push({ path: "/students/edit/" + student.id });
};

const confirmDelete = (student: Student) => {
  if (!student.id) return;
  emit("deleteStudent", Number(student.id));
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
    justify-content: left;
    gap: 10px;
    margin-left: -6%;
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