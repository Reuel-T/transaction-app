<template>
  <div class="page-card">
    <slot></slot>
  </div>
</template>

<script lang="ts" setup>
</script>

<style lang="scss" scoped>
  .page-card {
    backdrop-filter: blur(1px);
    border-radius: 1rem;
    border: 1px solid rgb(48, 48, 48);
    box-shadow: 0.05em 0.05em 1em rgba(255, 255, 255, 0.1);
    height: 85vh;
    width: 85vw;
    max-width: 900px;

    h1 {
      margin: 0;
      padding: 0;
    }

    overflow: hidden;
  }

  h1.title {
    color: #cecece;
    letter-spacing: 0.075em;
    text-align: center;
  }
</style>