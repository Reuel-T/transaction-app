<template>
  <AppPageWrapper>
    <h1 class="title">Not Found</h1>
    <h3>Hmmm... Can't seem to find what you're looking for...</h3>
    <RouterLink
          to="/"
          role="button"
        >
          <button
            id="back"
            class="btn pill"
          >
            Back
          </button>
        </RouterLink>
  </AppPageWrapper>
</template>

<script setup lang="ts">
import AppPageWrapper from '@/components/global/AppPageWrapper.vue';
</script>

<style scoped></style>
