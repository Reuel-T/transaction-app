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
    margin-left: 4rem;
    margin-right: 4rem;
    flex-grow: 1;

    padding-top: 2rem;
    padding-bottom: 2rem;
  }
</style>../components/global/AppHeader.vue/components/global/AppFooter.vue
