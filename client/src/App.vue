<script setup lang="ts">
import { RouterView } from 'vue-router'
  import AppHeader from './components/AppHeader.vue';
</script>

<template>
  <AppHeader />
  <div class="router-content">
    <RouterView />
  </div>
</template>

<style lang="scss" scoped>
  .router-content{
    margin-left: 4rem;
    margin-right: 4rem;
  }
</style>
