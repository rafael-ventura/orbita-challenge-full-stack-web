<template>
  <v-container class="fill-height d-flex flex-column align-center justify-center">
    <v-snackbar v-model="snackbar.show" :color="snackbar.color" timeout="4000">
      {{ snackbar.message }}
    </v-snackbar>

    <v-card class="form-card">
      <v-card-title class="form-title">
        <h2>Cadastro</h2>
      </v-card-title>
      <v-card-text>
        <v-form ref="form" v-model="valid">
          <v-text-field label="Nome" v-model="user.name" :rules="[rules.required, rules.minLength]" required/>
          <v-text-field label="E-mail" v-model="user.email" :rules="[rules.required, rules.email]" required/>
          <v-text-field
              label="Senha"
              v-model="user.password"
              :rules="[rules.required]"
              required
              type="password"
          />
        </v-form>
      </v-card-text>
      <v-card-actions class="d-flex justify-space-between">
        <v-btn color="grey" variant="outlined" @click="goToLogin"> Já tenho uma conta </v-btn>
        <v-btn color="primary" variant="outlined" :disabled="!valid" class="btn-register" @click="registerUser">
          Cadastrar
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-container>
</template>

<script setup lang="ts">
import {useAuthApi} from "~/composables/useAuthApi";
import {useRouter} from "vue-router";
import {useNotification} from "@/composables/useNotification";

const { register } = useAuthApi();
const router = useRouter();
const { snackbar, showNotification } = useNotification();

const form = ref<any>(null);
const valid = ref(false);
const user = ref({
  name: "",
  email: "",
  password: "",
});

const rules = {
  required: (v: string) => !!v || "Este campo é obrigatório.",
  email: (v: string) => /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/.test(v) || "E-mail inválido.",
  minLength: (v: string) => v.length >= 2 || "O nome deve ter pelo menos 2 caracteres.",
};

const registerUser = async () => {
  if (!form.value?.validate()) return;

  try {
    await register(user.value);
    showNotification("Cadastro realizado com sucesso!", "success");

    await router.push({
      path: "/auth/login",
      query: {
        email: user.value.email,
        password: user.value.password,
      },
    });
  } catch (error: any) {
    showNotification(error?.response?._data?.error, "error");
  }
};

const goToLogin = () => {
  router.push("/auth/login");
};
</script>

<style scoped lang="scss">
@use "@/styles/app.scss";

.fill-height {
  height: 100vh;
}

.form-card {
  width: 80%;
  max-width: 90%;
  padding: 3%;
}

.form-title {
  font-size: 1.7rem;
  font-weight: bold;
  text-align: center;
}

.btn-register {
  background-color: #c8102e !important;
  color: white !important;
  font-weight: bold;
  padding: 0 24px;
  transition: background 0.3s;

  &:hover {
    background-color: rgba(#c8102e, 0.8) !important;
  }

  &:disabled {
    background-color: #cccccc !important;
    color: #888888 !important;
  }
}
</style>
