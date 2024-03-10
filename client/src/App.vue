<script setup lang="ts">
  import { RouterView } from 'vue-router'
  import AppHeader from './components/global/AppHeader.vue'
  import AppFooter from './components/global/AppFooter.vue'
</script>

<template>
  <AppHeader />

  <div class="router-content">
    <RouterView v-slot="{ Component, route }">
      <Transition
        :name="route.meta.transition ? 'route-back' : 'route'"
        mode="out-in"
      >
        <component :is="Component"></component>
      </Transition>
    </RouterView>
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

  /* route transitions */
  .route-enter-from {
    opacity: 0;
    transform: translateX(100px);
  }
  .route-enter-active {
    transition: all 0.3s ease-out;
  }
  .route-leave-to {
    opacity: 0;
    transform: translateX(-100px);
  }
  .route-leave-active {
    transition: all 0.3s ease-in;
  }

  .route-back-enter-from {
    opacity: 0;
    transform: translateX(-100px);
  }

  .route-back-enter-active {
    transition: all 0.3s ease-out;
  }

  .route-back-leave-to {
    opacity: 0;
    transform: translateX(100px);
  }

  .route-back-leave-active {
    transition: all 0.3s ease-in;
  }
</style>
