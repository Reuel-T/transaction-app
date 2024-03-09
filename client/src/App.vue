<script setup lang="ts">
  import { RouterView } from 'vue-router'
  import AppHeader from './components/global/AppHeader.vue'
  import AppFooter from './components/global/AppFooter.vue'
</script>

<template>
  <AppHeader />
  <div class="router-content">
    <RouterView />
  </div>
  <AppFooter />
</template>

<style lang="scss" scoped>
  .router-content {
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
    flex-grow: 1;
  }
</style>
