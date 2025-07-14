<template>
  <div>
    <p>Redirecionando...</p>
  </div>
</template>

<script setup>
import { navigateTo } from "#app";

navigateTo("/auth/login");
</script>
